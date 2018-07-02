/*Prognosis part*/
function showPrognosisNotifSummary(diseaseName, clickURL){
    $.notify({
        // options
        icon: 'fa fa-heart-o fa-2x',
        title: 'Prognosis tips',
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
        title: 'Family and friends',
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
function showSOSNotifSummary(senderLocation, situationSummary, senderContact){
    $.notify({
        // options
        icon: 'fa fa-exclamation-triangle fa-2x',
        title: 'Emergency alert',
        message: '<b>Summary:</b><p>' + situationSummary + '</p>' +
                 '<span class="fa fa-map-marker fa-lg cspn-awsome"></span> ' + senderLocation + '<br />' +
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
        template: `<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">
                        <button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>
                        <span data-notify="icon"></span>
                        <h4 data-notify="title">{1}</h4><hr/>
                        <span class="cspn-notif-summary" data-notify="message">{2}</span><hr />
                        <button class="btn btn-default cbtn-going"><span class="fa fa-check"></span>Ok, Going</button>
                    </div>`
    });
}