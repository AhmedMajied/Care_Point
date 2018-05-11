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
using Microsoft.AspNet.Identity;
using System.Collections;

namespace CarePoint.Controllers
{
    public class CitizenController : Controller
    {
        private CitizenBusinessLayer _citizenBusinessLayer;

        public CitizenController()
        {
            _citizenBusinessLayer = new CitizenBusinessLayer();
        }

        public CitizenBusinessLayer CitizenBusinessLayer
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
            Citizen citizen = CitizenBusinessLayer.GetCitizen(citizenID);

            return View(citizen);
        }
        public ActionResult MedicalHistory(long id)
        {
            var user = User.Identity.GetCitizen();
            if (user is Models.Specialist || id == user.Id)
            {
                return View(CitizenBusinessLayer.GetCitizen(id).HistoryRecords);
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
                return View(CitizenBusinessLayer.GetCitizen(id).Attachments);
            }
            else
            {
                return new HttpUnauthorizedResult();
            }
        }
        private List<dynamic> GetSearchResult(IEnumerable<Citizen> queryResult)
        {
            List<dynamic> result = new List<dynamic>();
            foreach(var res in queryResult)
            {
                dynamic obj;
                var relative = res.Relatives.SingleOrDefault(r => r.CitizenID == User.Identity.GetUserId<long>());
                if(relative != null)
                {
                    obj = new { res.Name, res.Id, res.Photo,Relation=relative.RelationType.Name };
                }
                else
                {
                    obj = new { res.Name, res.Id, res.Photo, Relation = "None" };
                }
                relative = res.AddedRelatives.SingleOrDefault(r => r.RelativeID == User.Identity.GetUserId<long>());
                if (relative != null)
                {
                    if (relative.RelationType.Name == "Parent")
                        obj = new { res.Name, res.Id, res.Photo, Relation = "Scion" };
                    else
                        obj = new { res.Name, res.Id, res.Photo, Relation = relative.RelationType.Name };
                }
                else
                {
                    obj = new { res.Name, res.Id, res.Photo, Relation = "None" };
                }

                result.Add(obj);

            }
            return result;
        }
        public JsonResult SearchAccount(string key, string value)
        {
            List<List<Citizen>> allCitizens = _citizenBusinessLayer.searchAccounts(key, value);
            var citizens = GetSearchResult(allCitizens[0]);
            var doctors = GetSearchResult(allCitizens[1]);
            var pharmacists = GetSearchResult(allCitizens[2]);
            var res = new{citizens, doctors, pharmacists};
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

        public JsonResult AddRelative(long relativeId, string relationType)
        {
            Relative relative = new Relative();
            int relationId = (relationType == "Parent") ? 1 : (relationType == "Friend") ? 3 : 4;
            if(relationId != 4)
            {
                relative.CitizenID = User.Identity.GetUserId<long>();
                relative.RelativeID = relativeId;
                relative.RelationTypeID = relationId;
                relative.CitizenConfirmed = true;
            }
            else
            {
                relative.RelativeID = User.Identity.GetUserId<long>();
                relative.CitizenID = relativeId;
                relative.RelationTypeID = 1;
                relative.RelativeConfirmed = true;
            }
            try
            {
                CitizenBusinessLayer.AddRelative(relative);
                return Json("Added Successfully");
            }
            catch(Exception e)
            {
                return Json(e.InnerException.InnerException.Message);
            }
        }

        public void RemoveRelation(long relativeId)
        {
            CitizenBusinessLayer.RemoveRelation(User.Identity.GetUserId<long>(), relativeId);
        }
    }
}