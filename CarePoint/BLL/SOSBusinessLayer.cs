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
        /// <summary>
        ///  Add new SOS to SOSses table in database
        /// </summary>
        /// <param name="sos">A SOSs object</param>
        public void AddSOS(SOSs sos)
        {
            DBEntities.SOSses.Add(sos);
            DBEntities.SaveChanges();
        }
        /// <summary>
        /// get SOS from database by id
        /// </summary>
        /// <param name="id">a long value</param>
        /// <returns>SOS object </returns>
        public SOSs GetSOS(long id)
        {
            return DBEntities.SOSses.SingleOrDefault(sos => sos.ID == id);
        }
        /// <summary>
        /// Get SOS requests for specific medicalPlace
        /// </summary>
        /// <param name="hospitalID">a long value</param>
        /// <returns>list Of SOSs </returns>
        public ICollection<SOSs>GetAllSosRequests(long hospitalID)
        {
            ICollection<SOSs> all = new List<SOSs>();
            all = DBEntities.SOSses.Where(sos => sos.MedicalPlaceID == hospitalID).ToList();
            return all;
        }
        /// <summary>
        /// get all sos for a citizen
        /// </summary>
        /// <param name="citizenID">a long value</param>
        /// <returns>List of SOS</returns>
        public ICollection<SOSs> GetAll(long citizenID)
        {
            ICollection<SOSs> all = new List<SOSs>();
            all = DBEntities.SOSses.Where(sos => sos.SenderID == citizenID).ToList();
            return all;
        }
        /// <summary>
        /// chanfe status of sos
        /// </summary>
        /// <param name="sosID">a long value</param>
        /// <param name="status">a long value</param>
        public void ChangeStatus(long sosID,int status)
        {
            SOSs s=DBEntities.SOSses.Single(sos => sos.ID == sosID);
            s.StatusID = status;
            DBEntities.SOSses.Attach(s);
            DBEntities.Entry(s).State = System.Data.Entity.EntityState.Modified;
            DBEntities.SaveChanges();
        }
        /// <summary>
        ///  medical place accept sos request
        /// </summary>
        /// <param name="sosID">a long value</param>
        /// <param name="hopsitalID">a long value</param>
        public void AcceptSOS(long sosID,long hopsitalID)
        {
            SOSs s = DBEntities.SOSses.Single(sos => sos.ID == sosID);
            s.MedicalPlaceID = hopsitalID;
            DBEntities.SOSses.Attach(s);
            DBEntities.Entry(s).State = System.Data.Entity.EntityState.Modified;
            DBEntities.SaveChanges();
        }
        /// <summary>
        /// get contributers of sos service based on nearest medicalPlaces
        /// </summary>
        /// <param name="location">a string value</param>
        /// <param name="numberOfPlaces"></param>
        /// <returns>List of Specialists joined these sos services</returns>
        public ICollection<Specialist> GetContributersOfSOSsServices(string location, int numberOfPlaces)
        {
            ICollection<MedicalPlace> medicalPlaces = new List<MedicalPlace>();
            medicalPlaces = medicalPlaces.OrderBy(medicalPlace => medicalPlace.Location.Distance(DbGeography.FromText(location, 4326))).ToList();
            ICollection<Service> services = new List<Service>();
            List<Specialist> providers = new List<Specialist>();
            services = medicalPlaces.Select(s => s.Services.SingleOrDefault(service => service.ServiceCategory.Name.Equals("Ambulance"))).ToList();
            int min = Math.Min(services.Count(), numberOfPlaces);
            services = services.Take(min).ToList();
            foreach (Service service in services)
            {
                providers=providers.Union(service.ServiceMembershipRequests.
                        Where(request => request.IsConfirmed == true).Select(s => s.Specialist)).ToList();
            }
            return providers;
        }
    }
}
