using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using PeterKrausAP05LAP.Models;
using PeterKrausAP05LAP.Tools;

namespace PeterKrausAP05LAP.Controllers
{
    public class AccountController : Controller
    {
        public static StockGamesDatabaseEntities context = new StockGamesDatabaseEntities();

        [HttpGet]
        [ActionName("Login")]
        public ActionResult LoginPageGet()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Login")]
        public ActionResult LoginPagePost(string email, string password)
        {
            

            return View();
        }

        [HttpGet]
        [ActionName("Registratur")]
        public ActionResult RegistraturGet(bool wantToLogIn = true)
        {

            // wenn wantToLogIn false ist dann wird dem User
            // nur angezeigt das er sich Eingeloggt hat
            return View(wantToLogIn);
        }
        [HttpPost]
        [ActionName("Registratur")]
        public ActionResult RegistraturPost(string title,string adress, string fname, string lname, string email, string password, int PLZ, string Ort)
        {
            Customer newCustomer = new Customer(title,fname,lname,email,adress,PLZ.ToString(),Ort,password);

            // mail Sending doesn't work atm
            // MailManager.SendEmail($"<br/><br/>Hello {fname} {lname}<br/><br/> You succefully registered to our Website!", $"Welcome {lname} to Stockgames",email);

            TempData["Registratur"] = "";

            if (!context.Customer.Any(x => x.Email == email))
            {
                context.Customer.Add(newCustomer);
                context.SaveChanges();
                List<ToastMessage> toastMessages = new List<ToastMessage> {
                    new ToastMessage(
                        "Erledigt",
                        "Benutzer wurde erstellt",
                        Toasttype.success)
                };
                ViewBag.toasts = toastMessages;
                return View(false);
            }
            else
            {
                List<ToastMessage> toastMessages = new List<ToastMessage> { 
                    new ToastMessage(
                        "Fehler bei der Registrierung", 
                        email + " besitzt schon einen Account", 
                        Toasttype.error)
                };
                ViewBag.toasts = toastMessages;
                return View(true);
            }


        }

        // GET: Account/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Account/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Account/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Account/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
