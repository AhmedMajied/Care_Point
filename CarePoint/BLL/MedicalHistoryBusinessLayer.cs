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

        public void addAttachment(Attachment attachment)
        {
            DBEntities.Attachments.Add(attachment);
            DBEntities.SaveChanges();
            
            // not tested
        }

        public void addPrescription(HistoryRecord historyRecord)
        {
            DBEntities.HistoryRecords.Add(historyRecord);
            DBEntities.SaveChanges();
            
            // not tested
        }

        public ICollection<Disease> getAllDiseases()
        {
            return DBEntities.Diseases.ToList();
        }

    }
}
