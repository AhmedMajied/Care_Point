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
        public static void NotifySOS(long sosId,List<long>citizens,int type,string description,float lat,float lng,string phoneNumber)
        {
            // type here to know if citizen or specialist
            foreach(long citizen in citizens)
            {
                string connectionId;
                Connections.TryGetValue(citizen, out connectionId);
                if (!String.IsNullOrWhiteSpace(connectionId))
                {
                    hubContext.Clients.Client(connectionId).notifySOS(sosId,type,description,lat,lng,phoneNumber);
                }
            }
        }
        public static void HideSOSNotification(List<long>contributers)
        {
            foreach(long contributer in contributers)
            {
                string connectionId;
                Connections.TryGetValue(contributer, out connectionId);
                if (!String.IsNullOrWhiteSpace(connectionId))
                {
                    hubContext.Clients.Client(connectionId).hideSOSNotification();
                }
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