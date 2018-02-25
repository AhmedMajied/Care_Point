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

        public FileResult ShowAttachmentFile(String path, String fileName)
        {
            String mimeType = MimeMapping.GetMimeMapping(path);

            return new FilePathResult(path, mimeType);
        }


    }
}