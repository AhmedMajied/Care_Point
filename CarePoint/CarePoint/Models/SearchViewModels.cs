using DAL;
using System.Collections.Generic;

namespace CarePoint.Models
    {
        public class AccountResultViewModel
        {
            public ICollection<Citizen> doctors { get; set; }
            public ICollection<Citizen> pharmacists { get; set; }
            public ICollection<Citizen> nonSpecialists { get; set; }
        }
    }