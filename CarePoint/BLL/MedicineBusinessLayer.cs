using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class MedicineBusinessLayer
    {
        private CarePointEntities DBEntities;

        public MedicineBusinessLayer()
        {
            DBEntities = new CarePointEntities();
        }

        public ICollection<Medicine> GetAllMedicines()
        {
            return DBEntities.Medicines.ToList();
        }

        public ICollection<Medicine> getMedicineAlternatives(string medicineName)
        {
            Medicine medicine = DBEntities.Medicines.Single(m => m.Name == medicineName);

            long activeIngredientID = (medicine.MedicineActiveIngredients.ToList())[0].ActiveIngredientID;

            List<Medicine> medicineAlternatives = DBEntities.Medicines.Where
                (m => m.MedicineActiveIngredients.Any(e => e.ActiveIngredientID == activeIngredientID)
                    && m.Name != medicineName).ToList();

            return medicineAlternatives;
        }
    }
}
