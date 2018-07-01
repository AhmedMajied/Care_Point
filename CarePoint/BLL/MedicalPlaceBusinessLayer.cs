using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Diagnostics;
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

        /// <summary>
        /// Gets medical place given its Id
        /// </summary>
        /// <param name="id">Medical Place Id</param>
        /// <returns></returns>
        public MedicalPlace GetMedicalPlace(long id)
        {
            return DBEntities.MedicalPlaces.SingleOrDefault(place => place.ID == id);
        }

        public void AddMedicalPlace(MedicalPlace medicalPlace)
        {
            DBEntities.MedicalPlaces.Add(medicalPlace);
            DBEntities.SaveChanges();
        }
        public ICollection<MedicalPlaceType> GetAllTypes()
        {
            return DBEntities.MedicalPlaceTypes.ToList();
        }
        public ICollection<MedicalPlace> SearchCareUnitsPlace(double latitude, double longitude, string serviceType, string placeType, bool distance, bool cost, bool rate, bool popularity)
        {
            List<CareUnit> careUnits = new List<CareUnit>();
            List<CareUnit> sortedDistance = new List<CareUnit>();
            List<CareUnit> sortedPopularity = new List<CareUnit>();
            List<CareUnit> sortedCost = new List<CareUnit>();
            List<CareUnit> sortedRate = new List<CareUnit>();
            Dictionary<MedicalPlace, int> result = new Dictionary<MedicalPlace, int>();
            careUnits = DBEntities.CareUnits.Where(careUnit => careUnit.Name == serviceType || careUnit.ProviderID == (DBEntities.MedicalPlaces.SingleOrDefault(medicalPlace => medicalPlace.Name == placeType)).ID).ToList();
            if (distance)
            {
                string point = string.Format("POINT({0} {1})", longitude, latitude);
                sortedDistance = careUnits.OrderBy(careUnit => (DBEntities.MedicalPlaces.SingleOrDefault(place => place.ID == careUnit.ProviderID)).Location.Distance(DbGeography.FromText(point, 4326))).ToList();
            }
            if (rate)
            {
                sortedRate = careUnits.OrderBy(careUnit => careUnit.CareUnitRatings.Average(care => care.Rating)).ToList();
            }
            if (cost)
            {
                sortedCost = careUnits.OrderByDescending(careUnit => careUnit.Cost).ToList();
            }
            int Rate = 0, Cost = 0, Distance = 0, Popularity = 0;
            foreach (CareUnit careUnit in careUnits)
            {
                Distance = sortedDistance.IndexOf(careUnit) == -1 ? 1 : sortedDistance.IndexOf(careUnit) + 1;
                Rate = sortedRate.IndexOf(careUnit) + 1;
                //Popularity = sortedPopularity.IndexOf(careUnit) + 1;
                Cost = sortedCost.IndexOf(careUnit) + 1;
                MedicalPlace m = (DBEntities.MedicalPlaces.SingleOrDefault(medical => medical.ID == careUnit.ID));
                if (!result.ContainsKey(m))
                    result.Add(m, (int)((1 / Distance * 1.0) + 2 * Cost + 2 * Rate + Popularity));
            }
            return result.OrderByDescending(res => res.Value).Select(res => res.Key).ToList();
        }
        public ICollection<MedicalPlace> SearchMedicalPlace(double latitude, double longitude, string serviceName, string placeName, bool isDistance, bool isCost, bool isRate, bool isPopularity)
        {
            List<MedicalPlace> medicalPlaces = new List<MedicalPlace>();
            List<MedicalPlace> sortedDistance = new List<MedicalPlace>();
            List<MedicalPlace> sortedPopularity = new List<MedicalPlace>();
            List<MedicalPlace> sortedCost = new List<MedicalPlace>();
            List<MedicalPlace> sortedRate = new List<MedicalPlace>();
            Dictionary<MedicalPlace, int> result = new Dictionary<MedicalPlace, int>();
            if (placeName != "" && serviceName == "")
            {
                medicalPlaces = DBEntities.MedicalPlaces.Where(m => m.Name.Contains(placeName) /*&& m.IsConfirmed == true*/).ToList();
            }
            else if (placeName == "" && serviceName != "")
            {
                medicalPlaces = DBEntities.Services.Where(s => s.Name.Contains(serviceName)).Select(s => s.MedicalPlace).ToList();
            }
            else if (placeName != "" && serviceName != "")
            {
                medicalPlaces = DBEntities.Services.Where(s => s.Name.Contains(serviceName) || s.MedicalPlace.Name.Contains(placeName)).Select(m => m.MedicalPlace).ToList();
            }
            if (isDistance)
            {
                string point = string.Format("POINT({0} {1})", longitude, latitude);
                sortedDistance = medicalPlaces.OrderBy(m => m.Location.Distance(DbGeography.FromText(point, 4326))).ToList();
            }
            if (isRate)
            {
                sortedRate = medicalPlaces.OrderBy(m => m.Services.Average(s => s.ServiceRatings.Sum(x => x.Rating))).ToList();
            }
            if (isCost)
            {
                if (serviceName != "")
                {
                    sortedCost = DBEntities.Services.Where(s => s.Name.Contains(serviceName)).OrderByDescending(s => s.Cost).Select(m => m.MedicalPlace).ToList();
                }
                else
                {
                    sortedCost = medicalPlaces.OrderByDescending(m => m.Services.Average(c => c.Cost)).ToList();
                }
            }
            if (isPopularity)
            {
                sortedPopularity = SortMedicalPlacesOnPopularity(medicalPlaces);
            }
            int Rate = 0, Cost = 0, Distance = 0, Popularity = 0;
            foreach (MedicalPlace medicalPlace in medicalPlaces)
            {
                Distance = sortedDistance.IndexOf(medicalPlace) == -1 ? 1 : sortedDistance.IndexOf(medicalPlace) + 1;
                Rate = sortedRate.IndexOf(medicalPlace) + 1;
                Popularity = sortedPopularity.IndexOf(medicalPlace) + 1;
                Cost = sortedCost.IndexOf(medicalPlace) + 1;
                if(!result.ContainsKey(medicalPlace))
                     result.Add(medicalPlace, (int)((1 / Distance * 1.0) + 2 * Cost + 2 * Rate + Popularity));
            }
            return result.OrderByDescending(res => res.Value).Select(res => res.Key).ToList();
        }
        private List<MedicalPlace> SortMedicalPlacesOnPopularity(List<MedicalPlace> medicalPlaces)
        {
            Dictionary<MedicalPlace, long> result = new Dictionary<MedicalPlace, long>();
            foreach (MedicalPlace m in medicalPlaces)
            {
                long count=DBEntities.HistoryRecords.Where(h => h.MedicalPlace.ID == m.ID).Count();
                result.Add(m, count);
            }
            return result.OrderBy(res => res.Value).Select(res => res.Key).ToList();
        }

        public ICollection<MedicalPlace> SortMedicalPlacesByDistance(string location)
        {
            ICollection<MedicalPlace> medicalPlaces = new List<MedicalPlace>();
            medicalPlaces = medicalPlaces.OrderBy(medicalPlace => medicalPlace.Location.Distance(DbGeography.FromText(location, 4326))).ToList();
            return medicalPlaces;
        }
    }
}
