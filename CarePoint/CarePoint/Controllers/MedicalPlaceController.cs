using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using CarePoint.Models;
using DAL;
using Microsoft.AspNet.Identity;

namespace CarePoint.Controllers
{
    public class MedicalPlaceController : Controller
    {
        // GET: MedicalPlace
        private MedicalPlaceBusinessLayer _medicalPlaceBusinessLayer;
        private CareUnitBusinessLayer _careUnitBusinessLayer;

        public MedicalPlaceController()
        {
                
        }

        public MedicalPlaceController(MedicalPlaceBusinessLayer medicalPlaceBusinessLayer, CareUnitBusinessLayer careUnitBusinessLayer)
        {
            _medicalPlaceBusinessLayer = medicalPlaceBusinessLayer;
            _careUnitBusinessLayer = careUnitBusinessLayer;
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

        public CareUnitBusinessLayer CareUnitBusinessLayer
        {
            get
            {
                return _careUnitBusinessLayer ?? new CareUnitBusinessLayer();
            }
            private set
            {
                _careUnitBusinessLayer = value;
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
                MedicalPlaceID=medicalPlace.ID,
                Services = new List<ServiceViewModel>(),
                CareUnits =  new List<CareUnitViewModel>(),
                ServiceCategories = MedicalPlaceBusinessLayer.GetServiceCategories(),
                CareUnitTypes = CareUnitBusinessLayer.GetCareUnitTypes(),
                IsAdmin = medicalPlace.Specialists.Select(m => m.Id).Contains(User.Identity.GetUserId<long>())

            };
            
            
            foreach (var service in medicalPlace.Services)
            {
                var groupedSlots = service.WorkSlots.GroupBy(slot => new { slot.StartTime, slot.EndTime });
                ServiceViewModel smodel = new ServiceViewModel()
                {
                    ID = service.ID,
                    Name = service.Name,
                    Cost = service.Cost ?? 0,
                    Description = service.Description,
                    CategoryID = service.CategoryID,
                    ProviderID = service.ProviderID ?? 0,
                    WorkSlots = new List<WorkSlotViewModel>()
                };
                foreach (var element in groupedSlots)
                {
                    WorkSlotViewModel wmodel = new WorkSlotViewModel();
                    wmodel.StartTime = element.Key.StartTime ?? TimeSpan.Zero;
                    wmodel.EndTime = element.Key.EndTime ?? TimeSpan.Zero;
                    wmodel.ServiceID = service.ID;
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
                    }
                    smodel.WorkSlots.Add(wmodel);
                }
                model.Services.Add(smodel);
            }

            foreach(var careunit in medicalPlace.CareUnits)
            {
                CareUnitViewModel cmodel = new CareUnitViewModel()
                {
                    ID = careunit.ID,
                    Name = careunit.Name,
                    CareUnitTypeID = careunit.CareUnitTypeID,
                    AvailableRoomCount = careunit.AvailableRoomCount ?? 0,
                    Cost = careunit.Cost,
                    ProviderID = careunit.ProviderID,
                    Description = careunit.Description,
                    LastUpdate = careunit.LastUpdate
                };
                model.CareUnits.Add(cmodel);
            }

            return View("ProfilePage",model);
        }
        public void AddWorkslot(WorkSlotViewModel model)
        {
            foreach(var attribute in model.GetType().GetProperties()) {
                if(attribute.PropertyType.Name=="Boolean" && (Boolean)attribute.GetValue(model) == true && attribute.Name.Substring(attribute.Name.Length-3) == "day")
                {
                    WorkSlot slot = new WorkSlot()
                    {
                        StartTime = model.StartTime,
                        EndTime = model.EndTime,
                        ServiceID = model.ServiceID,
                        DayName = attribute.Name.Substring(2)
                    };
                    MedicalPlaceBusinessLayer.AddWorkSlot(slot);
                }
            }
        }
        public void RemoveWorkslot(long ServiceID,TimeSpan StartTime,TimeSpan EndTime)
        {
            MedicalPlaceBusinessLayer.RemoveWorkslot(ServiceID, StartTime, EndTime);
        }

        public ActionResult UpdateSchedule(MedicalPlaceProfileViewModel model)
        {
            if(ModelState.IsValid)
            {
                ServiceViewModel serviceModel = model.Services.First();
                Service service = new Service()
                {
                    ID = serviceModel.ID,
                    Name = serviceModel.Name,
                    CategoryID = serviceModel.CategoryID,
                    Cost = serviceModel.Cost,
                    Description = serviceModel.Description,
                    ProviderID = serviceModel.ProviderID
                };
                MedicalPlaceBusinessLayer.UpdateService(service);
            }
            return ProfilePage(model.Services.First().ProviderID);
        }

        [HttpPost]
        public void UpdateCareUnitsCount(List<CareUnit> careUnits)
        {
            CareUnitBusinessLayer.UpdateAvailableRoomCount(careUnits);
        }

        [HttpPost]
        public ActionResult UpdateCareUnit(MedicalPlaceProfileViewModel model)
        {
            if (ModelState.IsValid)
            {

                CareUnit careunit = new CareUnit()
                {
                    ID= model.CareUnits.ElementAt(0).ID,
                    Name = model.CareUnits.ElementAt(0).Name,
                    Description = model.CareUnits.ElementAt(0).Description,
                    Cost = model.CareUnits.ElementAt(0).Cost,
                    LastUpdate = model.CareUnits.ElementAt(0).LastUpdate,
                    CareUnitTypeID = model.CareUnits.ElementAt(0).CareUnitTypeID,
                    AvailableRoomCount = model.CareUnits.ElementAt(0).AvailableRoomCount,
                    ProviderID = model.CareUnits.ElementAt(0).ProviderID
                };
                CareUnitBusinessLayer.UpdateCareUnit(careunit);
            }
            return ProfilePage(model.MedicalPlaceID);
        }

        [HttpPost]
        public ActionResult AddCareUnit(MedicalPlaceProfileViewModel model)
        {
            if (ModelState.IsValid)
            {

                CareUnit careunit = new CareUnit()
                {
                    Name = model.NewCareUnit.Name,
                    Description = model.NewCareUnit.Description,
                    Cost = model.NewCareUnit.Cost,
                    LastUpdate = model.NewCareUnit.LastUpdate,
                    CareUnitTypeID = model.NewCareUnit.CareUnitTypeID,
                    AvailableRoomCount = model.NewCareUnit.AvailableRoomCount,
                    ProviderID = model.NewCareUnit.ProviderID
                };
                CareUnitBusinessLayer.AddCareUnit(careunit);
            }
            return ProfilePage(model.MedicalPlaceID);
        }
    }
    
}