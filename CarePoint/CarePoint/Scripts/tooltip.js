$(document).ready(function(){
    $('[data-toggle="tooltip"]').tooltip({
        'delay': { show: 1000, hide: 0 },
        'trigger': "hover"
    });

    $('[data-toggle="tooltip"]').click(function () {
        $('[data-toggle="tooltip"]').tooltip("hide");
     });
});