$(document).ready(function () {
    $("#iselect-speciality").on('change', function () {
        if (this.value > 0) {
            $("#ifile-license").show();
            $("#ilbl-license").show();
        } else{
            $("#ifile-license").hide();
            $("#ilbl-license").hide();
        }
    });
});