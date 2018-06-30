using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL;
using CarePoint.Models;
using DAL;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Spatial;
using System.IO;
using Extensions;
using System.Diagnostics;
using CarePoint.AuthorizeAttributes;
using System.Web;

namespace CarePoint.Controllers
{
    [Authorize]
    public class MedicalPlaceController : Controller
    {
        // GET: MedicalPlace
        private MedicalPlaceBusinessLayer _medicalPlaceBusinessLayer;
        private CareUnitBusinessLayer _careUnitBusinessLayer;
        private ServiceBusinessLayer _serviceBusinessLayer;

        public MedicalPlaceController()
        {
                
        }

        public MedicalPlaceController(MedicalPlaceBusinessLayer medicalPlaceBusinessLayer, CareUnitBusinessLayer careUnitBusinessLayer,ServiceBusinessLayer serviceBusinessLayer)
        {
            _medicalPlaceBusinessLayer = medicalPlaceBusinessLayer;
            _careUnitBusinessLayer = careUnitBusinessLayer;
            _serviceBusinessLayer = serviceBusinessLayer;
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

        public ServiceBusinessLayer ServiceBusinessLayer
        {
            get
            {
                return _serviceBusinessLayer ?? new ServiceBusinessLayer();
            }
            private set
            {
                _serviceBusinessLayer = value;
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
                MedicalPlaceID = medicalPlace.ID,
                Services = new List<ServiceViewModel>(),
                CareUnits = new List<CareUnitViewModel>(),
                ServiceCategories = ServiceBusinessLayer.GetServiceCategories(),
                CareUnitTypes = CareUnitBusinessLayer.GetCareUnitTypes(),
                IsAdmin = medicalPlace.Admins.Select(m => m.Id).Contains(User.Identity.GetUserId<long>()),
                MedicalPlace = medicalPlace,
                IsCurrentPlace=Request.Cookies["placeInfo"]!=null &&Convert.ToInt64(Request.Cookies["placeInfo"].Values["id"])==id?true : false,
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

        [AccessDeniedAuthorize(Roles = "Doctor")]
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
                    ServiceBusinessLayer.AddWorkSlot(slot);

                }
            }
        }

        [AccessDeniedAuthorize(Roles = "Doctor")]
        public void RemoveWorkslot(long ServiceID,TimeSpan StartTime,TimeSpan EndTime)
        {
            ServiceBusinessLayer.RemoveWorkslot(ServiceID, StartTime, EndTime);
        }

        [HttpPost]
        [AccessDeniedAuthorize(Roles = "Doctor")]
        public ActionResult AddService(MedicalPlaceProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                Service service = new Service()
                {
                    Name = model.NewService.Name,
                    CategoryID = model.NewService.CategoryID,
                    Cost = model.NewService.Cost,
                    Description = model.NewService.Description,
                    ProviderID = model.NewService.ProviderID
                };
                ServiceBusinessLayer.AddService(service);
            }
            return ProfilePage(model.Services.First().ProviderID);
        }

