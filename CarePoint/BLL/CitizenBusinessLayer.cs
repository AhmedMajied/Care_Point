using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data.Entity;
using System.Collections;

namespace BLL
{
    /// <summary>
    /// Responsible for all business logic associated with Citizen
    /// </summary>
    public class CitizenBusinessLayer
    {
        private CarePointEntities DBEntities;

        public CitizenBusinessLayer()
        {
            DBEntities = new CarePointEntities();
        }

        /// <summary>
        /// get citizen by converting QR code to its original national ID
        /// then search for it in database
        /// </summary>  
        /// <exception>thrown when citizenQRCode is not base-64 character</exception> 
        public Citizen GetCitizenByQR(string citizenQRCode)
        {
            string nationalID;
            try
            {
                // decode QR to original national ID
                var base64EncodedBytes = Convert.FromBase64String(citizenQRCode);
                nationalID = Encoding.UTF8.GetString(base64EncodedBytes);
            }catch(Exception)
            {
                return null;
            }
            return DBEntities.Citizens.SingleOrDefault(citizen => citizen.NationalIDNumber == nationalID);
        }

        /// <summary>
        /// Gets a citizen from database by Id
        /// </summary>
        /// <param name="citizenID">Citizen Id</param>
        /// <returns>Citizen</returns>
        public Citizen GetCitizen(long citizenID)
        {
            return DBEntities.Citizens.SingleOrDefault(citizen => citizen.Id == citizenID);
        }

        /// <summary>
        /// Gets all specialities from database
        /// </summary>
        /// <returns>ICollection</returns>
        public ICollection<Speciality> GetSpecialities()
        {
            return DBEntities.Specialities.ToList();
        }
        
        /// <summary>
        /// Gets all Blood Types from database
        /// </summary>
        /// <returns>ICollection</returns>
        public ICollection<BloodType> GetBloodTypes()
        {
            return DBEntities.BloodTypes.ToList();
        }

        /// <summary>
        /// Checks if the email exists in the database
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>bool</returns>
        public bool IsEmailExists(string email)
        {
            return DBEntities.Citizens.Any(citizen => citizen.Email == email);
        }

        /// <summary>
        /// Checks if the phone number exists in the database
        /// </summary>
        /// <param name="phone">Phone Numer</param>
        /// <returns>bool</returns>
        public bool IsPhoneNumberExists(string phone)
        {
            return DBEntities.Citizens.Any(citizen => citizen.PhoneNumber == phone);
        }

        /// <summary>
        /// Checks if the national Id exists in the database
        /// </summary>
        /// <param name="id">National Id Number</param>
        /// <returns>bool</returns>
        public bool IsNationalIDExists(string id)
        {
            return DBEntities.Citizens.Any(citizen => citizen.NationalIDNumber == id);
        }

        /// <summary>
        /// Gets citizen from database by phone number
        /// </summary>
        /// <param name="phone">Phone Number</param>
        /// <returns>Citizen</returns>
        public Citizen GetCitizenByPhone(string phone)
        {
            return DBEntities.Citizens.SingleOrDefault(citizen => citizen.PhoneNumber == phone);
        }


        public List<List<Citizen>> SearchAccounts(long citizenId,string searchBy, string searchValue)
        {
            List<Citizen> result = new List<Citizen>();
            List<Citizen> doctors = new List<Citizen>();
            List<Citizen> pharmacists = new List<Citizen>();
            List<Citizen> non_specialists = new List<Citizen>();
            String[] split = searchValue.Split(' ');
            if (searchBy.Equals("Name"))
            {
                foreach (string val in split)
                {
                    if (DBEntities.Citizens.Any(citizen => citizen.Name.Contains(val)))
                    {
                        result = result.Union(DBEntities.Citizens.Where(citizen => citizen.Name.Contains(val)).ToList()).ToList();
                    }
                }
                if (DBEntities.Citizens.Any(citizen => citizen.Name.Contains(searchValue)))
                {
                    result = result.Union(DBEntities.Citizens.Where(citizen => citizen.Name.Contains(searchValue)).ToList()).ToList();
                }
            }
            else if (searchBy.Equals("E-mail") && (DBEntities.Citizens.Any(citizen => citizen.Email.Contains(searchValue))))
            {
                result = DBEntities.Citizens.Where(citizen => citizen.Email.Contains(searchValue)).ToList();
            }
            else if (searchBy.Equals("Phone") && (DBEntities.Citizens.Any(citizen => citizen.PhoneNumber == searchValue)))
            {
                result = DBEntities.Citizens.Where(citizen => citizen.PhoneNumber == searchValue).ToList();
            }
            foreach (Citizen specialist in result)
            {
                if (specialist is Specialist && citizenId!=specialist.Id)
                {
                    if (((Specialist)specialist).SpecialityID == 1)
                    {
                        doctors.Add(specialist);
                    }
                    else if (((Specialist)specialist).SpecialityID == 2)
                    {
                        pharmacists.Add(specialist);
                    }
                }
                else if(!(specialist is Specialist) &&citizenId != specialist.Id)
                {
                    non_specialists.Add(specialist);

                }
            }
            List<List<Citizen>> allCitizens = new List<List<Citizen>>();
            allCitizens.Add(non_specialists);//0
            allCitizens.Add(doctors);//1
            allCitizens.Add(pharmacists);//2
            return allCitizens;
        }

