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

$("#ibtn-close-patient-list").click(function () {
    $("#itab-females .row").remove();
    $("#itab-females hr").remove();
    $("#itab-males .row").remove();
    $("#itab-males hr").remove();
});

function PatientsModalFill(parent, usrName, img, showHistoryHref) {
    if (img == null) {
        img = '../Images/notfound.png';
    }
    html = $("<div class='row'>").append("<div class='col-sm-2 cdiv-vcenter'><img class='cimg-user' src='" + img + "'/></div>")
            .append($("<div class='col-sm-6 cdiv-vcenter'><label class='clbl-name'>" + usrName + "</label></div>"))
            .append("<div class='col-sm-4 cdiv-vcenter'><a href =" + showHistoryHref + "><button class='btn btn-default'>Open medical history</button></a></div>");
    $(parent).append(html);
    $(parent).append("<hr>");
}
$(function () {
    $("#ilink-patient-list").click(function () {
        $("#imodal-patient-list button.close").prop('disabled', true);
        $("#imodal-patient-list .cdiv-custom-alert").addClass('hidden');
        $("#imodal-patient-list #itab-males, #imodal-patient-list #itab-females").append('<span class="cspn-proxy"><span class="cspn-loader"></span><br />Loading...</span>');
        var docId = $("#iinput-usr").val();
        $.ajax({
            type: 'POST',
            url: '/Citizen/PatientsList',
            data: { doctorId: docId },
            dataType: 'json',
            success: function (data) {
                var males = data[0];
                var malescount = males.length;
                
                for (var i = 0; i < malescount; i++) {
                    PatientsModalFill("#itab-males", males[i].Name, males[i].Photo, "/Citizen/CurrentPatient?citizenID=" + males[i].Id);
                }
                $("#imodal-patient-list #itab-males .cspn-proxy").remove();
                if (malescount == 0) {
                    $("#imodal-patient-list #itab-males .cdiv-custom-alert").removeClass('hidden');
                }
                var females = data[1];
                var femalescount = females.length;
                for (var i = 0; i < femalescount; i++) {
                    PatientsModalFill("#itab-females", females[i].Name, females[i].Photo, "/Citizen/CurrentPatient?citizenID=" + females[i].Id);
                }
                $("#imodal-patient-list #itab-females .cspn-proxy").remove();
                if (femalescount == 0) {
                    $("#imodal-patient-list #itab-females .cdiv-custom-alert").removeClass('hidden');
                }

                $("#imodal-patient-list button.close").prop('disabled', false);
            },
            error: function (msg) {
                console.log(JSON.stringify(msg));
            }
        });
    });
});
