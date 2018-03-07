using BLL;
using CarePoint.Models;
using Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;

namespace CarePoint.Controllers
{

    public class MedicalHistoryController : Controller
    {
        private MedicalHistoryBusinessLayer _medicalHistorBusinessLayer;

        private CitizenBusinessLayer _citizenBusinessLayer;
        public MedicalHistoryController()
        {
            _medicalHistorBusinessLayer = new MedicalHistoryBusinessLayer();
        }
        public MedicalHistoryController(CitizenBusinessLayer citizenBusinessLayer)
        {
            _citizenBusinessLayer = citizenBusinessLayer;
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

        //GET: MedicalHistory
        public ActionResult MedicalHistory(long id)
        {
            var user = User.Identity.GetCitizen();
            if (user is Models.Specialist || id == user.Id)
            {
                return View(citizenBusinessLayer.GetCitizen(id).HistoryRecords);
            }
            else
            {
                return new HttpUnauthorizedResult();
            }
        }

        // GET: Attachments
        public ActionResult Attachments(long id)
        {
            var user = User.Identity.GetCitizen();
            if (user is Models.Specialist || id == user.Id)
            {
                return View(citizenBusinessLayer.GetCitizen(id).Attachments);
            }
            else
            {
                return new HttpUnauthorizedResult();
            }
        }

        public FileResult ShowAttachmentFile(string path, string fileName)
        {
            String mimeType = MimeMapping.GetMimeMapping(path);

            return new FilePathResult(path, mimeType);
        }

        [HttpPost]
        public ActionResult UploadAttachments(HttpPostedFileBase[] files)
        {
            foreach (HttpPostedFileBase file in files)
            {
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Attachments"),
                    file.FileName);
                    file.SaveAs(path);
                    
                    /*Attachment a = new Attachment
                    {
                        TypeID = 1,
                        Date = DateTime.Now,
                        SpecialistID = 1,
                        CitizenID = 1,
                        FilePath = path,
                        FileName = file.FileName
                    };
                    */
                    //TODO save to DB here  
                }
                catch (Exception ex)
                {
                    //return "ERROR:" + ex.Message.ToString();
                }
                
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public string UploadPrescription(FormCollection form)
        {
            List<List<string>> medicinesAlternatives = new List<List<string>>();

            


            string[] symptoms = form.GetValues("symptomName");
            string[] diseases = form.GetValues("diseaseName");
            string[] medicines = form.GetValues("medicineName");
            string[] doses = form.GetValues("dose");
            string remarks = form["remarks"];

            // bind selected medicines alternatives
            for(int i = 0; i < medicines.Length; i++)
            {
                medicinesAlternatives.Add(form.GetValues("medicineAlternativeFor" + i).ToList());
            }

            HistoryRecord historyRecord = new HistoryRecord
            {
                Date = DateTime.Now,
                Remarks = remarks,
                MedicalPlaceID = 1, //TODO get from session
                CitizenID = 1, //TODO get from view (@hidden)
                SpecialistID = 1,//TODO get from session
            };

            return symptoms[0] +symptoms[1]+diseases[0]+diseases[1]+medicines[0]+medicines[1]
                +doses[0]+doses[1]+medicinesAlternatives[0][0]+medicinesAlternatives[1][1];
            //return Redirect(Request.UrlReferrer.ToString());
        }

    }
}