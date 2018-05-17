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
        public bool SendSos(SOSViewModel sosViewModel)
        {
            List<RelationType> relationTypes = sosBusinessLayer.GetRelationTypes().ToList();
            long friend =relationTypes.Where(r => r.Name == "Friend").Select(r=>r.ID).ToList()[0];
            long parent= relationTypes.Where(r => r.Name == "Parent").Select(r => r.ID).ToList()[0];
            long sibling = relationTypes.Where(r=>r.Name == "Sibling").Select(r=>r.ID).ToList()[0];
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
                // for who will send the sos for hospitals ?? this notifications will saved to notifications table or not
                MedicalPlaceBusinessLayer medicalPlaceBL = new MedicalPlaceBusinessLayer();
                citizens=(List<Citizen>)(medicalPlaceBL.GetNearestNMedicalPlace(pointString, numberOfPlaces)).Select(o=>o.OwnerID);
            }
            if (sosViewModel.isFriend)
            {
                citizens = citizenBusinessLayer.GetCitizenRelatives(user.Id, friend).ToList();
            }
            if (sosViewModel.isFamily)
            {
                citizens = citizenBusinessLayer.GetCitizenRelatives(user.Id, parent).ToList();
                citizens.Union(citizenBusinessLayer.GetCitizenRelatives(user.Id, sibling).ToList());
            }
            // sosBusinessLayer.AddSOS(sos);
            // sosBusinessLayer.SaveNotifications(citizens,time,user.Name+" Requests SOS and Says : "+ sosViewModel.description);
            return true;
        }
        public void AcceptSOS(long sosId , long hospitalID)
        {
            sosBusinessLayer.AcceptSOS(sosId, hospitalID);
        }
    }
}