
/* Author: Ahmed Hussein */

$.validator.setDefaults({
    /*This enforces validation even if the element is hidden*/
    ignore: ":hidden:not('.cfile-enforce-validation')"
});

$(document).ready(function () {

    //Write the name of the chosen file
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

    //Ensure that license is required only from specialists, not regular citizens
    $("#iselect-speciality").on('change', function () {
        if (this.value > 0) {
            $("#idiv-license").removeClass("hidden");
        } else{
            $("#idiv-license").addClass("hidden");
        }
    });
});