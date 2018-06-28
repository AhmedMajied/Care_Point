using DAL;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using CarePoint.Models;
using Extensions;
using System.Diagnostics;
using CarePoint.Hubs;

namespace CarePoint.Controllers
{
    public class SOSController : Controller
    {
        private SOSBusinessLayer _sosBusinessLayer;
        private CitizenBusinessLayer _citizenBusinessLayer;
        // GET: SOS
        public ActionResult Index()
        {
            return View();
        }
        public SOSController()
        {
            _sosBusinessLayer = new SOSBusinessLayer();
        }
        public SOSBusinessLayer sosBusinessLayer
        {
            get
            {
                return _sosBusinessLayer ?? new SOSBusinessLayer();
            }
            private set
            {
                _sosBusinessLayer = value;
            }
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
        public JsonResult SendSos(SOSViewModel model)
        {

            List<RelationType> relationTypes = citizenBusinessLayer.GetRelationTypes().ToList();
            long friend = relationTypes.Single(r => r.Name == "Friend").ID;
            long parent = relationTypes.Single(r => r.Name == "Parent").ID;
            long sibling = relationTypes.Single(r=>r.Name == "Sibling").ID;
            int numberOfPlaces = 5;
            SOSs sos = new SOSs();
            var user = User.Identity.GetCitizen();
            var pointString = string.Format("POINT({0} {1})", model.longitude.ToString(), model.latitude.ToString());
            var location = System.Data.Entity.Spatial.DbGeography.FromText(pointString);
            DateTime time = DateTime.Now;
            sos.Description = model.description;
            sos.SenderID =user.Id;
            sos.StatusID = 1; // Pending 
            sos.Time =time;
            sos.IsAccepted = false;
            sos.Location = location;
            List<long> citizens = new List<long>();
            List<long> contributers = new List<long>();
            if (model.isMedicalPlace)
            {
                 contributers = (sosBusinessLayer.GetContributersOfSOSsServices(pointString, numberOfPlaces)).Where(s=>s.Id!=user.Id).Select(s=>s.Id).ToList();
            }
            if (model.isFriend)
            {
                citizens = (citizenBusinessLayer.GetCitizenRelatives(user.Id, friend)).Select(c=>c.Id).ToList();
            }
            if (model.isFamily)
            {
                citizens=citizens.Union(citizenBusinessLayer.GetCitizenRelatives(user.Id, parent).Select(c=>c.Id)).ToList();
                citizens=citizens.Union(citizenBusinessLayer.GetCitizenRelatives(user.Id, sibling).Select(c => c.Id)).ToList();
            }
            try
            {
                int citizensType = 2;
                int contributersType = 1;
                sosBusinessLayer.AddSOS(sos);
                NotificationsHub.NotifySOS(sos.ID,citizens,citizensType, model.description, model.latitude
                                    , model.longitude, user.PhoneNumber);
                NotificationsHub.NotifySOS(sos.ID,contributers, contributersType, model.description, model.latitude
                                    , model.longitude, user.PhoneNumber);
                return Json("Your Request is Successfully Sent");
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }
        public JsonResult AcceptSOS(long sosId , long hospitalId)
        {
            try
            {
                List<long> contributers = new List<long>();
                int numberOfPlaces = 5;
                string location = sosBusinessLayer.GetSOS(sosId).Location.ToString();
                sosBusinessLayer.AcceptSOS(sosId, hospitalId);
                contributers=(sosBusinessLayer.GetContributersOfSOSsServices(location, numberOfPlaces)).Select(s => s.Id).ToList();
                NotificationsHub.HideSOSNotification(contributers);
                return Json("SOS is Accepted");
            }
            catch(Exception e)
            {
                return Json(e.Message);
            }
        }
    }
}