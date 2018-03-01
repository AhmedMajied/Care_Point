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

        public ICollection<CareUnit> getMedicalPlaceCareUnits(long medicalPlaceID)
        {
            return DBEntities.CareUnits.Where(careUnit => careUnit.ProviderID == 
                                                medicalPlaceID).ToList();
        }

        public void updateAvailableRoomCount(List<CareUnit> careUnits)
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
    }
}
