var drugs;
var attachmentTypes;

function updateIDSerial(oldID) {
    var serial = oldID.split('-').pop(), newID;

    if (serial && !isNaN(serial)) {
        newID = oldID.substring(0, oldID.lastIndexOf("-") + 1) + (parseInt(serial) + 1);
    } else {
        newID = oldID + "-1";
    }

    return newID;
}

function uploadAllowed() {
    var allowed = true;
    $('#imodal-upload-attachment .cdiv-list').find("input, select").each(function () {
        if ($(this).is("select:has(> option:selected:disabled)") || $(this).val().trim() == "") {
            allowed = false;
        }
    });

    return allowed;
}

$(document).ready(function () {
    $('input, textarea, select').on('focus', function () {
        $(this).removeClass('input-error');
    });

    $(function () {
        $(document).on('change', ':file', function () {
            var input = $(this);
            var fileName = input.val().replace(/\\/g, '/').replace(/.*\//, '');
            input.trigger('fileselected', fileName);

            if (uploadAllowed()) {
                $('#ibtn-upload').prop('disabled', false);
            } else {
                $('#ibtn-upload').prop('disabled', true);
            }
        });

        $(':file').on('fileselected', function (event, fileName) {
            $(this).parents('.input-group').find(':text').val(fileName);
        });
    });

    $('.cselect-attachment-type').change(function () {
        if (uploadAllowed()) {
            $('#ibtn-upload').prop('disabled', false);
        } else {
            $('#ibtn-upload').prop('disabled', true);
        }
    });

    $(".cbtn-add").click(function () {
        $('#ibtn-upload').prop('disabled', true);
        var originalElement = $(this).closest('.row');
        var empty_input = false;
        // fields validation
        $(this).closest('.cdiv-list').find("input, select").each(function () {
            if ($(this).is("select:has(> option:selected:disabled)") ||$(this).val().trim() == "") {
                $(this).addClass('input-error');
                empty_input = true;
            }

            else {
                $(this).removeClass('input-error');
            }
        });

        if (empty_input == false) {
            var copy = originalElement.clone(true).addClass("c-dirty"); // c-dirty class is used when the modal is reset
            copy.find("input:not([type='radio']):not([type='checkbox']):not([type='button']):not([type='submit']), textarea, select").val("");
            copy.find("[type=checkbox]").prop('checked', false);
            copy.find("[type=radio]").prop('checked', false);
            copy.find("input").each(function () {
                var oldID = $(this).attr('id'), newID;

                if (oldID) {
                    newID = updateIDSerial(oldID);
                    $(this).attr('id', newID);
                }
            });

            copy.find("input[type='checkbox']").each(function () {
                var oldVal = $(this).attr('value'), newVal;
                if (oldVal) {
                    oldVal = parseInt(oldVal);
                    if (!isNaN(oldVal)) {
                        newVal = oldVal + 1;
                        $(this).attr('value', newVal);
                    }
                }
            });

            copy.find("label").each(function () {
                var oldFor = $(this).attr('for'), newFor;

                if (oldFor) {
                    newFor = updateIDSerial(oldFor);
                    $(this).attr('for', newFor);
                }
            });

            $(this).addClass('hidden');
            $(this).prev().removeClass('hidden');
            $(this).closest('.container-fluid').append(copy);
        }
    });

    $('.cbtn-remove').click(function () {
        var targetedItem = $(this).closest('.row');
        if ( ! targetedItem.hasClass("c-dirty")) {
            targetedItem.next().removeClass("c-dirty"); //to guarantee that exactly one item does NOT have the class c-dirty
        }
        targetedItem.remove();
    });

    $("#ibtn-add-prescription").click(function () {
        //Reset step wizard progress
        $("#ibtn-submit").css('display', 'none');
        $("#ibtn-prev").css('display', 'none');
        $("#ibtn-nxt").css('display', 'inline-block');
        $("#ibtn-nxt, #ibtn-prev, #ibtn-submit").prop("disabled", true);
        var progress_line = $('.modal-header').find('.f1-progress-line');
        var parent_fieldset = $('.f1 .cdiv-step:nth-child(' + current_step + ')');
        current_step = 1;
        $("#imodal-history-record .modal-header").find(".f1-step").removeClass("activated active");
        $("#imodal-history-record .modal-header").find(".f1-step:first").addClass("active");
        bar_progress(progress_line, null);
        parent_fieldset.fadeOut(400, function () {
            $('.f1 .cdiv-step:nth-child(1)').fadeIn(function () {
                update_step_nav_buttons();
            });
        });

        //Load drugs
        if (drugs == null) {
            $.ajaxSetup({ async: false });
            $.post("/Medicine/GetAllMedicines", {}, function (data) {
                var datalist = $("#drugs");
                drugs = data;

                for (var i = 0; i < drugs.length; i++) {
                    var option = document.createElement("option");
                    option.value = drugs[i].Name;
                    datalist.append(option);
                }
            });
        }
    });

    $("#ibtn-upload-attachment").click(function () {
        $('#ibtn-upload').prop('disabled', true);
        if (attachmentTypes == null) {
            $.ajaxSetup({ async: false });
            $.post("/MedicalHistory/GetAttachmentTypes", {}, function (data) {
                var selectList = $("#iselect-attachment-types");
                attachmentTypes = data;
                
                for (var i = 0; i < attachmentTypes.length-1; i++) {
                    var option = document.createElement("option");
                    option.value = attachmentTypes[i].ID;
                    option.text = attachmentTypes[i].Name;
                    selectList.append(option);
                }
            });
        }
    });

    //Reset modals when closed
    $("#imodal-history-record, #imodal-upload-attachment").on("hidden.bs.modal", function () {
        $(this).find(".c-dirty").remove();
        $(this).find("input:not([type='radio']):not([type='checkbox']):not([type='button']):not([type='submit']):not(.cinp-unresettable), textarea, select").val("");
        $(this).find("[type=checkbox]").prop('checked', false);
        $(this).find("[type=radio]").prop('checked', false);
        $(this).find(".cbtn-remove").addClass("hidden");
        $(this).find(".cbtn-add").removeClass("hidden");
        $(this).find("input").removeClass("input-error");
        $(this).find(".alert").addClass("hidden");
    });

    $("#iForm-prescription").submit(function () {
        $("#imodal-saving").modal('show');
        event.preventDefault();
        var url = $(this).attr("action");
        
        $.post(url, $(this).serialize()).done(function (fileName) {
            $("#imodal-saving").modal('hide');
            $('#imodal-history-record').modal('hide');
            window.location = "/MedicalHistory/DownloadPrescription?fileName=" + fileName;
        });

    });
});