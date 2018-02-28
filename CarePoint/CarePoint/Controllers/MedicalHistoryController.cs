using BLL;
using CarePoint.Models;
using Extensions;
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
        private CitizenBusinessLayer _citizenBusinessLayer;
        public MedicalHistoryController()
        {

        }
        public MedicalHistoryController(CitizenBusinessLayer citizenBusinessLayer)
        {
            _citizenBusinessLayer = citizenBusinessLayer;
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

        //GET: MedicalHistory
        public ActionResult MedicalHistory(long id)
        {
            var user = User.Identity.GetCitizen();
            if (user is Specialist || id == user.Id)
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
            if (user is Specialist || id == user.Id)
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