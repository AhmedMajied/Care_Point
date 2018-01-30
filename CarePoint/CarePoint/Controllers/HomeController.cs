using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using BLL;

namespace CarePoint.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var db = new DAL.CarePointEntities();
            //var ct = db.Citizens.FirstOrDefault(c => c.Id == 1);
            //FileInfo fileInfo = new FileInfo("F:\\1432320.jpg");

            //// The byte[] to save the data in
            //byte[] data = new byte[fileInfo.Length];

            //// Load a filestream and put its content into the byte[]
            //using (FileStream fs = fileInfo.OpenRead())
            //{
            //    fs.Read(data, 0, data.Length);
            //    ct.Photo = data;
            //    db.Citizens.Attach(ct);
            //    db.Entry(ct).State = System.Data.Entity.EntityState.Modified;
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

        public FileResult showFile(String path, String fileName)
        {
            String mimeType = MimeMapping.GetMimeMapping(path);

            return new FilePathResult(path, mimeType);
        }


    }
}