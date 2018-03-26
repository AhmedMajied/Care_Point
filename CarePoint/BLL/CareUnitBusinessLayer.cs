using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CareUnitBusinessLayer
    {
        private CarePointEntities DBEntities;

        public CareUnitBusinessLayer()
        {
            DBEntities = new CarePointEntities();
        }

        public ICollection<CareUnit> GetMedicalPlaceCareUnits(long medicalPlaceID)
        {
            return DBEntities.CareUnits.Where(careUnit => careUnit.ProviderID == 
                                                medicalPlaceID).ToList();
        }

        public void UpdateAvailableRoomCount(List<CareUnit> careUnits)
        {
            DateTime time = DateTime.Now;
            
            foreach (CareUnit careUnit in careUnits)
            {
                careUnit.LastUpdate = time;
                
                DBEntities.CareUnits.Attach(careUnit);

                DBEntities.Entry(careUnit).Property(unit => unit.LastUpdate).IsModified = true;
                DBEntities.Entry(careUnit).Property(unit => unit.AvailableRoomCount).IsModified = true;
                
                
            }
            DBEntities.SaveChanges();
        }

        public ICollection<CareUnitType> GetCareUnitTypes()
        {
            return DBEntities.CareUnitTypes.ToList();
        }

        public void UpdateCareUnit(CareUnit careunit)
        {
            DBEntities.CareUnits.Attach(careunit);
            DBEntities.Entry(careunit).State = EntityState.Modified;
            DBEntities.SaveChanges();
        }

        public void AddCareUnit(CareUnit careunit)
        {
            DBEntities.CareUnits.Add(careunit);
            DBEntities.SaveChanges();
        }
    }
}
