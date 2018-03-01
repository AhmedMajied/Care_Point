using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarePoint.Controllers
{
    public class CitizenController : Controller
    {
        // GET: Citizen
        public ActionResult CurrentPatient()
        {
            return View();
        }
    }
}