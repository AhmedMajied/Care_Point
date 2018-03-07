$(document).ready(function () {
    $("#ilink-features").on('click', function(){
        var targetScrollPosition = $("#idiv-middle").offset().top;
        animateScroll(targetScrollPosition); 
    });

    $("#ilink-support").on('click', function(){
        var targetScrollPosition = $("#idiv-bottom").offset().top;
        animateScroll(targetScrollPosition);
    });
});

function animateScroll(targetScrollPosition){
    var currentTop = $(window).scrollTop(), rate = 2;
    var distance = Math.abs(currentTop - targetScrollPosition);
    var scrollDuration = distance / rate;
    $("HTML, BODY").animate({scrollTop: targetScrollPosition }, scrollDuration);
}