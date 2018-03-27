using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class MedicalPlaceBusinessLayer
    {
        private CarePointEntities DBEntities { get; set; }
        public MedicalPlaceBusinessLayer()
        {
            DBEntities = new CarePointEntities();
        }
        public MedicalPlace GetMedicalPlace(long id)
        {
            return DBEntities.MedicalPlaces.SingleOrDefault(place => place.ID == id);
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
        public void addMedicalPlace(MedicalPlace medicalPlace)
        {
            DBEntities.MedicalPlaces.Add(medicalPlace);
            DBEntities.SaveChanges();
        }
        public MedicalPlace getMedicalPlace(long medicalPlaceId)
        {
            MedicalPlace medicalPlace = new MedicalPlace();
            DBEntities.MedicalPlaces.Any(medical => medical.ID == medicalPlaceId);
            return medicalPlace;
        }
        public ICollection<MedicalPlaceType> getAllTypes()
        {
            return DBEntities.MedicalPlaceTypes.ToList();
        }
        public ICollection<MedicalPlace>SearchPlace(double latitude,double longitude,string serviceType,string placeType,bool distance,bool cost,bool rate,bool popularity)
        {
            List<MedicalPlace> result = new List<MedicalPlace>();
            return result;
        }
    }
}
