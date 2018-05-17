(function () {
    var soSNotificationsHub = $.connection.soSNotificationsHub;
    $.connection.hub.logging = true;
    $.connection.hub.start();

    soSNotificationsHub.client.notifications = function (sosNotification) {
        model.pushSosNotification(sosNotification);
    };

    var Model = function () {
        var self = this;
        self.message = ko.observable(""),
        self.messages = ko.observableArray()
    };

    Model.prototype = {
        sendSos: function () {
            var self = this;
            soSNotificationsHub.server.sendSosNotifications(self.message());
            self.message("");
        },
        pushSosNotification: function (sosNotification) {
            var self = this;
            alert(sosNotification);
            self.messages.push(sosNotification);
        }
    };


    var model = new Model();


    $(function () {
        ko.applyBindings(model);
    });
}());