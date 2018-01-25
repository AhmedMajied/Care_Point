using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class CitizenBusinessLayer
    {
        private CarePointEntities DBEntities;

        public CitizenBusinessLayer()
        {
            DBEntities = new CarePointEntities();
        }

        public ICollection<Attachment> getPatientAttachments(long citizenID)
        {
            return DBEntities.Attachments.Where(attachment => attachment.CitizenID == citizenID).ToList();
            // not tested
        }

        public ICollection<HistoryRecord> getPatientHistory(long citizenID)
        {
            return DBEntities.HistoryRecords.Where(record => record.CitizenID == citizenID).ToList();
            // not tested
        }
    }
}
