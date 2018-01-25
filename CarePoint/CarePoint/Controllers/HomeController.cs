using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}