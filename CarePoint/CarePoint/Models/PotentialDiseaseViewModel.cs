using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarePoint.Models
{
    public class PotentialDiseaseViewModel
    {
        public string DiseaseName { get; set; }
        public int Level { get; set; }
        public DateTime TimeStamp { get; set; }
        public int NumberOfCasualties { get; set; }
        public bool IsRead { get; set; }
    }
}