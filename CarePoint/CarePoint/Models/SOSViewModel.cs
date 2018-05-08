using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarePoint.Models
{
    public class SOSViewModel
    {
        public bool isMedicalPlace { get; set; }
        public bool isFamily { get; set; }
        public bool isFriend { get; set; }
        public string description { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
    }
}