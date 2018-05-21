current_step = 1;

function bar_progress(progress_line_object, direction) {
    var number_of_steps = progress_line_object.data('number-of-steps');
    var now_value = progress_line_object.data('now-value');
    var new_value = 0;
    if (direction == 'right') {
        new_value = now_value + (100 / number_of_steps);
    }
    else if (direction == 'left') {
        new_value = now_value - (100 / number_of_steps);
    }
    else {
        new_value = 12.5;
    }
    progress_line_object.attr('style', 'width: ' + new_value + '%;').data('now-value', new_value);
}

function update_step_nav_buttons() {
    if (current_step == 1) {
        $("#ibtn-nxt").css('display', 'inline-block');
        $("#ibtn-nxt").prop('disabled', false);
        $("#ibtn-prev").css('display', 'none');
        $("#ibtn-submit").css('display', 'none');
    }
    else if (current_step == $('.f1').children().length) {
        $("#ibtn-nxt").css('display', 'none');
        $("#ibtn-prev").css('display', 'inline-block');
        $("#ibtn-prev").prop('disabled', false);
        $("#ibtn-submit").css('display', 'inline-block');
        $("#ibtn-submit").prop('disabled', false);
    }
    else {
        $("#ibtn-nxt").css('display', 'inline-block');
        $("#ibtn-nxt").prop('disabled', false);
        $("#ibtn-prev").css('display', 'inline-block');
        $("#ibtn-prev").prop('disabled', false);
        $("#ibtn-submit").css('display', 'none');
    }
}

function createAlternativesDiv() {
    var patientDrugs = $("input[name=drugName]");
    var alternativesDiv = $("#idiv-alternatives");
    var alternatives;
    var foundAlternatives = 0;
    var patientDrugName;
    var wrapperDiv = document.createElement("div");

    wrapperDiv.id = "idiv-alternatives-wrapper";
    wrapperDiv.className = "c-dirty";

    for (var i = 0; i < patientDrugs.length; i++) {
        patientDrugName = patientDrugs.eq(i).val();

        if (patientDrugName === "" || patientDrugName === " ")
            continue;

        alternatives = getMedicineAlternatives(patientDrugName);

        if (alternatives.length > 0) {
            foundAlternatives++;

            var panel = document.createElement("div");
            var panelBody = document.createElement("div");
            var panelHeading = document.createElement("div");

            // heading
            panelHeading.innerHTML = "<b>Alternatives for :  </b>" + patientDrugName;
            panel.appendChild(panelHeading);

            // body
            for (var alternativeIndex = 0; alternativeIndex < alternatives.length; alternativeIndex++) {
                var medicineAlternativeSpan = document.createElement("span");
                medicineAlternativeSpan.className = "cspn-alternative cspn-radio-chck";
                medicineAlternativeSpan.innerHTML = `
                    <input id="iinp-` + i + "-" + alternativeIndex + `" class="cchck-secondary" name="medicineAlternativeFor` + i +
                    `" value="` + alternatives[alternativeIndex].Name + `" type="checkbox" />
                    <label for="iinp-` + i + "-" + alternativeIndex + `">` + alternatives[alternativeIndex].Name + `</label>`;

                var medicineAlternativeLabel = document.createTextNode(alternatives[alternativeIndex].Name);
                medicineAlternativeLabel.htmlFor = "medicineAlternativeFor" + alternativeIndex;

                panelBody.appendChild(medicineAlternativeSpan);
            }

            panel.appendChild(panelHeading);
            panel.appendChild(panelBody);

            wrapperDiv.append(panel);

            // Div style 
            panel.className = "panel panel-default";
            panelBody.className = "panel-body";
            panelHeading.className = "panel-heading";
        }
    }

    if (foundAlternatives != 0) { //if there are any alternatives for any drug
        var alternativesHdr = document.createElement("h4");
        alternativesHdr.innerHTML = ("Which of the following drugs do you accept as alternatives ?");
        alternativesHdr.id = "ih4-alternatives";
        wrapperDiv.prepend(alternativesHdr);
        alternativesDiv.append(wrapperDiv);
    }
}

function getMedicineAlternatives(drugName) {
    var alternatives;

    if (drugName !== "") {
        $.ajaxSetup({ async: false });
        $.post("/Medicine/GetMedicineAlternatives", { medicineName: drugName }, function (data) {
            alternatives = data;
        });
    }
    return alternatives;
}

