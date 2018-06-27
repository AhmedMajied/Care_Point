/*Prognosis part*/
function showICUReminder(workplaceName, linkURL) {
    $.notify({
        // options
        icon: 'fa fa-bell-o fa-2x',
        title: 'Reminder',
        message: 'You need to updated number of care units available in <b>' + workplaceName + '</b>. <a href=' + linkURL +
            `>Update now</a><br /><br /><b>Remind me in (minutes): </b>
                 <div class="row">
                    <div class="col-xs-7"><input id='iinput-minuits-value' class="form-control" type="number" min="1" value="30" /></div>
                    <div class="col-xs-4"><input onclick="remindMe('`+ workplaceName + `','` + linkURL +`');" class= "btn btn-primary" type="button" value="Remind later" /></div>
                 </div>`,
        target: '_self'
    }, {
            // settings
            type: 'notif-primary',
            newest_on_top: true,
            placement: {
                from: "bottom",
                align: "left"
            },
            z_index: 1031,
            delay: 5000,
            url_target: '_self',
            mouse_over: 'pause',
            animate: {
                enter: 'animated fadeInDown',
                exit: 'animated fadeOutUp'
            },
            icon_type: 'class',
            template: `<div id="idiv-display-place-reminder" data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">
                        <button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>
                        <span data-notify="icon"></span>
                        <h4 data-notify="title">{1}</h4><hr/>
                        <span class="cspn-notif-summary" data-notify="message">{2}</span>
                        <a href="{3}" target="{4}" data-notify="url"></a>
                    </div>`
        });
}
function remindMe(workPlaceName, linkURL) {
    var minuits = $("#iinput-minuits-value").val();
    var placeId = linkURL.split("=")[1];
    $.ajax({
        type: 'POST',
        url: "/MedicalPlace/PlaceReminder",
        data: { minuits: minuits, placeId: placeId, placeName: workPlaceName },
        dataType: 'json',
        success: function (data)
        {
            $("#idiv-display-place-reminder").hide();
            CalculateReminderTime(data.placeName, data.placeId,data.year, data.month, data.day, data.hours, data.minuits, data.seconds);
        },
        error: function (msg) {
            alert("Sorry an Error Happened Please Try Again");
        }
    });
}
function CalculateReminderTime(placeName, placeId, year, month, day, hours, minuits, seconds) {
    var currentDate = new Date();
    var expirationDate = new Date(year, month - 1, day, hours, minuits, seconds);
    var interval = setInterval(function () {
        currentDate.setSeconds(currentDate.getSeconds() + 1);
        if (currentDate.toString() == expirationDate.toString()) {
            clearInterval(interval);
            $("#ibtn-click-reminder").click();
        }
    }, 1000);
}
$(document).ready(function () {
    $.ajax({
        type: 'POST',
        url: "/MedicalPlace/GetExpirationDate",
        dataType: 'json',
        success: function (data) {
            try {
                $("#idiv-display-place-reminder").hide();
            }
            catch{ console.log("Not Found"); }

            if (data.expired == false) {
                CalculateReminderTime(data.placeName, data.placeId, data.year, data.month, data.day, data.hours, data.minuits, data.seconds);
            }
            else {
                $.ajax({
                    type: 'POST',
                    url: '/MedicalPlace/GetCurrentWorkPlace',
                    dataType: 'json',
                    success: function (data) {
                        var url = (window.location.href);
                        var placeId = data.id;
                        if ((url.split("=")[1] == placeId || placeId == url.split("/")[5]) && url.includes('http://localhost:51902/MedicalPlace/ProfilePage'))
                        {
                            $("#ibtn-click-reminder").click();
                        }
                    },
                    error: function (msg) {
                        alert("Sorry An Error Happened Please Try Again");
                    }
                });
            }
        },
        error: function (msg) {
            console.log("Error Happened");
        }
    });
});