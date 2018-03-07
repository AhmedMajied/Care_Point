using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.IO;

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
        }

        public ICollection<AttachmentType> GetAttachmentTypes()
        {
            return DBEntities.AttachmentTypes.ToList();
        }

        public void AddPrescription(HistoryRecord historyRecord)
        {

            // Draw prescription as image here

            // add prescription as an attachment
            Attachment attachment = new Attachment
            {
                TypeID = 3,
                Date = historyRecord.Date,
                SpecialistID = historyRecord.SpecialistID,
                CitizenID = historyRecord.CitizenID,
                FilePath = ("~/Attachments/Prescriptions/") + historyRecord.Date.ToString()
                            +".jpg",
                FileName = historyRecord.Date.ToString()
            };

            // save attachment to database
            DBEntities.Attachments.Add(attachment);

            // save history record to database
            DBEntities.HistoryRecords.Add(historyRecord);
            DBEntities.SaveChanges();
        }

    }
}
