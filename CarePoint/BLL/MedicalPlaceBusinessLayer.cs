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
        public ICollection<MedicalPlace> searchCareUnitsPlace(double latitude, double longitude, string serviceType, string placeType, bool distance, bool cost, bool rate, bool popularity)
        {
            List<CareUnit> careUnits = new List<CareUnit>();
            List<CareUnit> sortedDistance = new List<CareUnit>();// ascending
            List<CareUnit> sortedPopularity = new List<CareUnit>();// descending
            List<CareUnit> sortedCost = new List<CareUnit>();// ascending
            List<CareUnit> sortedRate = new List<CareUnit>();//descending
            Dictionary<MedicalPlace, int> result = new Dictionary<MedicalPlace, int>();
            careUnits = DBEntities.CareUnits.Where(careUnit => careUnit.Name == serviceType || careUnit.ProviderID == (DBEntities.MedicalPlaces.SingleOrDefault(medicalPlace => medicalPlace.Name == placeType)).ID).ToList();
            if (distance)
            {
                string point = string.Format("POINT({0} {1})", longitude, latitude);
                sortedDistance = careUnits.OrderBy(careUnit => (DBEntities.MedicalPlaces.SingleOrDefault(place => place.ID == careUnit.ProviderID)).Location.Distance(DbGeography.FromText(point, 4326))).ToList();
            }
            if (rate)
            {
                sortedRate = careUnits.OrderByDescending(careUnit => careUnit.CareUnitRatings.Average(care => care.Rating)).ToList();
            }
            if (cost)
            {
                sortedCost = careUnits.OrderBy(careUnit => careUnit.Cost).ToList();
            }
            int Rate = 0, Cost = 0, Distance = 0, Popularity = 0;
            foreach (CareUnit careUnit in careUnits)
            {
                Distance = sortedDistance.IndexOf(careUnit) + 1;
                Rate = sortedRate.IndexOf(careUnit) + 1;
                Popularity = sortedPopularity.IndexOf(careUnit) + 1;
                Cost = sortedCost.IndexOf(careUnit) + 1;
                result.Add((DBEntities.MedicalPlaces.SingleOrDefault(medical => medical.ID == careUnit.ID)), (int)((1 / Distance * 1.0) + 2 * Cost + 2 * Rate + Popularity));
            }
            return result.OrderByDescending(res => res.Value).Select(res => res.Key).ToList();
        }

        public ICollection<MedicalPlace> SearchMedicalPlace(double latitude, double longitude, string serviceType, string placeType, bool distance, bool cost, bool rate, bool popularity)
        {
            List<Service> medicalPlaces = new List<Service>();
            List<Service> sortedDistance = new List<Service>();// ascending
            List<Service> sortedPopularity = new List<Service>();// descending
            List<Service> sortedCost = new List<Service>();// ascending
            List<Service> sortedRate = new List<Service>();//descending
            Dictionary<MedicalPlace, int> result = new Dictionary<MedicalPlace, int>();
            medicalPlaces = DBEntities.Services.Where(service => service.Name.Contains(serviceType) || service.MedicalPlace.Name.Contains(placeType)).OrderBy(service => service.Cost).ToList();
            if (distance)
            {
                string point = string.Format("POINT({0} {1})", longitude, latitude);
                sortedDistance = medicalPlaces.OrderBy(medicalPlace => medicalPlace.MedicalPlace.Location.Distance(DbGeography.FromText(point, 4326))).ToList();
            }
            if (cost)
            {
                sortedCost = medicalPlaces.OrderBy(service => service.Cost).ToList();
            }
            if (rate)
            {
                sortedRate = medicalPlaces.OrderByDescending(service => service.ServiceRatings.Average(r => r.Rating)).ToList();
            }
            int Rate = 0, Cost = 0, Distance = 0, Popularity = 0;
            foreach (Service service in medicalPlaces)
            {
                Distance = sortedDistance.IndexOf(service) + 1;
                Rate = sortedRate.IndexOf(service) + 1;
                Popularity = sortedPopularity.IndexOf(service) + 1;
                Cost = sortedCost.IndexOf(service) + 1;
                result.Add(service.MedicalPlace, (int)((1 / Distance * 1.0) + 2 * Cost + 2 * Rate + Popularity));
            }
            return result.OrderByDescending(res => res.Value).Select(res => res.Key).ToList();
        }

    }
}
