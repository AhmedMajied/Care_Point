using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data.Entity;

namespace BLL
{
    public class CitizenBusinessLayer
    {
        private CarePointEntities DBEntities;

        public CitizenBusinessLayer()
        {
            DBEntities = new CarePointEntities();
        }

        public Citizen GetCitizen(long citizenID)
        {
            return DBEntities.Citizens.Single(citizen => citizen.Id == citizenID);
            // not tested
        }

        public Specialist GetSpecialist(long specialistID)
        {
            return DBEntities.Citizens.OfType<Specialist>().Single(specialist => specialist.Id == specialistID);
            // not tested and need to be checked 
        }
<<<<<<< HEAD

=======
        
>>>>>>> master
        public ICollection<Speciality> GetSpecialities()
        {
            return DBEntities.Specialities.ToList();
        }

        public ICollection<BloodType> GetBloodTypes()
        {
            return DBEntities.BloodTypes.ToList();
        }

        public bool IsEmailExists(string email)
        {
            return DBEntities.Citizens.Any(citizen => citizen.Email == email);
        }

        public bool IsPhoneNumberExists(string phone)
        {
            return DBEntities.Citizens.Any(citizen => citizen.PhoneNumber == phone);
        }

        public bool IsNationalIDExists(string id)
        {
            return DBEntities.Citizens.Any(citizen => citizen.NationalIDNumber == id);
        }

        public Citizen GetCitizenByPhone(string phone)
        {
            return DBEntities.Citizens.SingleOrDefault(citizen => citizen.PhoneNumber == phone);
        }
    }
}
