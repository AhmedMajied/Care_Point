using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarePoint.Models
{
    public class AttachmentViewModel
    {
        public bool IsRead { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string SpecialistName { get; set; }
        public DateTime Date { get; set; }
    }
}