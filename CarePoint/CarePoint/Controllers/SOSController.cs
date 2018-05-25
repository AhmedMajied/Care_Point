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
        public JsonResult SendSos(SOSViewModel sosViewModel)
        {
            List<RelationType> relationTypes = citizenBusinessLayer.GetRelationTypes().ToList();
            long friend = relationTypes.Single(r => r.Name == "Friend").ID;
            long parent = relationTypes.Single(r => r.Name == "Parent").ID;
            long sibling = relationTypes.Single(r=>r.Name == "Sibling").ID;
            int numberOfPlaces = 5;
            SOSs sos = new SOSs();
            var user = User.Identity.GetCitizen();
            var pointString = string.Format("POINT({0} {1})", sosViewModel.longitude.ToString(), sosViewModel.latitude.ToString());
            var location = System.Data.Entity.Spatial.DbGeography.FromText(pointString);
            DateTime time = DateTime.Now;
            sos.Description = sosViewModel.description;
            sos.SenderID =user.Id;
            sos.Time =time;
            sos.IsAccepted = false;
            sos.Location = location;
            // what is need of medicalPlaceID 
            List<Citizen> citizens = new List<Citizen>();
            if (sosViewModel.isMedicalPlace)
            {
                MedicalPlaceBusinessLayer medicalPlaceBL = new MedicalPlaceBusinessLayer();
                citizens=(List<Citizen>)(medicalPlaceBL.GetContributersOfAmbulanceService(pointString, numberOfPlaces));
            }
            if (sosViewModel.isFriend)
            {
                citizens.Union(citizenBusinessLayer.GetCitizenRelatives(user.Id, friend).ToList());
            }
            if (sosViewModel.isFamily)
            {
                citizens.Union(citizenBusinessLayer.GetCitizenRelatives(user.Id, parent).ToList());
                citizens.Union(citizenBusinessLayer.GetCitizenRelatives(user.Id, sibling).ToList());
            }
            try
            {
                // sosBusinessLayer.AddSOS(sos);
                // sosBusinessLayer.SaveNotifications(citizens,time,user.Name+" Requests SOS and Says : "+ sosViewModel.description);
                SOSNotificationViewModel s = new SOSNotificationViewModel();
                s.description = sosViewModel.description;
                s.latitude = sosViewModel.latitude;
                s.longitude = sosViewModel.longitude;
                s.userPhoneNumber = user.PhoneNumber;
                SosHub.StaticNotify(citizens.Select(c=>c.Id).ToList(),s);
                return Json("Your Request is Successfully Sent");
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }

        public void AcceptSOS(long sosId , long hospitalID)
        {
            sosBusinessLayer.AcceptSOS(sosId, hospitalID);
        }
    }
}