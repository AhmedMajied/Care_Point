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

        public void UpdateCitizen(Citizen modifiedCitizen)
        {
            DBEntities.Entry(modifiedCitizen).State = EntityState.Modified;
            DBEntities.SaveChanges();
            
            // not tested
        }

        public Specialist GetSpecialist(long specialistID)
        {
            return DBEntities.Citizens.OfType<Specialist>().Single(specialist => specialist.Id == specialistID);
            // not tested and need to be checked 
        }

        public void UpdateSpecialist(Specialist modifiedSpecialist)
        {
            DBEntities.Entry(modifiedSpecialist).State = EntityState.Modified;
            DBEntities.SaveChanges();

            //not tested and need to be checked 
        }

        public ICollection<Attachment> GetPatientAttachments(long citizenID)
        {
            return DBEntities.Attachments.Where(attachment => attachment.CitizenID == citizenID).ToList();
            // not tested
        }

        public ICollection<HistoryRecord> GetPatientHistory(long citizenID)
        {
            return DBEntities.HistoryRecords.Where(record => record.CitizenID == citizenID).ToList();
            // not tested
        }

        public ICollection<Speciality> GetSpecialities()
        {
            return DBEntities.Specialities.ToList();
        }

    }
}
