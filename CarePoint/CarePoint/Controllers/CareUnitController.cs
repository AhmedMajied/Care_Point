using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using DAL;

namespace CarePoint.Controllers
{
<<<<<<< HEAD

=======
    
>>>>>>> master
    public class CareUnitController : Controller
    {
        private CareUnitBusinessLayer _careUnitBusinessLayer;

        public CareUnitController()
        {
            _careUnitBusinessLayer = new CareUnitBusinessLayer();
        }
<<<<<<< HEAD

        // GET: CareUnit
        public ActionResult Index()
        {
            ICollection<CareUnit> careUnits = _careUnitBusinessLayer.GetMedicalPlaceCareUnits(4);
=======
        
        // GET: CareUnit
        public ActionResult Index()
        {
            ICollection<CareUnit> careUnits = _careUnitBusinessLayer.getMedicalPlaceCareUnits(4);
>>>>>>> master

            return View(careUnits);
        }

        [HttpPost]
        public void UpdateCareUnitsCount(List<CareUnit> careUnits)
        {
<<<<<<< HEAD
            _careUnitBusinessLayer.UpdateAvailableRoomCount(careUnits);
        }

=======
            _careUnitBusinessLayer.updateAvailableRoomCount(careUnits);
        }
        
>>>>>>> master
    }
}