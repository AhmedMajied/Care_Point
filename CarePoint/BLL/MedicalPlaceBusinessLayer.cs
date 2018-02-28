using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
namespace BLL
{
    public class MedicalPlaceBusinessLayer
    {
        public CarePointEntities dbEntities;
        public MedicalPlaceBusinessLayer()
        {
            dbEntities = new CarePointEntities();
        }
        public void addMedicalPlace(MedicalPlace medicalPlace)
        {
            dbEntities.MedicalPlaces.Add(medicalPlace);
            dbEntities.SaveChanges();
        }
        public MedicalPlace getMedicalPlace(long medicalPlaceId)
        {
            MedicalPlace medicalPlace = new MedicalPlace();
            dbEntities.MedicalPlaces.Any(medical => medical.ID == medicalPlaceId);
            return medicalPlace;
        }
        public ICollection<MedicalPlaceType> getAllTypes()
        {
            return dbEntities.MedicalPlaceTypes.ToList();
        }
    }
}
