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
    public class RelativesHub : Hub
    {
        public static ConcurrentDictionary<long, string> Connections = new ConcurrentDictionary<long, string>();
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<RelativesHub>();
        public void Notify(long userId, int increment)
        {
            string connectionId;
            Connections.TryGetValue(userId, out connectionId);
            if (!String.IsNullOrWhiteSpace(connectionId))
            {
                Clients.Client(connectionId).notify(increment);
            }
        }

        public static void StaticNotify(long userId, int increment)
        {
            string connectionId;
            Connections.TryGetValue(userId, out connectionId);
            if (!String.IsNullOrWhiteSpace(connectionId))
            {
                hubContext.Clients.Client(connectionId).notify(increment);
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