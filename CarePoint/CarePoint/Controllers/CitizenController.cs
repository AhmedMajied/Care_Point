using BLL;
using DAL;
using Extensions;
using System;
using System.Collections.Generic;
using MessagingToolkit.QRCode.Codec;
using System.Linq;
using System.Web.Mvc;
using CarePoint.Models;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;
using CarePoint.Hubs;
using CarePoint.AuthorizeAttributes;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace CarePoint.Controllers
{
    [Authorize]
    public class CitizenController : Controller
    {
        private CitizenBusinessLayer _citizenBusinessLayer;

        public CitizenController()
        {
            _citizenBusinessLayer = new CitizenBusinessLayer();
        }

        public CitizenBusinessLayer CitizenBusinessLayer
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

        public FileResult DownloadMyCard()
        {
            PatientCardCanvas canvas = new PatientCardCanvas();
            string nationalID = User.Identity.GetCitizen().NationalIDNumber;

            // encode national ID
            var textBytes = Encoding.UTF8.GetBytes(nationalID);
            string ecodedText = Convert.ToBase64String(textBytes);

            // convert text to QR code 
            QRCodeEncoder QRencoder = new QRCodeEncoder();
            Bitmap QRCode = QRencoder.Encode(ecodedText);

            // Draw patient card
            Bitmap logo = new Bitmap(Server.MapPath("~/Images/logo.png"));
            Bitmap photo = new Bitmap(Server.MapPath("~/Images/notfound.png"));
            Bitmap patientCard = canvas.Draw(User.Identity.GetCitizen(), QRCode,logo,photo);
            patientCard.Save(Server.MapPath("~/Images/PatientCard.jpg"), ImageFormat.Jpeg);

            return new FilePathResult(Server.MapPath("~/Images/PatientCard.jpg"), "image/jpeg")
            {
                FileDownloadName = "Patient Card.jpg"
            };
        }

        public JavaScriptResult GetCitizenByQR(string citizenQRCode)
        {
            Citizen citizen = CitizenBusinessLayer.GetCitizenByQR(citizenQRCode);

            if (citizen == null || citizen.Id == User.Identity.GetUserId<long>())
                return null;

            return JavaScript("window.location = '/Citizen/CurrentPatient?citizenID="+citizen.Id+"'");
        } 

        [AccessDeniedAuthorize(Roles = "Doctor")]
        public ActionResult CurrentPatient(long citizenID)
        {
            Citizen citizen = CitizenBusinessLayer.GetCitizen(citizenID);
            CurrentPatientViewModel model = new CurrentPatientViewModel()
            {
                Id = citizen.Id,
                Name = citizen.Name,
                Age = DateTime.Now.Year - citizen.DateOfBirth.Value.Year,
                BloodType = citizen.BloodType.Name,
                Gender = citizen.Gender,
                Photo = citizen.Photo,
                HistoryRecords = citizen.HistoryRecords,
                Attachments = citizen.Attachments.OrderBy(a => a.AttachmentType.ID)
                .GroupBy(a => a.AttachmentType)
                .ToDictionary(g => g.Key, g => g.Select(attachment => new AttachmentViewModel()
                {
                    IsRead = attachment.IsRead ?? false,
                    Date = attachment.Date,
                    FileName = attachment.FileName,
                    FilePath = attachment.FilePath,
                    SpecialistName = attachment.Specialist.Name
                }).ToList())
            };
            return View(model);
        }

        public ActionResult MedicalHistory()
        {
            return View(CitizenBusinessLayer.GetCitizen(User.Identity.GetUserId<long>()).HistoryRecords); 
        }

        // GET: Attachments
        public ActionResult Attachments()
        {
            IDictionary<AttachmentType, List<AttachmentViewModel>> model = CitizenBusinessLayer.GetCitizen(User.Identity.GetUserId<long>()).Attachments.OrderBy(a => a.AttachmentType.ID)
                .GroupBy(a => a.AttachmentType)
                .ToDictionary(g => g.Key,g => g.Select( attachment => new AttachmentViewModel() {
                    IsRead=attachment.IsRead??false,
                    Date=attachment.Date,
                    FileName=attachment.FileName,
                    FilePath=attachment.FilePath,
                    SpecialistName=attachment.Specialist.Name
                }).ToList());
            
            return View(model);
        }

        public ActionResult Relatives()
        {
            long citizenId = User.Identity.GetUserId<long>();
            IEnumerable<Relative> relatives = CitizenBusinessLayer.GetRelatives(citizenId);

            ICollection<Relative> newConnections = relatives.Where(r => (r.CitizenID == citizenId && !(r.CitizenConfirmed ?? false)) || (r.RelativeID == citizenId && !(r.RelativeConfirmed ?? false)))
               .ToList();

            ICollection<Relative> friends = relatives.Where(r => r.RelationTypeID == 3).Except(newConnections).ToList();

            ICollection<Relative> family = relatives.Where(r=>r.RelationTypeID != 3).Except(newConnections).ToList();
            RelativesPageViewModel model = new RelativesPageViewModel()
            {
                Friends = friends.Select(r => new RelativeViewModel()
                {
                    FullName = (r.CitizenID == citizenId) ? r.RelativeCitizen.Name : r.Citizen.Name,
                    ID = (r.CitizenID == citizenId) ? r.RelativeID : r.CitizenID,
                    Photo = (r.CitizenID == citizenId) ? r.RelativeCitizen.Photo : r.Citizen.Photo,
                    RelationType = r.RelationType.Name
                }).ToList(),
                Family = family.Select(r => new RelativeViewModel()
                {
                    FullName = (r.CitizenID == citizenId) ? r.RelativeCitizen.Name : r.Citizen.Name,
                    ID = (r.CitizenID == citizenId) ? r.RelativeID : r.CitizenID,
                    Photo = (r.CitizenID == citizenId) ? r.RelativeCitizen.Photo : r.Citizen.Photo,
                    RelationType = (r.RelationTypeID != 1 || r.CitizenID == citizenId) ? r.RelationType.Name : "Scion"
                }).ToList(),
                NewConnections = newConnections.Select(r => new RelativeViewModel()
                {
                    FullName = (r.CitizenID == citizenId) ? r.RelativeCitizen.Name : r.Citizen.Name,
                    ID = (r.CitizenID == citizenId) ? r.RelativeID : r.CitizenID,
                    Photo = (r.CitizenID == citizenId) ? r.RelativeCitizen.Photo : r.Citizen.Photo,
                    RelationType = (r.RelationTypeID != 1 || r.CitizenID == citizenId) ? r.RelationType.Name : "Scion"
                }).ToList()
            };
            CitizenBusinessLayer.ConfirmAllRelatives(citizenId);
            return View(model);
        }

        private List<dynamic> GetSearchResult(IEnumerable<Citizen> queryResult)
        {
            List<dynamic> result = new List<dynamic>();
            foreach(var res in queryResult)
            {
                dynamic obj;
                var relative = res.Relatives.SingleOrDefault(r => r.CitizenID == User.Identity.GetUserId<long>());
                if(relative != null)
                {
                    obj = new { res.Name, res.Id, res.Photo,Relation=relative.RelationType.Name };
                }
                else
                {
                    relative = res.AddedRelatives.SingleOrDefault(r => r.RelativeID == User.Identity.GetUserId<long>());
                    if (relative != null)
                    {
                        if (relative.RelationType.Name == "Parent")
                            obj = new { res.Name, res.Id, res.Photo, Relation = "Scion" };
                        else
                            obj = new { res.Name, res.Id, res.Photo, Relation = relative.RelationType.Name };
                    }
                    else
                    {
                        obj = new { res.Name, res.Id, res.Photo, Relation = "None" };
                    }
                }
                

                result.Add(obj);

            }
            return result;
        }
        public JsonResult SearchAccount(string key, string value)
        {
            long citizenId = User.Identity.GetCitizen().Id;
            List<List<Citizen>> allCitizens = _citizenBusinessLayer.SearchAccounts(citizenId,key,value);
            var citizens = GetSearchResult(allCitizens[0]);
            var doctors = GetSearchResult(allCitizens[1]);
            var pharmacists = GetSearchResult(allCitizens[2]);
            var res = new{citizens, doctors, pharmacists};
            return Json(res);
        }

        [AccessDeniedAuthorize(Roles = "Doctor")]
        public JsonResult PatientsList(long doctorId, long placeId)
        {
            List<Citizen> list = _citizenBusinessLayer.GetPatientList(doctorId,placeId);
            var males = (list.Where(c => c.Gender.ToLower().Equals("male"))).Select(x => new { x.Name, x.Id, x.Photo });
            var females = (list.Where(c => c.Gender.ToLower().Equals("female"))).Select(x => new { x.Name, x.Id, x.Photo });
            var result = new[] { males, females };
            return Json(result);
        }

        public JsonResult AddRelative(long relativeId, string relationType)
        {
            int relationId = (relationType == "Parent") ? 1 : (relationType == "Friend") ? 3 : 4;
            long citizenId = User.Identity.GetUserId<long>();
            try
            {
                CitizenBusinessLayer.AddRelative(citizenId,relativeId,relationId,() => NotificationsHub.NotifyRelative(relativeId, 1, CitizenBusinessLayer.GetCitizen(relativeId).Name, relationType));
                
                return Json(new { Code=0,Message="Added Successfully"});
            }
            catch(Exception e)
            {
                SqlException exception = e.InnerException.InnerException as SqlException;
                return Json(new { Code = exception.Number, Message = exception.Message });
            }
        }

        public void RemoveRelation(long relativeId)
        {
            
            CitizenBusinessLayer.RemoveRelation(User.Identity.GetUserId<long>(), relativeId,() => {
                if (!CitizenBusinessLayer.IsRelationConfirmed(User.Identity.GetUserId<long>(), relativeId))
                {
                    NotificationsHub.NotifyRelative(relativeId, -1);
                }
            });
        }


        public ActionResult Prognosis()
        {
            long id = User.Identity.GetUserId<long>();
            var potintialDiseases = CitizenBusinessLayer.GetPotintialDiseases(id);
            ICollection<PotentialDiseaseViewModel> model = potintialDiseases.GroupBy(p => p.DiseaseID)
                .Select(g => new PotentialDiseaseViewModel {
                    DiseaseName=g.First().Disease.Name,
                    Level = g.Min(p => p.Level),
                    NumberOfCasualties=g.Count(),
                    TimeStamp=g.Max(p => p.TimeStamp),
                    IsRead=!g.Any(p => !p.IsRead)
                }).ToList();
            CitizenBusinessLayer.ReadAllPotentialDiseases(id);
            return View(model);
        }

        [HttpPost]
        public void ReadAttachmentsOfType(int typeId)
        {
            CitizenBusinessLayer.ReadAttachmentsOfType(User.Identity.GetUserId<long>(), typeId);
        }
        [HttpPost]
        public JsonResult GetSpecialistWorkPlaces(long specialistId)
        {
            string speciality = CitizenBusinessLayer.GetSpeciality(specialistId);
            var result=new object();
            var medicalPlaceTypes = new object();
            if(speciality.Equals("Pharmacist"))
            {
                List<Pharmacy> pharmacies = new List<Pharmacy>();
                pharmacies = CitizenBusinessLayer.GetSpecialistPharmacyPlace(specialistId).ToList();
                result = pharmacies.Select(m => new { Type = "Pharmacy", m.ID, m.Name, Photo = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(m.Photo)), url= "/Pharmacy/PharmacyProfile?Id="+m.ID});
                medicalPlaceTypes = new { Type = "Pharmacy" };
            }
            else
            {
                List<MedicalPlace> medicalPlaces = new List<MedicalPlace>();
                medicalPlaces = CitizenBusinessLayer.GetSpecialistWorkPlaces(specialistId).ToList();
                result = medicalPlaces.Select(m => new {Type=m.MedicalPlaceType.Name, m.ID, m.Name , Photo = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(m.Photo)), url = "/MedicalPlace/ProfilePage?id="+m.ID});
                medicalPlaceTypes = medicalPlaces.GroupBy(m=>m.MedicalPlaceType.ID).Select(m => new { Type = m.First().MedicalPlaceType.Name });
            }
            var res = new {result, medicalPlaceTypes};
            return Json(res);
        }

    }
}