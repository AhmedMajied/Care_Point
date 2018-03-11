$(document).ready(function () {
    $('#iinp-srch').on('input', function (e) {
        var srch_query = $(this).val();
        var patient_names = $('div.active .clbl-name');
        for (var i = 0; i < patient_names.length; i++) {
            var regex = ".*" + srch_query.split("").join(".*") + ".*";
            if (patient_names[i].textContent.search(regex) == -1) {
                $(patient_names[i]).closest('.row').css('display', 'none').next('hr').css('display', 'none');
            }
            else {
                $(patient_names[i]).closest('.row').css('display', 'block').next('hr').css('display', 'block');
            }
        }
    });
});


