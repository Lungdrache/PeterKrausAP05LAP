using PeterKrausAP05LAP.Tools;
using PeterKrausAP05LAP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PeterKrausAP05LAP.Controllers
{
    public class ShopController : Controller
    {
        StockGamesDatabaseEntities context = new StockGamesDatabaseEntities();

        // GET: Shop
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ActionName("ShopCart")]
        public ActionResult ShopCartGet()
        {
            List<VM_ProductDetail> products = new List<VM_ProductDetail>();

            if (Session["loggedInCustomer"] != null)
            {
                // ist der User eingeloggt
                Customer customer = (Customer)Session["loggedInCustomer"];
                // Suche seine noch nicht vollendete Bestellung raus
                Order openOrder = context.Order.Where(x => x.CustomerId == customer.Id && x.PriceTotal == null).FirstOrDefault();
                // TODO: WHAT IF SHOPCART IS EMPTY!
                List<OrderLine> orders = context.OrderLine.Where(x => x.OrderId == openOrder.Id).ToList();

                foreach (OrderLine order in orders)
                {
                    // Finde die Produkt Daten
                    Product product = context.Product.Where(x => x.Id == order.ProductId).FirstOrDefault();
                    Category category = context.Category.Where(x => x.Id == product.CategoryId).FirstOrDefault();
                    Manufacturer manufacturer = context.Manufacturer.Where(x => x.Id == product.ManufactureId).FirstOrDefault();

                    List<ProductImages> productImages = context.ProductImages.Where(x => x.ProductId == product.Id).ToList();
                    List<string> allimagePaths = new List<string>();
                    string headerImages = "";
                    foreach (ProductImages image in productImages)
                    {
                        if (Path.GetFileNameWithoutExtension(image.ImagePath) == "headerimage")
                        {
                            headerImages = "../Images" + Regex.Replace(image.ImagePath, "[^A-Za-z^0-9^/^.]", "");
                        }
                        else
                        {
                            allimagePaths.Add("../Images" + Regex.Replace(image.ImagePath, "[^A-Za-z^0-9^/^.]", ""));
                        }
                    }

                    VM_ProductDetail orderedProduct = new VM_ProductDetail()
                    {
                        Id = product.Id,
                        categoryId = category.Id,
                        productName = product.ProductName,
                        price = product.NetUnitPrice,
                        tax = category.TaxRate,
                        manufactureName = manufacturer.Name,
                        manufactureId = manufacturer.Id,
                        categoryName = category.Name,
                        description = product.Description,
                        imagePaths = allimagePaths,
                        imageHeaderPath = headerImages,
                        videoPath = product.TrailerPath
                    };

                    products.Add(orderedProduct);
                }

            }

            return View(products);
        }





        // User.Identity.IsAuthenticated
        [HttpGet]
        [ActionName("ShopPage")]
        public ActionResult ShopIndex(int? addCart, int? pageSelect)
        {
            if (addCart != null)
            {
                Product addedProduct = context.Product.Where(x => x.Id == addCart).FirstOrDefault();
                Category productCategory = context.Category.Where(x => x.Id == addedProduct.CategoryId).FirstOrDefault();
                Order newOrder = new Order();

                int orderID = 0;
                if (Session["OrderID"] == null)
                {
                    // TODO: working
                    Customer loggedinUser = new Customer();
                    if (Session["loggedInCustomer"] == null)
                    {
                        Random ran = new Random();
                        string guestName = "GUEST" + ran.Next();
                        // wenn es sich um einen Gast handelt
                        loggedinUser = new Customer(
                            "",
                            guestName,
                            "GUEST",
                            guestName,
                            "GUEST",
                            "GUEST",
                            "GUEST",
                            "GUEST"
                            );

                        AuthenticateUser(loggedinUser.Email);
                        TempData["justLoggedIn"] = true;
                        TempData["userEmail"] = loggedinUser.Email;

                        Session["loggedInCustomer"] = loggedinUser;
                        Session["Guest"] = true;
                        using (StockGamesDatabaseEntities dbSetup = new StockGamesDatabaseEntities())
                        {
                            dbSetup.Customer.Add(loggedinUser);
                            dbSetup.SaveChanges();
                        }

                    }
                    else
                    {
                        // wenn jemand eingeloggt ist
                        loggedinUser = (Customer)Session["loggedInCustomer"];
                    }
                    newOrder = new Order(loggedinUser, 0);
                    newOrder = context.Order.Add(newOrder);
                    context.SaveChanges();
                    Session["OrderID"] = newOrder.Id;

                }
                else
                {
                    orderID = (int)Session["OrderID"];
                    newOrder = context.Order.Where(x => x.Id == orderID).FirstOrDefault();
                }
                

                List<ToastMessage> toastMessages = new List<ToastMessage> {
                    new ToastMessage(
                        "Hinzugefügt zum Warenkorb",
                        addedProduct.ProductName + " wurde in ihren Warenkorb hinzugefügt.",
                        Toasttype.info)
                };
                ViewBag.toasts = toastMessages;



                OrderLine newItem = new OrderLine(newOrder, addedProduct, productCategory.TaxRate);
                context.OrderLine.Add(newItem);
                context.SaveChanges();
            }

            List<VM_Product> someProducts = new List<VM_Product>();

            int currentPage = (pageSelect == null) ? 1 : pageSelect.Value;
            int productCount = context.Product.Count();
            int numberOfPages = (int)Math.Round((float)productCount/25f,0);
            List<Product> importedProducts = new List<Product>();

            if ((currentPage)*25 > productCount)
            {
                int restproducts = productCount - (currentPage - 1) * 25;
                importedProducts = context.Product.OrderBy(x => x.ProductName).Skip((currentPage - 1) * 25).Take(restproducts).ToList();
            }
            else
            {
                importedProducts = context.Product.OrderBy(x => x.ProductName).Skip((currentPage - 1) * 25).Take(25).ToList();
            }



            foreach (Product product in importedProducts)
            {

                VM_Product toExport = new VM_Product();


                toExport.Id = product.Id;
                toExport.ProductName = product.ProductName;
                List<ProductImages> productImages = context.ProductImages.Where(x => x.ProductId == product.Id).ToList();

                foreach (ProductImages image in productImages)
                {
                    if (Path.GetFileNameWithoutExtension(image.ImagePath) == "headerimage")
                    {
                        toExport.HeaderImgPath = "../Images" + Regex.Replace(image.ImagePath, "[^A-Za-z^0-9^/^.]", "");
                        break;
                    }
                }
                toExport.ShortDescription = product.Description;
                someProducts.Add(toExport);
            }
            VM_ShopView shopView = new VM_ShopView()
            {
                PageCount = numberOfPages,
                PageNumber = currentPage,
                AllProducts = someProducts};


            return View(shopView);
        }

        [HttpPost]
        [ActionName("ShopPage")]
        public ActionResult ShopFilter(string name, int? category, string manufacturer, int? prize)
        {
            List<VM_Product> someProducts = new List<VM_Product>();







            return View(someProducts);
        }

        [HttpGet]
        [ActionName("ProductDetail")]
        public ActionResult ProductDetailGet(int? id)
        {
            if (id == null)
            {
                id = 2;
            }

            VM_ProductDetail product = new VM_ProductDetail();
            Product dBProduct = context.Product.Where(x => x.Id == id).FirstOrDefault();
            Category dBCategory = context.Category.Where(x => x.Id == dBProduct.CategoryId).FirstOrDefault();
            Manufacturer dBManufacturer = context.Manufacturer.Where(x => x.Id == dBProduct.ManufactureId).FirstOrDefault();

            product.Id = id.Value;
            product.productName = dBProduct.ProductName;


            List<ProductImages> productImages = context.ProductImages.Where(x => x.ProductId == product.Id).ToList();
            product.imagePaths = new List<string>();

            foreach (ProductImages image in productImages)
            {
                if (Path.GetFileNameWithoutExtension(image.ImagePath) == "headerimage")
                {
                    product.imageHeaderPath = "../../Images" + Regex.Replace(image.ImagePath, "[^A-Za-z^0-9^/^.]", "");
                }
                else
                {
                    product.imagePaths.Add("../../Images" + Regex.Replace(image.ImagePath, "[^A-Za-z^0-9^/^.]", ""));
                }
            }

            product.manufactureId = dBProduct.ManufactureId;
            product.categoryId = dBProduct.CategoryId;
            product.videoPath = dBProduct.TrailerPath;
            product.manufactureName = context.Manufacturer.Where(x => x.Id == product.manufactureId).FirstOrDefault().Name;
            product.categoryName = context.Category.Where(x => x.Id == product.categoryId).FirstOrDefault().Name;
            product.price = dBProduct.NetUnitPrice;
            product.tax = dBCategory.TaxRate;
            product.description = dBProduct.Description;



            return View(product);
        }


        [HttpGet]
        [ActionName("ProductDelete")]
        public ActionResult ProductDetaildelete(int? id)
        {
            Customer customer = (Customer)Session["loggedInCustomer"];
            Order order = context.Order.Where(x => x.PriceTotal == null && x.CustomerId == customer.Id).FirstOrDefault();
            OrderLine toRemove = context.OrderLine.Where(x => x.OrderId == order.Id && x.ProductId == id).FirstOrDefault();

            context.OrderLine.Remove(toRemove);
            context.SaveChanges();
            return RedirectToAction("ShopCart");
        }


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