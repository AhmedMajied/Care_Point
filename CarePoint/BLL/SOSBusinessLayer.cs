using DAL;
using System;
using System.Collections.Generic;
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
        public ICollection<SOSs>GetAllHospitals(long hospitalID)
        {
            ICollection<SOSs> all = new List<SOSs>();
            all = DBEntities.SOSses.Where(sos => sos.MedicalPlaceID == hospitalID).ToList();
            return all;
        }
        public ICollection<SOSs> GetAllCitizens(long citizenID)
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
        public ICollection<RelationType> GetRelationTypes()
        {
            return DBEntities.RelationTypes.ToList();
        }
        public void SaveNotifications(List<Citizen> citizens ,DateTime time,string Text)
        {
            foreach(Citizen citizen in citizens)
            {
                Notification notification = new Notification();
                notification.CitizenID = citizen.Id;
                notification.Text = Text;
                notification.Time = time;
                DBEntities.Notifications.Add(notification);
                DBEntities.SaveChanges();
            }
        }
    }
}
