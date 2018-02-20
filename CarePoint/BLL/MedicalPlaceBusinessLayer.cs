using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class MedicalPlaceBusinessLayer
    {
        private CarePointEntities DBEntities { get; set; }
        public MedicalPlaceBusinessLayer()
        {
            DBEntities = new CarePointEntities();
        }
        public MedicalPlace GetMedicalPlace(long id)
        {
            return DBEntities.MedicalPlaces.SingleOrDefault(place => place.ID == id);
        }
    }
}
