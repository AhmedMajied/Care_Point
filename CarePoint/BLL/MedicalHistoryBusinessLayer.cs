using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class MedicalHistoryBusinessLayer
    {
        private CarePointEntities DBEntities;

        public MedicalHistoryBusinessLayer()
        {
            DBEntities = new CarePointEntities();
        }

        public void AddAttachment(Attachment attachment)
        {
            DBEntities.Attachments.Add(attachment);
            DBEntities.SaveChanges();
            
            // not tested
        }

        public ICollection<AttachmentType> GetAttachmentTypes()
        {
            return DBEntities.AttachmentTypes.ToList();
        }

        public void AddPrescription(HistoryRecord historyRecord)
        {
            DBEntities.HistoryRecords.Add(historyRecord);
            DBEntities.SaveChanges();
            
            // not tested
        }

        public ICollection<Disease> GetAllDiseases()
        {
            return DBEntities.Diseases.ToList();
        }

        public ICollection<Symptom> GetAllSymptoms()
        {
            return DBEntities.Symptoms.ToList();
        }

    }
}
