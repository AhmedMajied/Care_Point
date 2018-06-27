$(document).ready(function () {
    $('.tree-node-header').on('click', function () {
        $(this).closest('.tree-node').find('.tree-node-body').toggle();

        var nodeIcon = $(this).find('.tree-node-icon');
        if ((nodeIcon).hasClass('fa-plus')) {
            nodeIcon.removeClass('fa-plus');
            nodeIcon.addClass('fa-minus');
        } else {
            nodeIcon.removeClass('fa-minus');
            nodeIcon.addClass('fa-plus');
        }
    });
});