        [HttpPost]
        [AccessDeniedAuthorize(Roles = "Doctor")]
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
                ServiceBusinessLayer.UpdateService(service);
            }
            return ProfilePage(model.Services.First().ProviderID);
        }
        [HttpPost]
        [AccessDeniedAuthorize(Roles = "Doctor")]
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
        [AccessDeniedAuthorize(Roles = "Doctor")]
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

        [AccessDeniedAuthorize(Roles = "Doctor")]
        public ActionResult MedicalPlace()
        {
            MedicalPlaceViewModels model = new MedicalPlaceViewModels();
            ICollection<MedicalPlaceType> medicalPlaceTypes = MedicalPlaceBusinessLayer.GetAllTypes();
            List<SelectListItem> dropDownList = new List<SelectListItem>();
            foreach (MedicalPlaceType medicalType in medicalPlaceTypes)
            {
                dropDownList.Add(new SelectListItem { Text = medicalType.Name, Value = medicalType.ID.ToString() });
            }
            model.MedicalPlaceTypes = dropDownList;
            return View("AddMedicalPlace", model);
        }

        [AccessDeniedAuthorize(Roles = "Doctor")]
        public ActionResult AddMedicalPlace(MedicalPlaceViewModels model)
        {
            DAL.MedicalPlace newPlace = new DAL.MedicalPlace();
            newPlace.Address = model.MedicalPlace.Address;
            newPlace.Description = model.MedicalPlace.Description;
            newPlace.Name = model.MedicalPlace.Name;
            newPlace.IsConfirmed = false;
            newPlace.OwnerID = User.Identity.GetCitizen().Id;
            newPlace.Phone = model.MedicalPlace.Phone;
            newPlace.TypeID = model.MedicalPlace.TypeID;
            double latitude = model.Latitude;
            double longitude = model.Longitude;
            var pointString = string.Format("POINT({0} {1})", longitude.ToString(), latitude.ToString());
            var point = DbGeography.FromText(pointString);
            newPlace.Location = point;
            using (var binaryReader = new BinaryReader(model.MedicalPlace.Photo.InputStream))
            {
                newPlace.Photo = binaryReader.ReadBytes(model.MedicalPlace.Photo.ContentLength);
            }
            using (var binaryReader = new BinaryReader(model.MedicalPlace.Permission.InputStream))
            {
                newPlace.Permission = binaryReader.ReadBytes(model.MedicalPlace.Permission.ContentLength);
            }
            MedicalPlaceBusinessLayer.AddMedicalPlace(newPlace);
            return ProfilePage(newPlace.ID);
        }

        public JsonResult SearchPlace(SearchPlaceViewModel model)
        {
            if (model.ServiceType == null)
                model.ServiceType = "";
            if (model.PlaceName == null)
                model.PlaceName = "";
            Citizen user = User.Identity.GetCitizen();
            List<MedicalPlace> medicalPlaces = new List<MedicalPlace>();
            if (model.ServiceType.ToUpper().Equals("ICU") || model.PlaceName.ToUpper().Equals("ICU"))
            {
                medicalPlaces = MedicalPlaceBusinessLayer.SearchCareUnitsPlace(model.Latitude, model.Longitude, model.ServiceType, model.PlaceName, model.IsDistance, model.IsCost, model.IsRate, model.IsPopularity).ToList();
            }
            else
            {
                medicalPlaces = MedicalPlaceBusinessLayer.SearchMedicalPlace(model.Latitude, model.Longitude, model.ServiceType, model.PlaceName, model.IsDistance, model.IsCost, model.IsRate, model.IsPopularity).ToList();
            }

            var result = medicalPlaces.Select(place => new { place.ID ,placeType=place.MedicalPlaceType.Name, place.Name , place.Address,
                place.Phone,Photo = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(place.Photo))}).ToList();
            return Json(new { places = result });
        }
        public JsonResult SwitchPlace(long id,string type,string url)
        {
            HttpCookie cookie = Request.Cookies["placeInfo"];
            if (cookie == null)
            {
                cookie = new HttpCookie("placeInfo");
            }
            cookie.Values["id"] = id.ToString();
            cookie.Values["type"] = type;
            cookie.Values["url"] = url;
            cookie.Expires = DateTime.UtcNow.AddDays(2);
            Response.Cookies.Add(cookie);
            return Json(true);
        }
        public JsonResult GetCurrentWorkPlace()
        {
            var url = new object();
            var id = new object();
            var type=new object(); 
            var cookie = Request.Cookies["placeInfo"];
            if (cookie == null)
                url = "#";
            else
            {
                id = cookie.Values["id"];
                url = cookie.Values["url"];
                type = cookie.Values["type"];
            }
            return Json(new { id,type, url });
        }
        private List<SlotViewModel> SplitSlots(long serviceId, string dayName)
        {
            // first Get WorkSlots
            List<WorkSlot> workSlots = (ServiceBusinessLayer.GetWorkSlots(serviceId, dayName)).ToList();
            // Add Slot to Slots
            List<SlotViewModel> slots = new List<SlotViewModel>();
            SlotViewModel slot = new SlotViewModel();
            if(workSlots.Count()>0)
            {
                if (workSlots[0].StartTime.Value.Hours != 12)
                {
                    slot = new SlotViewModel();
                    slot.Type = "free-slot";
                    slot.Time = "";
                    slot.Description = "";
                    slot.Duration = 0;
                    slot.EndTime = workSlots[0].StartTime.Value;
                    if (workSlots[0].StartTime.Value.Hours < 12)
                    {
                        slot.StartTime = new TimeSpan(00, 00, 00);
                    }
                    else
                    {
                        slot.StartTime = new TimeSpan(12, 00, 00);
                    }
                    slots.Add(slot);
                }
                slot = new SlotViewModel()
                {
                    Type = "work-slot",
                    Time = "",
                    Description = "",
                    StartTime = workSlots[0].StartTime.Value,
                    EndTime = workSlots[0].EndTime.Value,
                    Duration = 0
                };
                slots.Add(slot);
            }
            for (int i = 1; i < workSlots.Count(); i++)
            {
                // Work Slot
                slot = new SlotViewModel()
                {
                    Type = "work-slot",
                    Time = "",
                    Description = "",
                    StartTime = workSlots[i].StartTime.Value,
                    EndTime = workSlots[i].EndTime.Value,
                    Duration = 0
                };
                slots.Add(slot);
                // check if There is time so it will be Free Slot
                if ((workSlots[i - 1].EndTime.Value.Hours - workSlots[i].StartTime.Value.Hours != 0) ||
                   (workSlots[i - 1].EndTime.Value.Minutes - workSlots[i].StartTime.Value.Minutes) != 0)
                {
                    slot = new SlotViewModel()
                    {
                        Type = "free-slot",
                        Time = "",
                        Description = "",
                        StartTime = workSlots[i - 1].EndTime.Value,
                        EndTime = workSlots[i].StartTime.Value,
                        Duration = 0
                    };
                    slots.Add(slot);
                }
            }
            return slots;
        }
        private dynamic GetServiceSlots(long serviceId,string dayName)
        {
            List<SlotViewModel> slots = SplitSlots(serviceId, dayName);
            SlotViewModel slot = new SlotViewModel();
            int count = slots.Count();
            for(int i=0;i<count;i++)
            {
                if (slots[i].StartTime.Hours <12 && slots[i].EndTime.Hours <=12)
                {
                    slots[i].Time = "AM";
                    if(slots[i].EndTime.Hours>=12 && slots[i].EndTime.Minutes>0)
                    {
                        slot = new SlotViewModel()
                        {
                            Type = "PM",
                            StartTime = new TimeSpan(12, 00, 00),
                            EndTime = slots[i].EndTime
                        };
                        slots[i].EndTime = new TimeSpan(12, 00, 00);
                        slots.Add(slot);
                    }
                }
                else if(slots[i].StartTime.Hours >=12 && slots[i].EndTime.Hours >= 12)
                {
                    slots[i].Time = "PM";
                    slots[i].StartTime = new TimeSpan(slots[i].StartTime.Hours - 12, slots[i].StartTime.Minutes, 00);
                    slots[i].EndTime = new TimeSpan(slots[i].EndTime.Hours - 12, slots[i].EndTime.Minutes, 00);
                }
                else if(slots[i].StartTime.Hours>=12 && slots[i].EndTime.Hours<12)
                {
                    slots[i].Time = "PM";
                    slots[i].StartTime = new TimeSpan(24-slots[i].StartTime.Hours, slots[i].StartTime.Minutes,00);
                    slot = new SlotViewModel()
                    {
                        Time = "AM",
                        Type=slots[i].Type,
                        StartTime = new TimeSpan(00, 00, 00),
                        EndTime=slots[i].EndTime
                    };
                    slots[i].EndTime= new TimeSpan(24, 00, 00);
                    slots.Add(slot);
                }
                else if(slots[i].StartTime.Hours<12&&slots[i].EndTime.Hours>=12)
                {
                    slots[i].Time = "AM";
                    slot = new SlotViewModel()
                    {
                        Time = "PM",
                        Type = slots[i].Type,
                        StartTime = new TimeSpan(00, 00, 00),
                        EndTime = new TimeSpan(slots[i].EndTime.Hours-12, slots[i].EndTime.Minutes, 00)
                    };
                    slots[i].EndTime = new TimeSpan(12, 00, 00);
                    slots.Add(slot);
                }
            }
            slots.OrderBy(s => s.StartTime);
            for(int i=0;i<slots.Count();i++)
            {
                int endHours, endMinuits, startHours;
                endHours = (slots[i].EndTime.Hours == 0) ? 24 : slots[i].EndTime.Hours;
                endMinuits = slots[i].EndTime.Minutes;
                startHours= (slots[i].StartTime.Hours == 0) ? 12 : slots[i].StartTime.Hours;
                string isWorking = (slots[i].Type == "work-slot") ? "Working from " : "Off from ";
                slots[i].Description = isWorking + startHours + ":" + slots[i].StartTime.Minutes + " to " + endHours + ":" + endMinuits;
                slots[i].Duration = (endHours - slots[i].StartTime.Hours) * 60 + (endMinuits - slots[i].StartTime.Minutes);
            }
            var AM = slots.Where(x => x.Time.Equals("AM")).Select(t => new { type = t.Type, durationInMinutes = t.Duration, description = t.Description });
            var PM = slots.Where(x => x.Time.Equals("PM")).Select(t => new { type = t.Type, durationInMinutes = t.Duration, description = t.Description });
            dynamic res = new { ID=serviceId , AM , PM };
            return res;
        }
        [HttpPost]
        public JsonResult GetServicesSlots(List<ServiceDayViewModel> services)
        {
            List<dynamic> result = new List<dynamic>();
            if (services != null)
            {
                for (int i = 0; i < services.Count(); i++)
                {
                    result.Add(GetServiceSlots(services[i].ID, services[i].Day));
                }
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult PlaceReminder(long minuits,long placeId,string placeName)
        {
            HttpCookie cookie = Request.Cookies["placeReminder"];
            if (cookie == null)
            {
                cookie = new HttpCookie("placeReminder");
            }
            cookie.Values["placeId"] = placeId.ToString();
            cookie.Values["placeName"] = placeName;
            cookie.Expires = DateTime.Now.AddMinutes(minuits);
            Response.Cookies.Add(cookie);
            return Json(new
            {
                expired = false,
                placeName = cookie.Values["placeName"],
                placeId = cookie.Values["placeId"],
                year = cookie.Expires.Year,
                month = cookie.Expires.Month,
                day = cookie.Expires.Day,
                hours = cookie.Expires.Hour,
                minuits = cookie.Expires.Minute,
                seconds = cookie.Expires.Second,
             });
        }
        [HttpPost]
        public JsonResult GetExpirationDate()
        {
            Debug.WriteLine("Called");
            var cookie = Request.Cookies["placeReminder"];
            if(cookie!=null)
            {
                return Json(new
                {
                    expired = false,
                    placeName=cookie.Values["placeName"],
                    placeId=cookie.Values["placeId"],
                    year = cookie.Expires.Year,
                    month = cookie.Expires.Month,
                    day = cookie.Expires.Day,
                    hours = cookie.Expires.Hour,
                    minuits = cookie.Expires.Minute,
                    seconds = cookie.Expires.Second
                });
            }
            else
            {
                return Json(new
                {
                    expired = true,
                });
            }
        }

    }   
}