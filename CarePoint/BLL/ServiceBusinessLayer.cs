using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ServiceBusinessLayer
    {
        private CarePointEntities DBEntities;

        public ServiceBusinessLayer()
        {
            DBEntities = new CarePointEntities();
        }

        /// <summary>
        /// Adds workslot to the database
        /// </summary>
        /// <param name="slot">Workslot</param>
        public void AddWorkSlot(WorkSlot slot)
        {
            DBEntities.WorkSlots.Add(slot);
            DBEntities.SaveChanges();
        }

        /// <summary>
        /// Removes workslot from the database
        /// </summary>
        /// <param name="serviceID">Service Id</param>
        /// <param name="startTime">Start Time</param>
        /// <param name="endTime">End Time</param>
        public void RemoveWorkslot(long serviceID, TimeSpan startTime, TimeSpan endTime)
        {
            DBEntities.WorkSlots.RemoveRange(DBEntities.WorkSlots.Where(slot => slot.ServiceID == serviceID && slot.StartTime == startTime && slot.EndTime == endTime));
            DBEntities.SaveChanges();
        }

        /// <summary>
        /// Get all Service Categories from the database
        /// </summary>
        /// <returns>ICollection</returns>
        public ICollection<ServiceCategory> GetServiceCategories()
        {
            return DBEntities.ServiceCategories.ToList();
        }

        /// <summary>
        /// Updates a specific Service
        /// </summary>
        /// <param name="service">Service</param>
        public void UpdateService(Service service)
        {
            DBEntities.Services.Attach(service);
            DBEntities.Entry(service).State = System.Data.Entity.EntityState.Modified;
            DBEntities.SaveChanges();
        }

        /// <summary>
        /// Adds a Service to the database
        /// </summary>
        /// <param name="service">Service</param>
        public void AddService(Service service)
        {
            DBEntities.Services.Add(service);
            DBEntities.SaveChanges();
        }

        /// <summary>
        ///  get WorkSlots for specific service on specific Day
        /// </summary>
        /// <param name="serviceId">a long value</param>
        /// <param name="dayName">a string value</param>
        /// <returns>Workslots of Services in Specific Day</returns>
        public List<WorkSlot> GetWorkSlots(long serviceId,string dayName)
        {
            List<WorkSlot> workSlots = new List<WorkSlot>();
            workSlots = DBEntities.WorkSlots.Where(slot => slot.ServiceID == serviceId && slot.DayName.ToLower()==(dayName.ToLower())).OrderBy(s=>s.StartTime).ToList();
            return workSlots;
        }
    }
}
