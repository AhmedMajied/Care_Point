using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using BLL;
using System.IO;

namespace CarePoint.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult patientAttachments(long citizenID)
        {
            CitizenBusinessLayer businessLayer = new CitizenBusinessLayer();
            ICollection<Attachment> patientAttachments = businessLayer.getPatientAttachments(citizenID);
            
            return View(patientAttachments);
        }

        [HttpPost]
        public string uploadFile(HttpPostedFileBase file)
        {

            if (file != null && file.ContentLength > 0)
                try
                {
                    // save file on server
                    string path = Path.Combine(Server.MapPath("~/Content/files"), file.FileName);
                    file.SaveAs(path);

                    Attachment attachment = new Attachment {
                        fileName = file.FileName,
                        filePath = ("~/Content/files/") + file.FileName

                        // TODO you should assign rest of attributes 
                    };

                    return "succeed";
                }
                catch (Exception ex)
                {
                    return "ERROR:" + ex.Message.ToString();
                }
            else
            {
                return "You have not specified a file.";
            }
        }

        public FileResult showFile(String path, String fileName)
        {
            String mimeType = System.Web.MimeMapping.GetMimeMapping(path);

            return new FilePathResult(path, mimeType);
        }


    }
}