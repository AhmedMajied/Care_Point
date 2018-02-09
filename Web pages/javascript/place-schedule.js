$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip({
        'delay': { show: 1000, hide: 200 }
    });

    $("#iselect-service").on('change', function () {
        if (this.value == 0) {
            $("#iselect-workday").prop('disabled', true);
            $("#ibtn-plus").prop('disabled', true);
        } else{
            $("#iselect-workday").prop('disabled', false);
            $("#ibtn-plus").prop('disabled', false);
        }
    });

    $(function () {
        $('.cinp-time').clockface({
            format: 'HH:mm',
            trigger: 'manual'
        });

        $('.cdiv-time').click(function (e) {
            e.stopPropagation();
            $('.cinp-time').clockface('hide');
            $(this).find('.cinp-time').clockface('toggle');
        });
    });
});