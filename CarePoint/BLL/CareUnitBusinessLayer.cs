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

<<<<<<< HEAD
        public ICollection<CareUnit> GetMedicalPlaceCareUnits(long medicalPlaceID)
=======
        public ICollection<CareUnit> getMedicalPlaceCareUnits(long medicalPlaceID)
>>>>>>> master
        {
            return DBEntities.CareUnits.Where(careUnit => careUnit.ProviderID == 
                                                medicalPlaceID).ToList();
        }

<<<<<<< HEAD
        public void UpdateAvailableRoomCount(List<CareUnit> careUnits)
=======
        public void updateAvailableRoomCount(List<CareUnit> careUnits)
>>>>>>> master
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
