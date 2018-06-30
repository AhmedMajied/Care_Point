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
    public class CitizenBusinessLayer
    {
        private CarePointEntities DBEntities;

        public CitizenBusinessLayer()
        {
            DBEntities = new CarePointEntities();
        }

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
                return null;// if it is non base-64 character
            }
            return DBEntities.Citizens.SingleOrDefault(citizen => citizen.NationalIDNumber == nationalID);
        }   

        public Citizen GetCitizen(long citizenID)
        {
            return DBEntities.Citizens.SingleOrDefault(citizen => citizen.Id == citizenID);
        }
        
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
        public ICollection<Citizen> GetCitizenRelatives(long citizenID, long relationID)
        {
            ICollection<Citizen> relatives = (DBEntities.Relatives.Where(relative => relative.CitizenID == citizenID 
                                               && relative.RelationTypeID == relationID && relative.CitizenConfirmed==true 
                                               && relative.RelativeConfirmed==true).ToList())
                                              .Select(relative => relative.RelativeCitizen).ToList();
            return relatives;
        }
        public ICollection<RelationType> GetRelationTypes()
        {
            return DBEntities.RelationTypes.ToList();
        }
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

        public void RemoveRelation(long citizenId,long relativeId,Action notifyRelative = null)
        {
            DBEntities.Relatives.Remove(DBEntities.Relatives.SingleOrDefault(r => (r.CitizenID == citizenId && r.RelativeID == relativeId) || (r.RelativeID == citizenId && r.CitizenID == relativeId)));
            DBEntities.SaveChanges();
            notifyRelative?.Invoke();
        }

        public bool IsRelationConfirmed(long citizenId, long relativeId)
        {
            return DBEntities.Relatives.Any(r => ((r.CitizenID == citizenId && r.RelativeID == relativeId) || (r.RelativeID == citizenId && r.CitizenID == relativeId)) && (r.CitizenConfirmed ?? false) && (r.RelativeConfirmed ?? false));
        }

        public ICollection<PotentialDisease> GetPotintialDiseases(long citizenId)
        {
            return DBEntities.Citizens.SingleOrDefault(c => c.Id == citizenId).PotentialDiseases.ToList();
        }

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