        /// <summary>
        /// Gets all relatives of the citizen whose id matches the given citizenId
        /// </summary>
        /// <param name="citizenId">Citizen Id</param>
        /// <returns>ICollection</returns>
        public ICollection<Relative> GetRelatives(long citizenId)
        {
            return DBEntities.Relatives.Where(r => r.CitizenID == citizenId || r.RelativeID == citizenId).ToList();
        }


        public List<Citizen> GetPatientList(long doctorId,long placeId)
        {
            List<Citizen> patientList = new List<Citizen>();
            patientList = DBEntities.HistoryRecords.Where(patient => patient.SpecialistID == doctorId && patient.MedicalPlaceID==placeId).Select(p => p.Citizen).ToList();
            patientList = patientList.Distinct().ToList();
            return patientList;
        }

        /// <summary>
        /// Gets all relatives of a citizen that match the relationId
        /// </summary>
        /// <param name="citizenID">Citizen Id</param>
        /// <param name="relationID">Relation Type Id</param>
        /// <returns>ICollection</returns>
        public ICollection<Citizen> GetCitizenRelatives(long citizenID, long relationID)
        {
            ICollection<Citizen> relatives = (DBEntities.Relatives.Where(relative => relative.CitizenID == citizenID 
                                               && relative.RelationTypeID == relationID && relative.CitizenConfirmed==true 
                                               && relative.RelativeConfirmed==true).ToList())
                                              .Select(relative => relative.RelativeCitizen).ToList();
            return relatives;
        }

        /// <summary>
        /// Gets all relation types from database
        /// </summary>
        /// <returns>ICollection</returns>
        public ICollection<RelationType> GetRelationTypes()
        {
            return DBEntities.RelationTypes.ToList();
        }

        /// <summary>
        /// Confirms all relation requests to the citizen whose Id matches the given citizenId
        /// </summary>
        /// <param name="citizenId">Citizen Id</param>
        public void ConfirmAllRelatives(long citizenId)
        {
            var relatives = GetRelatives(citizenId);
            foreach(var relative in relatives)
            {
                DBEntities.Entry(relative).State = EntityState.Modified;
                relative.CitizenConfirmed = true;
                relative.RelativeConfirmed = true;
            }
            DBEntities.SaveChanges();
        }

        /// <summary>
        /// Sets IsRead to true for all Potential Diseases for the citizen
        /// </summary>
        /// <param name="citizenId">Citizen Id</param>
        public void ReadAllPotentialDiseases(long citizenId)
        {
            var potentialDiseases = DBEntities.PotentialDiseases.Where(p => p.CitizenID == citizenId);
            foreach(var disease in potentialDiseases)
            {
                DBEntities.Entry(disease).State = EntityState.Modified;
                disease.IsRead = true;
            }
            DBEntities.SaveChanges();
        }

