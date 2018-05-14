$(document).ready(function () {
    popoverContent = `<div id='idiv-sos-pop'>
                        <form action="#" onsubmit="return false;">
                            <textarea class='form-control input-lg ctextarea-description' placeholder="What's wrong ?!" rows=10 cols=25 style='resize: none;'></textarea>
                            <hr>
<span id="ispan-description-error"></span>
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
<span class="cspan-error-send"></span>
                            <input type='submit' value='Send' id='iinput-send-sos' class='btn btn-danger' style='width: 100%; margin-top: 1em;'>
                        </form>
                    </div>`;
    $('#ibtn-sos-pop').popover({
        html: true,
        container: 'body',
        content: popoverContent
    });
});

$("#iinput-send-sos").click(function () {
    var isMedicalPlace = $("#ichk-hospitals").is(':checked');
    var isFamily = $("#ichk-family").is(':checked');
    var isFriend = $("#ichk-friends").is(':checked');
    var description = $(".ctextarea-description").val();
    var latitude, longitude;
    // to get current location of user
    $.getJSON("http://freegeoip.net/json/", function (data) {
        latitude = data.latitude;
        longitude = data.longitude;
    });
    console.log(isMedicalPlace + "   " + isFamily + "   " + isFriend + "   " + description + "  " + latitude + "   " + longitude);
    if ((description != "") && (isMedicalPlace || isFriend || isFamily)) {
        var model = {
            isMedicalPlace: isMedicalPlace, isFamily: isFamily,
            isFriend: isFriend, description: description,
            latitude: latitude, longitude: longitude
        }
        $.ajax({
            type: 'POST',
            url: '/SOS/SendSos',
            data: { model },
            dataType: 'json',
            success: function (data) {
                console.log("success");
            },
            error: function (msg) {
                console.log(JSON.stringify(msg));
            }
        });
    }
    else {
        if (description == "") {
            $("#cspan-description-error").text("please fill What's Wrong field");
        }
        if (!(isMedicalPlace || isFriend || isFamily)) {
            $(".cspan-error-send").text("select at least one option").css("color:red");

        }

    }
});
$("#iinp-place-type").keydown(function () {
    $("#cspan-description-error").text("");
});
$('#ichk-hospitals').on('change', function () {
    $('.cspan-error-send').text("");
});
$('#ichk-family').on('change', function () {
    $('.cspan-error-send').text("");
});
$('#ichk-friends').on('change', function () {
    $('.cspan-error-send').text("");
});