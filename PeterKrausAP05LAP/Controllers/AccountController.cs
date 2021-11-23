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
            if (User.Identity.IsAuthenticated && (bool?)Session["Guest"] != true)
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
                if (customer == null)
                {
                    List<ToastMessage> toastMessages = new List<ToastMessage> {
                    new ToastMessage(
                        "Fehler",
                        "User ist nicht registriert",
                        Toasttype.error)
                };
                    ViewBag.toasts = toastMessages;
                    return View();
                }
                else
                {

                var hashedPassword = SecureManager.GenerateHash(password, customer.Salt);

                if (customer.PWHash == hashedPassword.hash)
                    {
                        List<ToastMessage> toastMessages = new List<ToastMessage> {
                    new ToastMessage(
                        "WIllkommen zurück " + email + "!",
                        "Sie wurden erfolgreich eingeloggt",
                        Toasttype.success)};

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


                        if ((bool?)Session["Guest"] == true)
                        {
                            Customer guestCustomer = (Customer)Session["loggedInCustomer"];

                            if (context.Order.Any(x => x.CustomerId == customer.Id && x.PriceTotal == null))
                            {
                                // wenn der User der sich eingeloggt hat noch eine bestellung offen hat!
                                Order openOrder = context.Order.Where(x => x.CustomerId == customer.Id && x.PriceTotal == null).FirstOrDefault();
                                Order guestOrder = context.Order.Where(x => x.CustomerId == guestCustomer.Id).FirstOrDefault();
                                List<OrderLine> guestbestellungen = context.OrderLine.Where(x => x.OrderId == guestOrder.Id).ToList();

                                // entfernt die guestbestellungen
                                foreach (OrderLine item in guestbestellungen)
                                {
                                    context.OrderLine.Remove(item);
                                    context.SaveChanges();
                                }
                                // fügt sie dem User hinzu
                                foreach (OrderLine item in guestbestellungen)
                                {
                                    item.OrderId = openOrder.Id;
                                    context.OrderLine.Add(item);
                                    context.SaveChanges();
                                }

                                context.Order.Remove(guestOrder);
                                context.SaveChanges();
                            }
                            else
                            {
                                context.Order.Where(x => x.CustomerId == guestCustomer.Id).FirstOrDefault().CustomerId = customer.Id;
                                context.Order.Where(x => x.CustomerId == guestCustomer.Id).FirstOrDefault().FirstName = customer.FirstName;
                                context.Order.Where(x => x.CustomerId == guestCustomer.Id).FirstOrDefault().LastName = customer.LastName;
                                context.Order.Where(x => x.CustomerId == guestCustomer.Id).FirstOrDefault().Zip = customer.Zip;
                                context.Order.Where(x => x.CustomerId == guestCustomer.Id).FirstOrDefault().City = customer.City;
                                context.SaveChanges();


                            }


                            Session["Guest"] = null;
                            context.Customer.Attach(guestCustomer);
                            context.Customer.Remove(guestCustomer);
                            context.SaveChanges();
                            DeAuthenticateUser();
                        }


                        AuthenticateUser(email);
                        TempData["justLoggedIn"] = true;
                        TempData["userEmail"] = customer.Email;
                        Session["loggedInCustomer"] = customer;



                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                    }
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
                Session["loggedInCustomer"] = null;
                Session["Guest"] = null;
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
                if ((bool?)Session["Guest"] == true)
                {
                    context.Customer.Add(newCustomer);
                    context.SaveChanges();

                    Customer guestCustomer = (Customer)Session["loggedInCustomer"];

                    context.Order.Where(x => x.CustomerId == guestCustomer.Id).FirstOrDefault().CustomerId = newCustomer.Id;
                    context.Order.Where(x => x.CustomerId == guestCustomer.Id).FirstOrDefault().FirstName = newCustomer.FirstName;
                    context.Order.Where(x => x.CustomerId == guestCustomer.Id).FirstOrDefault().LastName = newCustomer.LastName;
                    context.Order.Where(x => x.CustomerId == guestCustomer.Id).FirstOrDefault().Zip = newCustomer.Zip;
                    context.Order.Where(x => x.CustomerId == guestCustomer.Id).FirstOrDefault().City = newCustomer.City;
                    context.SaveChanges();
                    // Human @test
                    context.Customer.Attach(guestCustomer);
                    context.Customer.Remove(guestCustomer);
                    context.SaveChanges();

                    List<ToastMessage> toastMessages = new List<ToastMessage> {
                    new ToastMessage(
                        "Erledigt",
                        "Benutzer wurde erstellt",
                        Toasttype.success)};
                    ViewBag.toasts = toastMessages;

                    Session["Guest"] = null;

                    DeAuthenticateUser();
                    AuthenticateUser(email);

                    TempData["justLoggedIn"] = true;
                    TempData["userEmail"] = newCustomer.Email;
                    Session["loggedInCustomer"] = newCustomer;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    context.Customer.Add(newCustomer);
                    context.SaveChanges();
                    List<ToastMessage> toastMessages = new List<ToastMessage> {
                    new ToastMessage(
                        "Erledigt",
                        "Benutzer wurde erstellt",
                        Toasttype.success)};
                    ViewBag.toasts = toastMessages;
                    return View(false);
                }
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