        /// <summary>
        /// Adds Relative to Specific Citizen
        /// </summary>
        /// <param name="citizenId">Citizen Id</param>
        /// <param name="relativeId">Relative Id</param>
        /// <param name="relationId">Relation Type Id</param>
        /// <param name="notifyRelative">delegate that runs after adding relative to notify user</param>
        public void AddRelative(long citizenId,long relativeId,int relationId,Action notifyRelative = null)
        {
            Relative relative = new Relative();
            if (relationId != 4)
            {
                relative.CitizenID = citizenId;
                relative.RelativeID = relativeId;
                relative.RelationTypeID = relationId;
                relative.CitizenConfirmed = true;
                relative.RelativeConfirmed = false;
            }
            else
            {
                relative.RelativeID = citizenId;
                relative.CitizenID = relativeId;
                relative.RelationTypeID = 1;
                relative.RelativeConfirmed = true;
                relative.CitizenConfirmed = false;
            }

            DBEntities.Relatives.Add(relative);
            DBEntities.SaveChanges();
            notifyRelative?.Invoke();
        }

        /// <summary>
        /// Removes Relation between two citizens
        /// </summary>
        /// <param name="citizenId">Citizen Id</param>
        /// <param name="relativeId">Relative Id</param>
        /// <param name="notifyRelative">delegate that runs after removing relation to notify user</param>
        public void RemoveRelation(long citizenId,long relativeId,Action notifyRelative = null)
        {
            DBEntities.Relatives.Remove(DBEntities.Relatives.SingleOrDefault(r => (r.CitizenID == citizenId && r.RelativeID == relativeId) || (r.RelativeID == citizenId && r.CitizenID == relativeId)));
            DBEntities.SaveChanges();
            notifyRelative?.Invoke();
        }

        /// <summary>
        /// Checks if the relation between two citizens is confirmed
        /// </summary>
        /// <param name="citizenId">Citizen Id</param>
        /// <param name="relativeId">Relative Id</param>
        /// <returns>bool</returns>
        public bool IsRelationConfirmed(long citizenId, long relativeId)
        {
            return DBEntities.Relatives.Any(r => ((r.CitizenID == citizenId && r.RelativeID == relativeId) || (r.RelativeID == citizenId && r.CitizenID == relativeId)) && (r.CitizenConfirmed ?? false) && (r.RelativeConfirmed ?? false));
        }

        /// <summary>
        /// Gets all potential diseases for specific citizen
        /// </summary>
        /// <param name="citizenId">Citizen Id</param>
        /// <returns>ICollection</returns>
        public ICollection<PotentialDisease> GetPotintialDiseases(long citizenId)
        {
            return DBEntities.Citizens.SingleOrDefault(c => c.Id == citizenId).PotentialDiseases.ToList();
        }

        /// <summary>
        /// Sets all attachments of specific type as Read
        /// </summary>
        /// <param name="citizenId">Citizen Id</param>
        /// <param name="typeId">Relative Id</param>
        public void ReadAttachmentsOfType(long citizenId, int typeId)
        {
            var attachments = DBEntities.Attachments.Where( a => a.TypeID == typeId && a.CitizenID == citizenId);
            foreach(var attachment in attachments)
            {
                DBEntities.Entry(attachment).State = EntityState.Modified;
                attachment.IsRead = true;

            }
            DBEntities.SaveChanges();
        }    

        public string GetSpeciality(long specialistId)
        {
            Specialist specialist = (Specialist)DBEntities.Citizens.SingleOrDefault(citizen => citizen.Id == specialistId);
            string speciality = specialist.Speciality.Name;
            return speciality;
        }

        public ICollection<Pharmacy> GetSpecialistPharmacyPlace(long specialistId)
        {
            List<Pharmacy> pharmacies = new List<Pharmacy>();
            pharmacies = DBEntities.PharmacyMembershipRequests.Where(pharmacy => pharmacy.SpecialistID == specialistId && pharmacy.IsConfirmed == true).Select(p => p.Pharmacy).ToList();
            return pharmacies;
        }
        public ICollection<MedicalPlace>GetSpecialistWorkPlaces(long specialistId)
        {
            List<MedicalPlace> medicalPlaces = new List<MedicalPlace>();
            medicalPlaces = DBEntities.ServiceMembershipRequests.Where(service => service.IsConfirmed == true && service.SpecialistID == specialistId).Select(service => service.Service.MedicalPlace).ToList();
            medicalPlaces=medicalPlaces.Union(DBEntities.CareUnitMembershipRequests.Where(careUnit => careUnit.IsConfirmed == true && careUnit.SpecialistID == specialistId).Select(care => care.CareUnit.MedicalPlace).ToList()).ToList();
            return medicalPlaces;
        }
    }
}
