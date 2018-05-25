/*SOS part*/
function showSOSNotifSummary(senderLocation, situationSummary, senderContact) {
    $.notify({
        // options
        icon: 'fa fa-exclamation-triangle fa-2x',
        title: 'Emergency alert',
        message: '<b>Summary:</b><p>' + situationSummary + '</p>' +
            '<span class="fa fa-map-marker fa-lg cspn-awsome"></span> ' + senderLocation + '<br />' +
            '<span class="fa fa-phone fa-lg cspn-awsome"></span> ' + senderContact
    }, {
            // settings
            type: 'notif-danger',
            newest_on_top: true,
            placement: {
                from: "bottom",
                align: "left"
            },
            z_index: 1031,
            delay: 0, /* Never disappear */
            animate: {
                enter: 'animated fadeInDown',
                exit: 'animated fadeOutUp'
            },
            icon_type: 'class',
            template: `<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">
                        <button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>
                        <span data-notify="icon"></span>
                        <h4 data-notify="title">{1}</h4><hr/>
                        <span class="cspn-notif-summary" data-notify="message">{2}</span><hr />
                        <button class="btn btn-default cbtn-going"><span class="fa fa-check"></span>Ok, Going</button>
                    </div>`
        });
}
(function () {
    var sosHub = $.connection.sosHub;
    $.connection.hub.logging = true; // for testing only to see what happened
    $.connection.hub.start().done(function () {
        console.log("Hello connection with SOSHUB was established \n");
    });

    sosHub.client.sendSOSNotification = function (sos) {
        console.log("Phone Number : " + sos.userPhoneNumber + "\nDescription : " +
               sos.description + "\n Latitude : "
            + sos.latitude + "\nLongitude : " + sos.longitude);
        var location = "lat & lon " + sos.latitude + "  :  " + sos.longitude;
        showSOSNotifSummary(location, sos.description, sos.userPhoneNumber);
    }

}());