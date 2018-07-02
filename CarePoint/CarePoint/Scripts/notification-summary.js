/* Author: Ahmed Hussein */

/*Prognosis part*/
function showPrognosisNotifSummary(diseaseName, clickURL){
    $.notify({
        // options
        icon: 'fa fa-heart-o fa-2x',
        title: 'Prognosis Tips',
        message: 'Watch out !<br />Someone in your family has <b>' + diseaseName + '</b>',
        url: clickURL,
        target: '_self'
    },{
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
        template: `<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">
                        <button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>
                        <span data-notify="icon"></span>
                        <h4 data-notify="title">{1}</h4><hr/>
                        <span class="cspn-notif-summary" data-notify="message">{2}</span>
                        <a href="{3}" target="{4}" data-notify="url"></a>
                    </div>`
    });
}

/*Relatives part*/
function showRelativeNotifSummary(relativeName, relation, clickURL){
    $.notify({
        // options
        icon: 'fa fa-users fa-2x',
        title: 'Family & Friends',
        message: 'You have a new relationship !<br /><b>' + relativeName+'</b> has marked you as '  + relation,
        url: clickURL,
        target: '_self'
    },{
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
        template: `<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">
                        <button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>
                        <span data-notify="icon"></span>
                        <h4 data-notify="title">{1}</h4><hr/>
                        <span class="cspn-notif-summary" data-notify="message">{2}</span>
                        <a href="{3}" target="{4}" data-notify="url"></a>
                    </div>`
    });
}

/*Attachments part */
function showAttachmentNotifSummary(doctorName, fileName, clickURL){
    $.notify({
        // options
        icon: 'fa fa-paperclip fa-2x',
        title: 'Attachments',
        message: 'You have a new attachment !<br /><b>' + doctorName+'</b> has attached the file '  + fileName,
        url: clickURL,
        target: '_self'
    },{
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
        template: `<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">
                        <button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>
                        <span data-notify="icon"></span>
                        <h4 data-notify="title">{1}</h4><hr/>
                        <span class="cspn-notif-summary" data-notify="message">{2}</span>
                        <a href="{3}" target="{4}" data-notify="url"></a>
                    </div>`
    });
}

/*SOS part*/
function showSOSNotifSummary(latitude,longitude, situationSummary, senderContact, type,sosId) {
    if (type == 1) {
        Template = `<div id="idiv-displays-notification" data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">
                        <button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>
                        <span data-notify="icon"></span>
                        <h4 data-notify="title">{1}</h4><hr/>
                        <span class="cspn-notif-summary" data-notify="message">{2}</span><hr />
                        <button onclic="getWorkPlace('`+ sosId +`');" class="btn btn-default cbtn-going"><span class="fa fa-check"></span>Ok, Going</button>
                    </div>`
    }
    else {
        Template =`<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">
                        <button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>
                        <span data-notify="icon"></span>
                        <h4 data-notify="title">{1}</h4><hr/>
                        <span class="cspn-notif-summary" data-notify="message">{2}</span><hr />
                    </div>`
    }
    $.notify({
        // options
        icon: 'fa fa-exclamation-triangle fa-2x',
        title: 'Emergency alert',
        message: '<b>Summary:</b><p>' + situationSummary + '</p>' +
                 '<span class="fa fa-map-marker fa-lg cspn-awsome"></span><div style="width:150px; height:150px;" id="imap-citizen-location"> <br />' +
                 '<span class="fa fa-phone fa-lg cspn-awsome"></span> ' + senderContact
    },{
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
        template: Template
            /*
             * `<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">
                        <button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>
                        <span data-notify="icon"></span>
                        <h4 data-notify="title">{1}</h4><hr/>
                        <span class="cspn-notif-summary" data-notify="message">{2}</span><hr />
                        <button class="btn btn-default cbtn-going"><span class="fa fa-check"></span>Ok, Going</button>
                    </div>`
             */
        });
    initMap(latitude, longitude);
}
function initMap(latitude, longitude) {
    var location = { lat: lat, lng: lng };
    var map = new google.maps.Map(document.getElementById('imap-citizen-location'), {
        zoom: 14,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        center: location
    });
    var marker = new google.maps.Marker({ position: location, map: map });
}

function getWorkPlace(sosId) {
    $.ajax({
        type: 'POST',
        url: '/MedicalPlace/GetCurrentWorkPlace',
        dataType: 'json',
        success: function (data) {
            acceptSOS(data, sosId);
        },
        error: function (msg) {
            alert("Sorry An Error Happened Please Try Again");
        }
    });
}
function acceptSOS(data,sosId)
{
    if (data.url != "#") {
        var placeId = data.id;
        $.ajax({
            type: 'POST',
            url: '/SOS/AcceptSOS',
            data: { sosId: sosId, hospitalId: placeId },
            dataType: 'json',
            success: function (data) {
                alert(data);

            }, error: function (msg) {
                alert("Sorry An Error Happened Please Try Again");
            }
        });
    }
    else
        {
            alert("Please Choose Your Current WorkPlace First");
        }
}