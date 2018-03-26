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
            if (distance)
            {
                string point = string.Format("POINT({0} {1})", longitude, latitude);
                result= (DBEntities.MedicalPlaces.Where(medicalPlace => medicalPlace.IsConfirmed && medicalPlace.Services.Any(x => x.Name.Contains(serviceType)) && medicalPlace.MedicalPlaceType.Name.Contains(placeType)).ToList())
                        .OrderBy(medicalPlace => medicalPlace.Location.Distance(DbGeography.FromText(point, 4326))).ToList();
            }
            if(cost)// sort ascending
            {
                result = result.Union(DBEntities.Services.Where(service => service.Name.Contains(serviceType) && service.MedicalPlace.Name.Contains(placeType)).OrderBy(service => service.Cost).Select(medicalPlace=>medicalPlace.MedicalPlace)).ToList();
            }
            if(rate)
            {
                result=result.Union(DBEntities.ServiceRatings.Where(ser=>ser.Service.Name.Contains(serviceType)&&ser.Service.MedicalPlace.Name.Contains(placeType)).OrderByDescending(ser=>ser.Rating).Select(medicalPlace=>medicalPlace.Service.MedicalPlace)).ToList();
                // not tested
            }
            if(popularity)
            {

            }
            return result;
        }
    }
}
