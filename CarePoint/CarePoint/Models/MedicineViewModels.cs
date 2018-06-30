using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarePoint.Models
{
    public class MedicineViewModels
    {
        public class SearchMedicineViewModel
        {
            public string DrugName { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }
}