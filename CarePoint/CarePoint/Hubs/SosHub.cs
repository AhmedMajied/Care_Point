using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarePoint.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace CarePoint.Hubs
{
    public class SosHub : Hub
    {
        public static ConcurrentDictionary<long, string> connections = new ConcurrentDictionary<long, string>();
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<SosHub>();
        public override Task OnConnected()
        {
            long userID= Context.User.Identity.GetUserId<long>();
            if (!connections.ContainsKey(userID))
            {
                connections.TryAdd(userID, Context.ConnectionId);
            }
            return base.OnConnected();
        }
        public static void StaticNotify(List<long> citizens,SOSNotificationViewModel s)
        {
            foreach (long citizenID in citizens)
            {
                string connectionID;
                connections.TryGetValue(citizenID, out connectionID);
                if(!string.IsNullOrWhiteSpace(connectionID))
                {
                    hubContext.Clients.Client(connectionID).sendSOSNotification(s);
                }

            }

        }
        public void SendNotification(long citizenID,SOSNotificationViewModel s)
        {
            string connectionID;
            connections.TryGetValue(citizenID, out connectionID);
            if (!string.IsNullOrWhiteSpace(connectionID))
            {
                Clients.Client(connectionID).sendSOSNotification(s);
            }
        }
    }
}