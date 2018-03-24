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
using Microsoft.AspNet.Identity;
using System.Drawing;
using System.Drawing.Imaging;

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
        public ActionResult UploadAttachments(HttpPostedFileBase[] files, FormCollection form)
        {
            string[] typeIDs = form.GetValues("attachmentTypes");

            for(int i=0;i<files.Length;i++)
            {
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Attachments"),
                    files[i].FileName);
                    files[i].SaveAs(path);

                    Attachment attachment = new Attachment
                    {
                        TypeID = Convert.ToInt64(typeIDs[i]),
                        Date = DateTime.Now,
                        SpecialistID = 15,// User.Identity.GetUserId<long>() 
                        CitizenID = Convert.ToInt64(form["Id"]),
                        FilePath = path,
                        FileName = files[i].FileName
                    };

                    _medicalHistorBusinessLayer.SaveAttachment(attachment);
                }
                catch (Exception ex)
                {
                    //return "ERROR:" + ex.Message.ToString();
                }
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult UploadPrescription(FormCollection form)
        {
            string prescriptionFilePath = "~/Attachments/Prescriptions/" +
                                     Path.GetRandomFileName().Replace(".", "") + ".jpg";
            List<List<string>> medicinesAlternatives = new List<List<string>>();

            string[] symptoms = form.GetValues("symptomName");
            string[] diseases = form.GetValues("diseaseName");
            string[] medicines = form.GetValues("drugName");
            string[] dosesDescription = form.GetValues("dose");
            string remarks = form["remarks"];
            
            HistoryRecord historyRecord = new HistoryRecord
            {
                Date = DateTime.Now,
                Remarks = remarks,
                MedicalPlaceID = 4, //TODO get from session
                CitizenID = Convert.ToInt64(form["Id"]),
                SpecialistID = 15//User.Identity.GetUserId<long>()
            };

            // assign symptoms to history record
            for (int i = 0; i < symptoms.Length; i++)
            {
                if (!symptoms[i].Equals("") && !symptoms[i].Equals(" "))
                {
                    historyRecord.Symptoms.Add(new Symptom { Name = symptoms[i] });
                }
            }

            // assign diseases to history record
            for (int i = 0; i < diseases.Length; i++)
            {
                if (!diseases[i].Equals("") && !diseases[i].Equals(" "))
                {
                    historyRecord.Diseases.Add(new Disease { Name = diseases[i] });
                }
            }

            // get selected medicines alternatives from form
            for (int i = 0; i < medicines.Length; i++)
            {
                if (form.GetValues("medicineAlternativeFor" + i) != null)
                {
                    medicinesAlternatives.Add(form.GetValues("medicineAlternativeFor" + i).ToList());
                }
            }

            // save history record to database
            Bitmap bitmap = _medicalHistorBusinessLayer.SavePrescription(historyRecord,
                medicines, dosesDescription, medicinesAlternatives, prescriptionFilePath);

            if(bitmap != null)
                bitmap.Save(Server.MapPath(prescriptionFilePath), ImageFormat.Jpeg);

            //           if (medicines[0].Equals(""))
            return Redirect(Request.UrlReferrer.ToString());

            /*         return new FilePathResult(prescriptionFilePath, "image/jpg")
                     {
                         FileDownloadName = historyRecord.Date.ToString() + ".jpg"
                     };*/

        }

        public ActionResult GetAttachmentTypes()
        {
            var attachmentTypes = _medicalHistorBusinessLayer.GetAttachmentTypes().
                Select(type => new { type.ID,type.Name }).ToList();

            return Json(attachmentTypes);
        }

    }
}