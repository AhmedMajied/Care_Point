/* Author: Ahmed Hussein */

$(document).ready(function () {
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