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

        /// <summary>
        /// Add new Pharmacy to Pharmacies in database
        /// </summary>
        /// <param name="pharmacy">A Pharmacy object</param>
        public void AddPharmacy(Pharmacy pharmacy)
        {
            DBEntities.Pharmacies.Add(pharmacy);
            DBEntities.SaveChanges();
        }

        /// <summary>
        /// Get Pharmacy from database by id
        /// </summary>
        /// <param name="id">a long value</param>
        /// <returns>Pharmacy</returns>
        public Pharmacy GetPharmacy(long id)
        {
            return DBEntities.Pharmacies.SingleOrDefault(pharmacy => pharmacy.ID == id);
        }

        /// <summary>
        /// search Medicine in Pharmacies that have drugName and sort Pharmacies
        /// by distance from nearest to the furthest
        /// </summary>
        /// <param name="drugName">a string value</param>
        /// <param name="location">a string location</param>
        /// <returns>list of Pharmacies</returns>
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
