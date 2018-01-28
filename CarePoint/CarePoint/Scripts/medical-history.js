function populatePanel(img,medicalPlaceName,specialistName,date,symptom, disease, medicine, remarks) {
    var symptoms = symptom.split(',');
    var diseases = disease.split(',');
    var medicines = medicine.split(',');
    $("#iul-symptoms").empty();
    for (var i= 0; i < symptoms.length;++i){
        $("#iul-symptoms").append($("<li>").text(symptoms[i]));
    }
    $("#iul-diseases").empty();
    for (var i= 0; i < diseases.length;++i){
        $("#iul-diseases").append($("<li>").text(diseases[i]));
    }
    $("#iul-medicines").empty();
    for (var i = 0; i < medicines.length; ++i){
        $("#iul-medicines").append($("<li>").text(medicines[i]));
    }
    $(".panel-heading row").empty();
    $(".panel-heading row").append('<div class="col-md-2">' + img + '</div>');
    $(".panel-heading row").append('<div class="col-md-6">' + medicalPlaceName + '<br/>' + specialistName + '<br/>' + date + '</div>');
    $("#remarks").text(remarks);

}