using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace CarePoint.Hubs
{
    public class NotificationsHub : Hub
    {
        public static ConcurrentDictionary<long, string> Connections = new ConcurrentDictionary<long, string>();
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
        
        public static void NotifyRelative(long userId, int increment, string relativeName = "", string relation = "")
        {
            string connectionId;
            Connections.TryGetValue(userId, out connectionId);
            if (!String.IsNullOrWhiteSpace(connectionId))
            {
                hubContext.Clients.Client(connectionId).notifyRelative(increment, relativeName, relation);
            }
        }
        public static void NotifyPrognosis(long userId, string diseaseName)
        {
            string connectionId;
            Connections.TryGetValue(userId, out connectionId);
            if (!String.IsNullOrWhiteSpace(connectionId))
            {
                hubContext.Clients.Client(connectionId).notifyPrognosis(diseaseName);
            }
        }

        public static void NotifyAttachment(long userId, string doctorName,string fileName)
        {
            string connectionId;
            Connections.TryGetValue(userId, out connectionId);
            if (!String.IsNullOrWhiteSpace(connectionId))
            {
                hubContext.Clients.Client(connectionId).notifyAttachment(doctorName,fileName);
            }
        }

        public override Task OnConnected()
        {
            if (!Connections.ContainsKey(Context.User.Identity.GetUserId<long>()))
            {
                Connections.TryAdd(Context.User.Identity.GetUserId<long>(), Context.ConnectionId);
            }
            return base.OnConnected();
        }
    }
}