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

        public ActionResult GetAllMedicines()
        {
            var medicines = _medicineBusinessLayer.GetAllMedicines().Select(medicine =>
                            new { medicine.ID, medicine.Name }).ToList();

            return Json(medicines);
        }

        public ActionResult GetMedicineAlternatives(string medicineName)
        {
            if (!medicineName.Equals(""))
            {
                var medicineAlternatives = _medicineBusinessLayer.getMedicineAlternatives(medicineName).
                Select(medicine => new { medicine.Name }).ToList();

                return Json(medicineAlternatives);
            }

            return null;
        }

    }
}