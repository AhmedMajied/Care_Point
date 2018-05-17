using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace CarePoint.Hubs
{
    public class NotificationsHub : Hub
    {
        public void SendNotification(string message)
        {
            Clients.All.PushNotification(Context.User.Identity.Name + " says " + message);
        }
        public DateTime GetDateTime()
        {
            return DateTime.Now;
        }
    }
}