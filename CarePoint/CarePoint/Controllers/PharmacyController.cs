using BLL;
using CarePoint.Models;
using DAL;
using Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CarePoint.Models.MedicineViewModels;

namespace CarePoint.Controllers
{
    public class PharmacyController : Controller
    {
        private PharmacyBusinessLayer _pharmacyBusinessLayer;

        public PharmacyController()
        {
            _pharmacyBusinessLayer = new PharmacyBusinessLayer();
        }
        public PharmacyController(PharmacyBusinessLayer pharmacyBusinessLayer)
        {
            _pharmacyBusinessLayer = pharmacyBusinessLayer;
        }
        public PharmacyBusinessLayer PharmacyBusinessLayer
        {
            get
            {
                return _pharmacyBusinessLayer ?? new PharmacyBusinessLayer();
            }
            private set
            {
                _pharmacyBusinessLayer = value;
            }
        }
        // GET: Pharmacy
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PharmacyProfile(long Id)
        {
            Pharmacy pharmacy = new Pharmacy();
            pharmacy = PharmacyBusinessLayer.GetPharmacy(Id);
            return View("Profile",pharmacy);
        }
        public ActionResult AddPharmacy(MedicalPlaceViewModels model)
        {
            Pharmacy pharmacy = new Pharmacy();
            pharmacy.IsConfirmed = false;
            double latitude = model.latitude;
            double longitude = model.longitude;
            var pointString = string.Format("POINT({0} {1})", longitude.ToString(), latitude.ToString());
            var location = DbGeography.FromText(pointString);
            pharmacy.Location = location;
            pharmacy.Name = model.medicalPlace.Name;
            pharmacy.Address = model.medicalPlace.Address;
            pharmacy.Phone = model.medicalPlace.Phone;
            pharmacy.OwnerID = User.Identity.GetCitizen().Id;
            using (var binaryReader = new BinaryReader(model.medicalPlace.Photo.InputStream))
            {
                pharmacy.Photo = binaryReader.ReadBytes(model.medicalPlace.Photo.ContentLength);
            }
            using (var binaryReader = new BinaryReader(model.medicalPlace.Permission.InputStream))
            {
                pharmacy.Permission = binaryReader.ReadBytes(model.medicalPlace.Permission.ContentLength);
            }
            PharmacyBusinessLayer.AddPharmacy(pharmacy);
            return PharmacyProfile(pharmacy.ID);
        }
        public JsonResult SearchPharmacyMedicine(SearchMedicineViewModel model)
        {
            string location = string.Format("POINT({0} {1})", model.longitude, model.latitude);
            List<Pharmacy> result = PharmacyBusinessLayer.SearchMedicineInPharmacies(model.drugName, location).ToList();
            var pharmacies = result.Select(pharmacy => new { pharmacy.Name, Photo = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(pharmacy.Photo)) , pharmacy.Address , pharmacy.Phone});
            string drugName = model.drugName;
            return Json(new { pharmacies, drugName });
        }
    }
}