function validateDrugsNames() {
    var patientDrugs = $("input[name=drugName]");
    var found = false;

    // remove existing alternatives if exist
    $("#idiv-alternatives-wrapper").remove();

    for (var writtenDrugIndex = 0; writtenDrugIndex < patientDrugs.length && patientDrugs.eq(writtenDrugIndex).val() !== ""
        ; writtenDrugIndex++) {

        found = false;
        for (var drugIndex = 0; drugIndex < drugs.length; drugIndex++) {
            if (patientDrugs.eq(writtenDrugIndex).val() === drugs[drugIndex].Name) {
                found = true;
                break;
            }
        }

        if (!found) {
            return false;
        }
    }

    return true;
}

jQuery(document).ready(function () {
    $("#ibtn-add-prescription").on("click", function reset_progress() {
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
    });

    //Form
    $('.f1 .cdiv-step:first').fadeIn('slow');

    $('.f1 input[type="text"], .f1 input[type="password"], .f1 textarea').on('focus', function () {
        $(this).removeClass('input-error');
    });

    // next step
    $('#ibtn-nxt').on('click', function () {
        $("#ibtn-nxt, #ibtn-prev, #ibtn-submit").prop("disabled", true);
        var parent_fieldset = $('.f1 .cdiv-step:nth-child(' + current_step + ')');
        var next_step = true;
        // navigation steps / progress steps
        var current_active_step = $('.modal-header').find('.f1-step.active');
        var progress_line = $('.modal-header').find('.f1-progress-line');

        if (next_step) {

            parent_fieldset.fadeOut(400, function () {
                //update current step
                current_step++;

                // search for drugs alternatives
                if (current_step === 4) {
                    if (validateDrugsNames()) {
                        $("#idiv-warning").addClass("hidden");
                        createAlternativesDiv();
                    } else {
                        $("#idiv-warning").removeClass("hidden");
                    }
                }
                // change icons
                current_active_step.removeClass('active').addClass('activated').next().removeClass('activated').addClass('active');
                // progress bar
                bar_progress(progress_line, 'right');
                // show next step
                $(this).next().fadeIn(function () {
                    update_step_nav_buttons();
                });
            });
        }
    });

    // previous step
    $('#ibtn-prev').on('click', function () {
        $("#ibtn-nxt, #ibtn-prev, #ibtn-submit").prop("disabled", true);
        // navigation steps / progress steps
        var current_active_step = $('.modal-header').find('.f1-step.active');
        var progress_line = $('.modal-header').find('.f1-progress-line');
        $('.f1 .cdiv-step:nth-child(' + current_step + ')').fadeOut(400, function () {
            //update current step
            current_step--;
            // change icons
            current_active_step.removeClass('active').addClass('activated').prev().removeClass('activated').addClass('active');
            // progress bar
            bar_progress(progress_line, 'left');
            // show previous step
            $(this).prev().fadeIn(function () {
                update_step_nav_buttons();
            });
        });
    });

    $('.f1-step-icon').on('click', function (e) {

        e.preventDefault();
        $("#ibtn-nxt, #ibtn-prev, #ibtn-submit").prop("disabled", true);

        var selected_step = $(this).parent();
        if (selected_step.hasClass('activated')) {
            var parent_fieldset = $('.f1 .cdiv-step:nth-child(' + current_step + ')');
            // navigation steps / progress steps
            var current_active_step = $('.modal-header').find('.f1-step.active');
            var progress_line = $('.modal-header').find('.f1-progress-line');
            var temp = current_step;
            parent_fieldset.fadeOut(400, function () {
                //update current step
                current_step = selected_step.prevUntil('.f1-steps').length;

                // change icons
                current_active_step.removeClass('active').addClass('activated').next();
                selected_step.removeClass('activated').addClass('active');

                // progress bar
                if (temp < current_step) {
                    while (temp < current_step) {
                        bar_progress(progress_line, 'right');
                        temp++;
                    }
                }
                else {
                    while (temp > current_step) {
                        bar_progress(progress_line, 'left');
                        temp--;
                    }
                }

                // search for drugs alternatives
                if (current_step === 4) {
                    if (validateDrugsNames()) {
                        $("#idiv-warning").addClass("hidden");
                        createAlternativesDiv();
                    } else {
                        $("#idiv-warning").removeClass("hidden");
                    }
                }

                // show chosen step
                $('.f1 .cdiv-step:nth-child(' + current_step + ')').fadeIn(function () {
                    update_step_nav_buttons();
                });
            });
        }
    });
});