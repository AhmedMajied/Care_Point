using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using CarePoint.AuthorizeAttributes;
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
        public ActionResult Index(long id)
        {
            ICollection<CareUnit> careUnits = _careUnitBusinessLayer.GetMedicalPlaceCareUnits(id);
            return View(careUnits);
        }
        [HttpPost]
        [AccessDeniedAuthorize(Roles = "Doctor")]
        public ActionResult UpdateCareUnitsCount(List<CareUnit> careUnits)
        {
            _careUnitBusinessLayer.UpdateAvailableRoomCount(careUnits);
            long id = Convert.ToInt64((Request.Cookies["placeInfo"]).Values["id"]);
            return RedirectToAction("ProfilePage", "MedicalPlace", new { id =id });
        }

    }
}
