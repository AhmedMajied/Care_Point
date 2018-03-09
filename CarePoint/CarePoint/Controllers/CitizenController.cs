using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarePoint.Controllers
{
    public class CitizenController : Controller
    {
        private CitizenBusinessLayer _citizenBusinessLayer;

        public CitizenController()
        {
            _citizenBusinessLayer = new CitizenBusinessLayer();
        }


        public ActionResult CurrentPatient(long citizenID)
        {
            Citizen citizen = _citizenBusinessLayer.GetCitizen(citizenID);

            return View(citizen);
        }
    }
}