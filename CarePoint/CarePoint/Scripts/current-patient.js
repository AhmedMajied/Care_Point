var drugs;
var attachmentTypes;

$(document).ready(function () {
    $(function () {
        $(document).on('change', ':file', function () {
            var input = $(this);
            var fileName = input.val().replace(/\\/g, '/').replace(/.*\//, '');
            input.trigger('fileselected', fileName);
        });

        $(':file').on('fileselected', function (event, fileName) {
            $(this).parents('.input-group').find(':text').val(fileName);
        });
    });

    $(".cbtn-add").click(function () {
        var empty_input = false;
        // fields validation
        $(this).closest('.cdiv-list').find('input').each(function () {
            if ($(this).val().trim() == "" && $(this).hasClass('cinp-dose') == false) {
                $(this).addClass('input-error');
                empty_input = true;
            }
            else {
                $(this).removeClass('input-error');
            }
        });
        if (empty_input == false) {
            var copy = $(this).closest('.row').clone(true);
            copy.find("input").val("");
            $(this).addClass('hidden');
            $(this).prev().removeClass('hidden');
            $(this).closest('.container-fluid').append(copy);
        }
    });

    $('.btn-danger').click(function () {
        $(this).closest('.row').remove();
    });

    $("#ibtn-add-prescription").click(function () {

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
});



