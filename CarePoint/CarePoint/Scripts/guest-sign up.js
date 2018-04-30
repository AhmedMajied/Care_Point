$.validator.setDefaults({
    /*This enforces validation even if the element is hidden*/
    ignore: ":hidden:not('.cfile-enforce-validation')"
});
$(document).ready(function () {
    var speciality = $("#iselect-speciality").value;
    if (speciality > 0) {
        $("#ifile-license").show();
        $("#ilbl-license").show();
        $("#ifile-license").prop("required", true);
    }
    $("#iselect-speciality").on('change', function () {
        if (this.value > 0) {
            $("#ifile-license").show();
            $("#ilbl-license").show();
            $("#ifile-license").prop("required", true);
        }
        else {
            $("#ifile-license").hide();
            $("#ilbl-license").hide();
            $("#ifile-license").prop("required",false);
        }
    });
});