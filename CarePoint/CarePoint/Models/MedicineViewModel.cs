using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarePoint.Models
{
    public class MedicineViewModel
    {
        public class SearchMedicineViewModel
        {
            public string drugName { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
        }
    }
}