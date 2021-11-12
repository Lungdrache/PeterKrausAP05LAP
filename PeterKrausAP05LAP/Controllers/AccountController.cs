using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PeterKrausAP05LAP.Models;
using PeterKrausAP05LAP.Tools;

namespace PeterKrausAP05LAP.Controllers
{
    public class AccountController : Controller
    {
        public static StockGamesDatabaseEntities context = new StockGamesDatabaseEntities();


        #region Login

        [HttpGet]
        [ActionName("Login")]
        public ActionResult LoginPageGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("Login")]
        public ActionResult LoginPagePost(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {


            Customer customer = context.Customer.Where(x => x.Email == email).FirstOrDefault();

            var hashedPassword = SecureManager.GenerateHash(password, customer.Salt);

                if (customer.PWHash == hashedPassword.hash)
                {
                    List<ToastMessage> toastMessages = new List<ToastMessage> {
                    new ToastMessage(
                        "WIllkommen zurück " + email + "!",
                        "Sie wurden erfolgreich eingeloggt",
                        Toasttype.success)
                };
                    ViewBag.toasts = toastMessages;
                }
                else
                {
                    List<ToastMessage> toastMessages = new List<ToastMessage> {
                    new ToastMessage(
                        "Fehler",
                        "Email oder Passwort ist falsch",
                        Toasttype.error)
                };
                    ViewBag.toasts = toastMessages;

                }

                if (customer.PWHash == hashedPassword.hash)
                {
                    AuthenticateUser(email);
                    TempData["justLoggedIn"] = true;
                    TempData["userEmail"] = customer.Email;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                List<ToastMessage> toastMessages = new List<ToastMessage> {
                    new ToastMessage(
                        "Fehler",
                        "Email oder Passwort ist falsch",
                        Toasttype.error)
                };
                ViewBag.toasts = toastMessages;
                return View();
            }

        }
        #endregion

        #region Logout

        [HttpGet]
        [ActionName("Logout")]
        public ActionResult GetLogout()
        {
            if (User.Identity.IsAuthenticated)
            {
                DeAuthenticateUser();
                TempData["justLoggedOut"] = true;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion

        #region Registratur

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
        #endregion

        #region Authentication

        [NonAction]
        public void AuthenticateUser(string email)
        {

            string name = email;
            DateTime now = DateTime.Now;
            bool rememberMe = true;
            string userData = "";



            var ticket = new FormsAuthenticationTicket(
                0,
                name,
                now,
                now.AddMinutes(30),
                rememberMe,
                userData
                );

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(
                ".ASPXAUTH",
                encryptedTicket);

            Response.Cookies.Add(cookie);

        }
        [NonAction]
        public void DeAuthenticateUser()
        {
            if (Request.Cookies[".ASPXAUTH"] != null)
            {
                Response.Cookies[".ASPXAUTH"].Expires = DateTime.Now.AddDays(-1);
            }
        }
        #endregion

    }
}
