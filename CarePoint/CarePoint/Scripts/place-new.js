$.validator.setDefaults({
    /*This enforces validation even if the element is hidden*/
    ignore: ":hidden:not('.cfile-enforce-validation')"
});
$(document).ready(function () {
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
});