using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarePoint.Controllers
{
    [Authorize]
    public class MedicalHistoryController : Controller
    {
        // GET: Attachments
        public ActionResult Index()
        {
            return View("MedicalHistory");
        }
        public ActionResult Attachments()
        {
            return View();
        }

    }
}