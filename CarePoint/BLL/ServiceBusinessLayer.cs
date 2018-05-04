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
        public void AddWorkSlot(WorkSlot slot)
        {
            DBEntities.WorkSlots.Add(slot);
            DBEntities.SaveChanges();
        }

        public void RemoveWorkslot(long serviceID, TimeSpan startTime, TimeSpan endTime)
        {
            DBEntities.WorkSlots.RemoveRange(DBEntities.WorkSlots.Where(slot => slot.ServiceID == serviceID && slot.StartTime == startTime && slot.EndTime == endTime));
            DBEntities.SaveChanges();
        }

        public ICollection<ServiceCategory> GetServiceCategories()
        {
            return DBEntities.ServiceCategories.ToList();
        }

        public void UpdateService(Service service)
        {
            DBEntities.Services.Attach(service);
            DBEntities.Entry(service).State = System.Data.Entity.EntityState.Modified;
            DBEntities.SaveChanges();
        }

        public void AddService(Service service)
        {
            DBEntities.Services.Add(service);
            DBEntities.SaveChanges();
        }
    }
}
