using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using DAL;

namespace CarePoint.Controllers
{
    public class CareUnitController : Controller
    {
        private CareUnitBusinessLayer _careUnitBusinessLayer;

        public CareUnitController()
        {
            _careUnitBusinessLayer = new CareUnitBusinessLayer();
        }
        
        // GET: CareUnit
        public ActionResult Index()
        {
            ICollection<CareUnit> careUnits = _careUnitBusinessLayer.getMedicalPlaceCareUnits(4);

            return View(careUnits);
        }

        [HttpPost]
        public void UpdateCareUnitsCount(List<CareUnit> careUnits)
        {
            _careUnitBusinessLayer.updateAvailableRoomCount(careUnits);
        }
    }
}