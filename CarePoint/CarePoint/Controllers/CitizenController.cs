using BLL;
using DAL;
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


        public ActionResult CurrentPatient(long citizenID)
        {
            Citizen citizen = _citizenBusinessLayer.GetCitizen(citizenID);

            return View(citizen);
        }
        public JsonResult SearchAccount(string key, string value)
        {
            List<List<Citizen>> allCitizens = _citizenBusinessLayer.searchAccounts(key, value);
            var citizens = allCitizens[0].Select(x => new { x.Name, x.Id, x.Photo });
            var doctors = allCitizens[1].Select(x => new { x.Name, x.Id, x.Photo });
            var pharmacists = allCitizens[2].Select(x => new { x.Name, x.Id, x.Photo });
            var res = new[] {citizens, doctors, pharmacists };
            return Json(res);
        }
        public JsonResult PatientsList(long doctorId)
        {
            List<Citizen> list = _citizenBusinessLayer.getPatientList(doctorId);
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