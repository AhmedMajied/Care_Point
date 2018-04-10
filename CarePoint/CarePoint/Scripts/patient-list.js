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

$("#patient-list-modal-button").click(function () {
    $("#itab-females .row").remove();
    $("#itab-females hr").remove();
    $("#itab-males .row").remove();
    $("#itab-males hr").remove();
});

function PatientsModalFill(parent, usrName,img, showHistoryHref) {
    if (img == null)
        img = "../Images/notfound.png";
    html = $("<div class='row'>").append("<div class='col-md-2 cdiv-vcenter'><img class='cimg-user' src=" + img + "/></div>")
        .append($("<div class='col-md-5 cdiv-vcenter'><label class='clbl-name'>" + usrName + "</label></div>")).append("<div class='col-md-4 cdiv-vcenter'><a href =" + showHistoryHref + "><button class='btn btn-default'>Open history</button></a></div>");
    $(parent).append(html);
    $(parent).append("<hr>");
}

function openModal() {
    var docId = $("#iinput-usr").val();
    $.ajax({
        type: 'POST',
        url: '/Citizen/PatientsList',
        data: { doctorId: docId },
        dataType: 'json',
        success: function (data) {
            var femalecount = data.femaleCount;
            for (var i = 0; i < femalecount; i++) {
                PatientsModalFill("#itab-females", data.female[i].Name, data.female[i].Photo, "/Citizen/CurrentPatient/"+data.female[i].Id);
            }
            var malecount = data.maleCount;
            for (var i = 0; i < malecount; i++) {
                PatientsModalFill("#itab-males", data.male[i].Name, data.male[i].Photo, "/Citizen/CurrentPatient/" + data.male[i].Id);
            }
            $("#imodal-patient-list").modal('show');
        },
        error: function (msg) {
            console.log(JSON.stringify(msg));
        }
    });
}