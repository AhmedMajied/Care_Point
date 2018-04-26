using BLL;
using DAL;
using Extensions;
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

        public CitizenBusinessLayer citizenBusinessLayer
        {
            get
            {
                return _citizenBusinessLayer ?? new CitizenBusinessLayer();
            }
            private set
            {
                _citizenBusinessLayer = value;
            }
        }

        public ActionResult CurrentPatient(long citizenID)
        {
            Citizen citizen = _citizenBusinessLayer.GetCitizen(citizenID);

            return View(citizen);
        }
        public ActionResult MedicalHistory(long id)
        {
            var user = User.Identity.GetCitizen();
            if (user is Models.Specialist || id == user.Id)
            {
                return View(citizenBusinessLayer.GetCitizen(id).HistoryRecords);
            }
            else
            {
                return new HttpUnauthorizedResult();
            }
        }

        // GET: Attachments
        public ActionResult Attachments(long id)
        {
            var user = User.Identity.GetCitizen();
            if (user is Models.Specialist || id == user.Id)
            {
                return View(citizenBusinessLayer.GetCitizen(id).Attachments);
            }
            else
            {
                return new HttpUnauthorizedResult();
            }
        }
    }
}