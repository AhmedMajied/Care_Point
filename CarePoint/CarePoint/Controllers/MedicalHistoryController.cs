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
using CarePoint.AuthorizeAttributes;

namespace CarePoint.Controllers
{
    [Authorize]
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
        
        public FileResult DownloadAttachment(string path, string fileName)
        {
            String mimeType = MimeMapping.GetMimeMapping(path);

            return new FilePathResult(path, mimeType)
            {
                FileDownloadName = fileName 
            };
        }

        [HttpPost]
        [AccessDeniedAuthorize(Roles = "Doctor")]
        public ActionResult UploadAttachments(HttpPostedFileBase[] files, FormCollection form)
        {
            string[] typeIDs = form.GetValues("attachmentTypes");
            string fileExtension;

            for(int i=0;i<files.Length;i++)
            {
                try
                {
                    fileExtension = Path.GetExtension(files[i].FileName);

                    if (fileExtension != ".jpg" && fileExtension !=".pdf" && fileExtension != ".png")
                        throw new Exception("invalid extension");

                    string path = Path.Combine(Server.MapPath("~/Attachments"),
                        Path.GetRandomFileName().Replace(".", "")+Path.GetExtension(files[i].FileName));
                    files[i].SaveAs(path);

                    Attachment attachment = new Attachment
                    {
                        TypeID = Convert.ToInt64(typeIDs[i]),
                        Date = DateTime.Now,
                        SpecialistID = User.Identity.GetUserId<long>(),
                        CitizenID = Convert.ToInt64(form["Id"]),
                        FilePath = path,
                        FileName = files[i].FileName,
                        IsRead = false
                    };

                    MedicalHistoryBusinessLayer.SaveAttachment(attachment);
                    NotificationsHub.NotifyAttachment(attachment.CitizenID, User.Identity.GetCitizen().Name,attachment.FileName);
                }
                catch (Exception ex)
                {
                    //return "ERROR:" + ex.Message.ToString();
                }
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [AccessDeniedAuthorize(Roles = "Doctor")]
        public string UploadPrescription(FormCollection form)
        {
            string prescriptionFilePath = "~/Attachments/Prescriptions/" +
                                     Path.GetRandomFileName().Replace(".", "") + ".jpg";
            List<List<string>> medicinesAlternatives = new List<List<string>>();

            string[] symptoms = form.GetValues("symptomName");
            string[] diseases = form.GetValues("diseaseName");
            string[] genticDiseases = form.GetValues("genetic");
            string[] medicines = form.GetValues("drugName");
            string[] doses = form.GetValues("dose");
            string remarks = form["remarks"];
            
            HistoryRecord historyRecord = new HistoryRecord
            {
                Date = DateTime.Now,
                Remarks = remarks,
                MedicalPlaceID = 6, //TODO get from session
                CitizenID = Convert.ToInt64(form["Id"]),
                SpecialistID = User.Identity.GetUserId<long>()
            };

            // assign symptoms to history record
            for (int i = 0; i < symptoms.Length; i++)
            {
                symptoms[i] = symptoms[i].Trim();
                if (!symptoms[i].Equals(""))
                {
                    historyRecord.Symptoms.Add(new Symptom { Name = symptoms[i] });
                }
            }

            // assign diseases to history record
            for (int i = 0; i < diseases.Length; i++)
            {
                diseases[i] = diseases[i].Trim();
                if (!diseases[i].Equals(""))
                {
                    historyRecord.Diseases.Add(new Disease { Name = diseases[i], IsGenetic = false });
                }
            }
            
            // assign genetic diseases
            List<Disease> historyRecordDiseases = historyRecord.Diseases.ToList();
            for (int i = 0; genticDiseases != null && i < Math.Min(genticDiseases.Length,diseases.Length) ; i++)
            {
                if(diseases[Convert.ToInt32(genticDiseases[i]) - 1] == "")
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
                medicines, doses, medicinesAlternatives, prescriptionFilePath,(userId,diseaseName) => NotificationsHub.NotifyPrognosis(userId,diseaseName) );

            // don't save prescription if null
            if (bitmap != null)
                bitmap.Save(Server.MapPath(prescriptionFilePath), ImageFormat.Jpeg);
            else
                return null;
            
            return Path.GetFileName(prescriptionFilePath);
        }

        public ActionResult DownloadPrescription(string fileName)
        {
            return new FilePathResult("~/Attachments/Prescriptions/"+fileName, "image/jpg")
            {
                FileDownloadName = fileName
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