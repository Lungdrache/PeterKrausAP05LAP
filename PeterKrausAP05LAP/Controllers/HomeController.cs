using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

            ViewBag.Result = "Everything was Importet";



            return View();
        }
    }
}