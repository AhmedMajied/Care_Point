using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarePoint.Models
{
    public class SOSViewModel
    {
        public bool IsMedicalPlace { get; set; }
        public bool IsFamily { get; set; }
        public bool IsFriend { get; set; }
        public string Description { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}