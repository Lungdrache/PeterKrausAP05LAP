using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace PeterKrausAP05LAP.Controllers
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

        [HttpGet]
        [ActionName("Importer")]
        public ActionResult ImporterGet()
        {
            ViewBag.Result = "Want to import Files?";

            return View();
        }

        [HttpPost]
        [ActionName("Importer")]
        public ActionResult ImporterPost()
        {
            // gets the FolderPath
            string programmPath = Server.MapPath("~/App_Data");
            // counts the errors which was made
            int errorCount = 0;
            // counts all importet files
            int fileCount = 0;


            // checks if the Import folder even Exists
            if (Directory.Exists(programmPath + "/dataOutput") && Directory.Exists(programmPath + "/AssetFiles"))
            {
                string[] allFiles = Directory.GetFiles(programmPath + "/dataOutput");

                foreach (string filePath in allFiles)
                {

                    if (System.IO.File.Exists(filePath))
                    {
                        fileCount++;
                    }
                    else
                    {
                        errorCount++;
                    }



                }
                ViewBag.Result = "All Work is Done";
                ViewBag.Errors = errorCount;
                ViewBag.Files = fileCount;








            }
            else
            {
                ViewBag.Result = "Can't find the folder \"dataOutput\" and \"AssetFiles\"";
                ViewBag.Errors = 0;
                ViewBag.Files = 0;

            }


            return View();
        }
    }
}