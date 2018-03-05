using BLL;
using CarePoint.Models;
using DAL;
using Extensions;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Mvc;
namespace CarePoint.Controllers
{
    [Authorize]
    public class MedicalPlaceController : Controller
    {
        private MedicalPlaceBusinessLayer medicalPlaceBusinessLayer;
        public MedicalPlaceController()
        {
            medicalPlaceBusinessLayer = new MedicalPlaceBusinessLayer();
        }
        // GET: MedicalPlace
        public ActionResult MedicalPlace()
        {
            MedicalPlaceViewModels model = new MedicalPlaceViewModels();
            ICollection<MedicalPlaceType> medicalPlaceTypes = medicalPlaceBusinessLayer.getAllTypes();
            List<SelectListItem> dropDownList = new List<SelectListItem>();
            foreach (MedicalPlaceType medicalType in medicalPlaceTypes)
            {
                dropDownList.Add(new SelectListItem { Text = medicalType.Name, Value = medicalType.ID.ToString() });
            }
            model.medicalPlaceTypes = dropDownList;
            return View("AddMedicalPlace", model);
        }
        public void AddMedicalPlace(MedicalPlaceViewModels model)
        {
            DAL.MedicalPlace newPlace = new DAL.MedicalPlace();
            newPlace.Address = model.medicalPlace.Address;
            newPlace.Description = model.medicalPlace.Description;
            newPlace.Name = model.medicalPlace.Name;
            newPlace.IsConfirmed = false;
            newPlace.OwnerID = User.Identity.GetCitizen().Id;
            newPlace.Phone = model.medicalPlace.Phone;
            newPlace.TypeID = model.medicalPlace.TypeID;
            double latitude = model.latitude;
            double longitude = model.longitude;
            var pointString = string.Format("POINT({0} {1})", longitude.ToString(), latitude.ToString());
            var point = DbGeography.FromText(pointString);
            newPlace.Location = point;
            using (var binaryReader = new BinaryReader(model.medicalPlace.Photo.InputStream))
            {
                newPlace.Photo = binaryReader.ReadBytes(model.medicalPlace.Photo.ContentLength);
            }
            using (var binaryReader = new BinaryReader(model.medicalPlace.Permission.InputStream))
            {
                newPlace.Permission = binaryReader.ReadBytes(model.medicalPlace.Permission.ContentLength);
            }
            Debug.WriteLine("Name " + newPlace.Name);
            Debug.WriteLine("adds " + newPlace.Address);
            Debug.WriteLine("desc " + newPlace.Description);
            Debug.WriteLine("conf " + newPlace.IsConfirmed);
            Debug.WriteLine("lat " + newPlace.Location.Latitude + "  long  " + newPlace.Location.Longitude);
            Debug.WriteLine("ownID " + newPlace.OwnerID);
            Debug.WriteLine("perm " + newPlace.Permission);
            Debug.WriteLine("phon " + newPlace.Phone);
            Debug.WriteLine("photo" + newPlace.Photo);
            Debug.WriteLine("tyID " + newPlace.TypeID);





            medicalPlaceBusinessLayer.addMedicalPlace(newPlace);
            //return View("ProfilePage", newPlace.ID);
        }
    }
}