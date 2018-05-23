using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarePoint.Models
{
    public class RelativeViewModel
    {
        public long ID { get; set; }
        public string FullName { get; set; }
        public string RelationType { get; set; }
        public  byte[] Photo { get; set; }
    }

    public class RelativesPageViewModel
    {
        public ICollection<RelativeViewModel> NewConnections { get; set; }
        public ICollection<RelativeViewModel> Family { get; set; }
        public ICollection<RelativeViewModel> Friends { get; set; }
    }
}