using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using DAL;

namespace CarePoint.Controllers
{
    public class MedicineController : Controller
    {
        private MedicineBusinessLayer _medicineBusinessLayer;

        public MedicineController()
        {
            _medicineBusinessLayer = new MedicineBusinessLayer();
        }

        public ICollection<Medicine> GetMedicineAlternatives(string medicineName)
        {
            return _medicineBusinessLayer.getMedicineAlternatives(medicineName);
        }

    }
}