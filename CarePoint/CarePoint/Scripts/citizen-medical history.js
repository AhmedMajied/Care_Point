/*Authors: Ahmed Hussein, Andrew Emad */

$(document).ready(function () {
    $(".panel").hide();
    $('#itbl-medical-history').DataTable({
        "order": [[0, "desc"]],
        "iDisplayLength": 10,
        "bPaginate": true,
        "sPaginationType": "full_numbers"
    });
});

//Show medical history a table
function populatePanel(img, medicalPlaceName, specialistName, date, symptom, disease, medicine, remarks) {
    $(".panel").show();
    var symptoms = symptom.split(',');
    var diseases = disease.split(',');
    var medicines = medicine.split(',');
    $("#iul-symptoms").empty();
    for (var i = 0; i < symptoms.length; ++i) {
        $("#iul-symptoms").append($("<li>").text(symptoms[i]));
    }
    $("#iul-diseases").empty();
    for (var i = 0; i < diseases.length; ++i) {
        $("#iul-diseases").append($("<li>").text(diseases[i]));
    }
    $("#iul-medicines").empty();
    for (var i = 0; i < medicines.length; ++i) {
        $("#iul-medicines").append($("<li>").text(medicines[i]));
    }

    $("#iimg-medical-place").attr("src", "data:image/gif;base64," + img);
    $("#idiv-record-header").html(medicalPlaceName + ' <br/> ' + specialistName + ' <br/> ' + date);
    $("#remarks").text(remarks);

}