﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using BLL;
using System.Data.Entity.Spatial;

namespace CarePoint.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var db = new DAL.CarePointEntities();
            //var ct = db.Citizens.FirstOrDefault(c => c.Id == 1);
            //FileInfo fileInfo = new FileInfo("F:\\1432320.jpg");
            //var mp = new MedicalPlace()
            //{
            //    Name = "Nile Hospital",
            //    Description = "hospital",
            //    Location = DbGeography.FromText("POINT(0 0)"),
            //    TypeID = 1,
            //    OwnerID = 15
            //};
            //// The byte[] to save the data in
            //byte[] data = new byte[fileInfo.Length];

            //// Load a filestream and put its content into the byte[]
            //using (FileStream fs = fileInfo.OpenRead())
            //{
            //    fs.Read(data, 0, data.Length);
            //    mp.Photo = data;
            //    mp.Permission = data;
            //    db.MedicalPlaces.Add(mp);
            //    db.SaveChanges();
            //}

            //// Delete the temporary file
            ////fileInfo.Delete();

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
            ICollection<Attachment> patientAttachments = businessLayer.GetPatientAttachments(citizenID);
            
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