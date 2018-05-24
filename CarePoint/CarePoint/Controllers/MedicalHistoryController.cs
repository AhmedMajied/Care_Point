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
using CarePoint.Hubs;

namespace CarePoint.Controllers
{

    public class MedicalHistoryController : Controller
    {
        private MedicalHistoryBusinessLayer _medicalHistoryBusinessLayer;

        public MedicalHistoryController()
        {
            _medicalHistoryBusinessLayer = new MedicalHistoryBusinessLayer();
        }
        public MedicalHistoryController(MedicalHistoryBusinessLayer medicalHistoryBusinessLayer)
        {
            _medicalHistoryBusinessLayer = medicalHistoryBusinessLayer;
        }
        public MedicalHistoryBusinessLayer MedicalHistoryBusinessLayer
        {
            get
            {
                return _medicalHistoryBusinessLayer ?? new MedicalHistoryBusinessLayer();
            }
            private set
            {
                _medicalHistoryBusinessLayer = value;
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
                        SpecialistID = 26,//User.Identity.GetUserId<long>(),
                        CitizenID = Convert.ToInt64(form["Id"]),
                        FilePath = path,
                        FileName = files[i].FileName,
                        IsRead = false
                    };

                    MedicalHistoryBusinessLayer.SaveAttachment(attachment);
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
            string[] genticDiseases = form.GetValues("genetic");
            string[] medicines = form.GetValues("drugName");
            string[] dosesDescription = form.GetValues("dose");
            string remarks = form["remarks"];
            
            HistoryRecord historyRecord = new HistoryRecord
            {
                Date = DateTime.Now,
                Remarks = remarks,
                MedicalPlaceID = 6, //TODO get from session
                CitizenID = Convert.ToInt64(form["Id"]),
                SpecialistID = 26 //User.Identity.GetUserId<long>()
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
                    historyRecord.Diseases.Add(new Disease { Name = diseases[i], IsGenetic = false });
                }
            }
            
            // assign genetic diseases
            List<Disease> historyRecordDiseases = historyRecord.Diseases.ToList();
            for (int i = 0; genticDiseases != null && i < Math.Min(genticDiseases.Length,diseases.Length) ; i++)
            {
                if(diseases[Convert.ToInt32(genticDiseases[i]) - 1] == "" 
                    || diseases[Convert.ToInt32(genticDiseases[i]) - 1] == " ")
                {
                    continue;
                }

                historyRecordDiseases[Convert.ToInt32(genticDiseases[i])-1].IsGenetic = true;
            }

            // get selected medicines alternatives from form
            for (int i = 0; i < medicines.Length; i++)
            {
                if (form.GetValues("medicineAlternativeFor" + i) == null)
                {
                    medicinesAlternatives.Add(new List<string>());
                    continue;
                }

                medicinesAlternatives.Add(form.GetValues("medicineAlternativeFor" + i).ToList());
            }

            // save history record to database
            Bitmap bitmap = MedicalHistoryBusinessLayer.SavePrescription(historyRecord,
                medicines, dosesDescription, medicinesAlternatives, prescriptionFilePath,(userId,diseaseName) => NotificationsHub.NotifyPrognosis(userId,diseaseName) );

            if(bitmap != null)
                bitmap.Save(Server.MapPath(prescriptionFilePath), ImageFormat.Jpeg);

            if (medicines[0].Equals(""))
                return Redirect(Request.UrlReferrer.ToString());

            return new FilePathResult(prescriptionFilePath, "image/jpg")
            {
                FileDownloadName = historyRecord.Date.ToString() + ".jpg"
            };
        }

        public ActionResult GetAttachmentTypes()
        {
            var attachmentTypes = MedicalHistoryBusinessLayer.GetAttachmentTypes().
                Select(type => new { type.ID,type.Name }).ToList();

            return Json(attachmentTypes);
        }

    }
}