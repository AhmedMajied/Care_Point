using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;

namespace BLL
{
    public class SOSBusinessLayer
    {

        private CarePointEntities DBEntities;
        public SOSBusinessLayer()
        {
            DBEntities = new CarePointEntities();
        }
        public void AddSOS(SOSs sos)
        {
            DBEntities.SOSses.Add(sos);
            DBEntities.SaveChanges();
        }
        public SOSs GetSOS(long id)
        {
            return DBEntities.SOSses.SingleOrDefault(sos => sos.ID == id);
        }
        public ICollection<SOSs>GetAllSosRequests(long hospitalID)
        {
            ICollection<SOSs> all = new List<SOSs>();
            all = DBEntities.SOSses.Where(sos => sos.MedicalPlaceID == hospitalID).ToList();
            return all;
        }
        public ICollection<SOSs> GetAll(long citizenID)
        {
            ICollection<SOSs> all = new List<SOSs>();
            all = DBEntities.SOSses.Where(sos => sos.SenderID == citizenID).ToList();
            return all;
        }
        public void ChangeStatus(long sosID,int status)
        {
            SOSs s=DBEntities.SOSses.Single(sos => sos.ID == sosID);
            s.StatusID = status;
            DBEntities.SOSses.Attach(s);
            DBEntities.Entry(s).State = System.Data.Entity.EntityState.Modified;
            DBEntities.SaveChanges();
        }
        public void AcceptSOS(long sosID,long hopsitalID)
        {
            SOSs s = DBEntities.SOSses.Single(sos => sos.ID == sosID);
            s.MedicalPlaceID = hopsitalID;
            DBEntities.SOSses.Attach(s);
            DBEntities.Entry(s).State = System.Data.Entity.EntityState.Modified;
            DBEntities.SaveChanges();
        }
        public void SaveNotifications(long citizenId ,DateTime time,string Text)
        {
            Notification notification = new Notification();
            notification.CitizenID = citizenId;
            notification.Text = Text;
            notification.Time = time;
            DBEntities.Notifications.Add(notification);
            DBEntities.SaveChanges();
        }
        public ICollection<Specialist> GetContributersOfSOSsServices(string location, int numberOfPlaces)
        {
            ICollection<MedicalPlace> medicalPlaces = new List<MedicalPlace>();
            medicalPlaces = medicalPlaces.OrderBy(medicalPlace => medicalPlace.Location.Distance(DbGeography.FromText(location, 4326))).ToList();
            ICollection<Service> services = new List<Service>();
            List<Specialist> providers = new List<Specialist>();
            services = medicalPlaces.Select(s => s.Services.SingleOrDefault(service => service.ServiceCategory.Name.Equals("Ambulance"))).ToList();
            int min = Math.Min(services.ToArray().Length, numberOfPlaces);
            services = services.Take(min).ToList();
            foreach (Service service in services)
            {
                providers.Union(service.ServiceMembershipRequests.
                        Where(request => request.IsConfirmed == true).Select(s => s.Specialist));
            }
            return providers;
        }
    }
}
