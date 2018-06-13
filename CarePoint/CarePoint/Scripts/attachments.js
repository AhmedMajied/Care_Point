function readAttachmentsOfType(typeId) {
    var self = this;
    $.post("/Citizen/ReadAttachmentsOfType", { typeId: typeId }).done(function () {
        var tabId = $(self).attr("href");
        var notif = $(tabId).find(".cli-notif");
        notif.removeClass("cli-notif");
        var notificationMark = $("#ilink-attachments .cmark-notif");
        var notificationCount = parseInt(notificationMark.text());
        notificationCount -= notif.length;
        notificationMark.text(notificationCount);
        if (notificationCount == 0) {
            notificationMark.css("display", "none");
        }
    });
}