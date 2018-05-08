using DAL;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarePoint.Models;
using Extensions;

namespace CarePoint.Controllers
{
    public class SOSController : Controller
    {
        private SOSBusinessLayer _sosBusinessLayer;
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
        public void SendSos(SOSViewModel sosViewModel)
        {
            int friend = 1, parent = 2, sibling = 3;
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
            sosBusinessLayer.AddSOS(sos);
            if (sosViewModel.isMedicalPlace)
            {
                MedicalPlaceBusinessLayer medicalPlaceBL = new MedicalPlaceBusinessLayer();
                ICollection<MedicalPlace>medicalPlaces=medicalPlaceBL.GetNearestNMedicalPlace(pointString, numberOfPlaces);
            }
            if (sosViewModel.isFriend)
            {
                CitizenBusinessLayer citizenBusinessLayer = new CitizenBusinessLayer();
                ICollection<Citizen> friends = citizenBusinessLayer.GetCitizenRelatives(user.Id, friend);
            }
            if (sosViewModel.isFamily)
            {
                CitizenBusinessLayer citizenBusinessLayer = new CitizenBusinessLayer();
                ICollection<Citizen> family = citizenBusinessLayer.GetCitizenRelatives(user.Id, parent);
                family.Union(citizenBusinessLayer.GetCitizenRelatives(user.Id, sibling));
            }
        }
    }
}