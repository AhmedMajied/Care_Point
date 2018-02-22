using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using CarePoint.Models;

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

        public ActionResult ProfilePage(long id)
        {
            var medicalPlace = MedicalPlaceBusinessLayer.GetMedicalPlace(id);
            MedicalPlaceProfileViewModel model = new MedicalPlaceProfileViewModel()
            {
                Services = new List<ServiceViewModel>()
            };
            
            foreach (var service in medicalPlace.Services)
            {
                var groupedSlots = service.WorkSlots.GroupBy(slot => new { slot.StartTime, slot.EndTime });
                ServiceViewModel smodel = new ServiceViewModel()
                {
                    ID=service.ID,
                    Name=service.Name,
                    Cost=service.Cost??0,
                    Description=service.Description,
                    CategoryID=service.CategoryID,
                    WorkSlots = new List<WorkSlotViewModel>()
                };
                foreach (var element in groupedSlots)
                {
                    WorkSlotViewModel wmodel = new WorkSlotViewModel();
                    wmodel.StartTime = element.Key.StartTime ?? TimeSpan.Zero;
                    wmodel.EndTime = element.Key.EndTime ?? TimeSpan.Zero;
                    foreach(var day in element)
                    {
                        switch (day.DayName)
                        {
                            case "Saturday":
                                wmodel.IsSaturday = true;
                                break;
                            case "Sunday":
                                wmodel.IsSunday = true;
                                break;
                            case "Monday":
                                wmodel.IsMonday = true;
                                break;
                            case "Tuesday":
                                wmodel.IsTuesday = true;
                                break;
                            case "Wednesday":
                                wmodel.IsWednesday = true;
                                break;
                            case "Thursday":
                                wmodel.IsThursday = true;
                                break;
                            case "Friday":
                                wmodel.IsFriday = true;
                                break;
                        }
                        smodel.WorkSlots.Add(wmodel);
                    }
                    model.Services.Add(smodel);
                }
            }
            return View(model);
        }
    }
}