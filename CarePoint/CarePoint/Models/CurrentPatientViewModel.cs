using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
namespace CarePoint.Models
{
    public class CurrentPatientViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string BloodType { get; set; }
        public byte[] Photo { get; set; }
        public int Age { get; set; }
        public ICollection<HistoryRecord> HistoryRecords { get; set; }
        public IDictionary<AttachmentType, List<AttachmentViewModel>> Attachments { get; set; }
    }
}