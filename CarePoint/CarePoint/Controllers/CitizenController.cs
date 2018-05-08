using BLL;
using DAL;
using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarePoint.Models;
using System.Diagnostics;

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
        public JsonResult SearchAccount(string key, string value)
        {
            List<List<Citizen>> allCitizens = _citizenBusinessLayer.SearchAccounts(key, value);
            var citizens = allCitizens[0].Select(x => new { x.Name, x.Id, x.Photo });
            var doctors = allCitizens[1].Select(x => new { x.Name, x.Id, x.Photo });
            var pharmacists = allCitizens[2].Select(x => new { x.Name, x.Id, x.Photo });
            var res = new[] {citizens, doctors, pharmacists };
            return Json(res);
        }

        public JsonResult PatientsList(long doctorId)
        {
            List<Citizen> list = _citizenBusinessLayer.GetPatientList(doctorId);
            List<Citizen> maleList = new List<Citizen>();
            List<Citizen> femaleList = new List<Citizen>();
            foreach (Citizen c in list)
            {
                if (c.Gender.ToLower().Equals("male"))
                    maleList.Add(c);
                else if (c.Gender.ToLower().Equals("female"))
                    femaleList.Add(c);
            }
            var males = maleList.Select(x => new { x.Name, x.Id, x.Photo });
            var females = femaleList.Select(x => new { x.Name, x.Id, x.Photo });
            var result = new[] { males, females };
            return Json(result);
        }
    }
}