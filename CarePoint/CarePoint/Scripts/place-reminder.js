/*Prognosis part*/
function showICUReminder(workplaceName, linkURL) {
    $.notify({
        // options
        icon: 'fa fa-bell-o fa-2x',
        title: 'Reminder',
        message: 'You need to updated number of care units available in <b>' + workplaceName + '</b>. <a href=' + linkURL +
            `>Update now</a><br /><br /><b>Remind me in (minutes): </b>
                 <div class="row">
                    <div class="col-xs-7"><input class="form-control" type="number" min="1" value="30" /></div>
                    <div class="col-xs-4"><input class="btn btn-primary" type="button" value="Remind later" /></div>
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
            template: `<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">
                        <button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>
                        <span data-notify="icon"></span>
                        <h4 data-notify="title">{1}</h4><hr/>
                        <span class="cspn-notif-summary" data-notify="message">{2}</span>
                        <a href="{3}" target="{4}" data-notify="url"></a>
                    </div>`
        });
}