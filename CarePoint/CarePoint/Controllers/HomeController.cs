using System;
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


        [HttpPost]
        public string uploadFile(HttpPostedFileBase[] files)
        {

            foreach (HttpPostedFileBase file in files)
            {
                if (file != null)
                {
                    try
                    {
                        // save file on server
                        string path = Path.Combine(Server.MapPath("~/Content/files"), file.FileName);
                        file.SaveAs(path);

                        Attachment attachment = new Attachment
                        {
                            fileName = file.FileName,
                            filePath = ("~/Content/files/") + file.FileName

                            // TODO you should assign rest of attributes 
                        };


                    }
                    catch (Exception ex)
                    {
                        return "ERROR:" + ex.Message.ToString();
                    }
                }        
            }
            
            return "succeed";
        }

        public FileResult ShowAttachmentFile(String path, String fileName)
        {
            String mimeType = MimeMapping.GetMimeMapping(path);

            return new FilePathResult(path, mimeType);
        }


    }
}