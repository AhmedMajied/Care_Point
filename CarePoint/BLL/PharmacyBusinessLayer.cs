using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PharmacyBusinessLayer
    {
        private CarePointEntities DBEntities;

        public PharmacyBusinessLayer()
        {
            DBEntities = new CarePointEntities();
        }
        public void AddPharmacy(Pharmacy pharmacy)
        {
            DBEntities.Pharmacies.Add(pharmacy);
            DBEntities.SaveChanges();
        }
        public Pharmacy GetPharmacy(long id)
        {
            return DBEntities.Pharmacies.SingleOrDefault(pharmacy => pharmacy.ID == id);
        }
        public ICollection<Pharmacy> SearchMedicineInPharmacies(string drugName, string location)
        {
            List<Pharmacy> pharmacies = new List<Pharmacy>();
            List<Pharmacy> sortedPhrmacies = new List<Pharmacy>();
            pharmacies = DBEntities.PharmacyMedicines.Where(m => m.Medicine.Name.Contains(drugName) && m.Quantity>0).Select(p => p.Pharmacy).ToList();
            pharmacies = pharmacies.GroupBy(pharmacy => pharmacy.ID).Select(pharmacy=>pharmacy.First()).ToList();
            sortedPhrmacies = pharmacies.OrderBy(m => m.Location.Distance(DbGeography.FromText(location, 4326))).ToList();
            return sortedPhrmacies;
        }
    }
}
