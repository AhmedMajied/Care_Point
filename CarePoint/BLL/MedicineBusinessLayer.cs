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

        /// <summary>
        /// get all medicines stored in database
        /// </summary>
        public ICollection<Medicine> GetAllMedicines()
        {
            return DBEntities.Medicines.ToList();
        }

        /// <summary>
        /// get all alternatives to specific medicine 
        /// </summary>
        /// <remarks>
        /// two medicines are alternatives to each others if and only if 
        /// their active incredients are exactly the same
        /// </remarks>
        public ICollection<Medicine> getMedicineAlternatives(string medicineName)
        {
            List<Medicine> medicineAlternatives = null;
            Medicine medicine = DBEntities.Medicines.FirstOrDefault(m => m.Name == medicineName);
            List<MedicineActiveIngredient> medicineActiveIngredients = 
                                                medicine.MedicineActiveIngredients.ToList();
            long activeIngredientID = medicineActiveIngredients[0].ActiveIngredientID;

            // get all medicines that have the first active ingredient
            medicineAlternatives = DBEntities.Medicines.Where
                 (
                    m => m.MedicineActiveIngredients.Any
                    (e => e.ActiveIngredientID == activeIngredientID)
                     && m.Name != medicineName
                 ).ToList();

            // intersect with medicices that have the second active ingredient
            if(medicineActiveIngredients.Count == 2)
            {
                activeIngredientID = medicineActiveIngredients[1].ActiveIngredientID;
                medicineAlternatives.Intersect(
                    DBEntities.Medicines.Where(m => m.MedicineActiveIngredients.Any
                      (e => e.ActiveIngredientID == activeIngredientID)
                       && m.Name != medicineName).ToList()
                    );
            }
            
            return medicineAlternatives;
        }
    }
}
