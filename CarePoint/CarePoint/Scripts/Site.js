/* Author: Ahmed Hussein */

$(document).ready(function() {
    $('button').on('focus', function() {
        $(this).blur();
    })

    $('#iul-work-menu').hover(function () {
        event.stopPropagation();
    });
});