$(document).ready(function () {
    popoverContent = `<div id='idiv-sos-pop'>
                        <form action='#' onsubmit='return false;'>
                            <textarea class='form-control input-lg ctextarea-description' placeholder="What's wrong ?!" rows=10 cols=25 style='resize: none;'></textarea>
                            <span class='cspan-description-error'></span>
                            <hr>
                            <label class="text-danger">Send to:</label><br>
                            <span class="cspn-radio-chck">
                                <input id='ichk-hospitals' class='cchck-danger' type='checkbox' checked>
                                <label for='ichk-hospitals'>Hospitals</label>
                            </span>
                            <span class="cspn-radio-chck">
                                <input id='ichk-family' class='cchck-danger' type='checkbox'>
                                <label for='ichk-family'>Family</label>
                            </span>
                            <span class="cspn-radio-chck">
                                <input id='ichk-friends' class='cchck-danger' type='checkbox'>
                                <label for='ichk-friends'>Friends</label>
                            </span>
                            <span class='cspan-error-send'></span>
                            <input type='submit' value='Send' id='iisend-sos' onclick='sendSOS()' class='btn btn-danger' style='width: 100%; margin-top: 1em;'>
                        </form>
                    </div>`;
    $('#ibtn-sos-pop').popover({
        html: true,
        container: 'body',
        content: popoverContent
    });
});
function getUserLocation() {
    return $.getJSON("http://freegeoip.net/json/").then(function (data) {
        return {
            latitude: data.latitude,
            longitude: data.longitude
        }
    });
}
function sendSOS() {
    var IsMedicalPlace = $("#ichk-hospitals").is(':checked');
    var IsFamily = $("#ichk-family").is(':checked');
    var IsFriend = $("#ichk-friends").is(':checked');
    var Description = $(".ctextarea-description").val();
    var Latitude, Longitude;
    // to get current location of user
    getUserLocation().then(function (data) {
        Latitude = data.latitude;
        Longitude = data.longitude;
        if ((Description != "") && (IsMedicalPlace || IsFriend || IsFamily)) {
        var model = {
            isMedicalPlace: IsMedicalPlace, isFamily: IsFamily,
            isFriend: IsFriend, description: Description,
            latitude: Latitude, longitude: Longitude
            }
        $("#iisend-sos").prop("disabled", true);
        $.ajax({
            type: 'POST',
            url: '/SOS/SendSos',
            data: { model },
            dataType: 'json',
            success: function (data) {
//                alert(data);
                $("#iisend-sos").prop("disabled", false);
            },
            error: function (msg) {
                alert("Sorry an Error happened please try again !");
                $("#iisend-sos").prop("disabled", false);
            }
        });
    }
    else {
        if (Description == "") {
            $(".cspan-description-error").text("please fill What's Wrong field").css("color","red");
            $('.cspan-description-error').fadeIn('fast').delay(5000).fadeOut('slow');

        }
        if (!(IsMedicalPlace || IsFriend || IsFamily)) {
            $(".cspan-error-send").text("select at least one option").css("color", "red");
            $('.cspan-error-send').fadeIn('fast').delay(5000).fadeOut('slow');
        }
    }
    });
}