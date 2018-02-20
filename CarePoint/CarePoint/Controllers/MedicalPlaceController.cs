using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;

namespace CarePoint.Controllers
{
    public class MedicalPlaceController : Controller
    {
        // GET: MedicalPlace
        private MedicalPlaceBusinessLayer _medicalPlaceBusinessLayer;
        public MedicalPlaceController()
        {
                
        }

        public MedicalPlaceController(MedicalPlaceBusinessLayer medicalPlaceBusinessLayer)
        {
            _medicalPlaceBusinessLayer = medicalPlaceBusinessLayer;
        }

        public MedicalPlaceBusinessLayer MedicalPlaceBusinessLayer
        {
            get
            {
                return _medicalPlaceBusinessLayer ?? new MedicalPlaceBusinessLayer();
            }
            private set
            {
                _medicalPlaceBusinessLayer = value;
            }
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}