$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();

    $("#iselect-service").on('change', function () {
        if (this.value == 0) {
            $("#iselect-workday").prop('disabled', true);
            $("#ibtn-plus").prop('disabled', true);
        } else{
            $("#iselect-workday").prop('disabled', false);
            $("#ibtn-plus").prop('disabled', false);
        }
    });
});