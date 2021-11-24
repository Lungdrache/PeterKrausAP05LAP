using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Linq;
using System.Text.RegularExpressions;
using PeterKrausAP05LAP.ViewModels;
using PeterKrausAP05LAP.Tools;

namespace PeterKrausAP05LAP.Controllers
{
    // to make a Toast!
    //List<ToastMessage>
    //toastMessages = new List<ToastMessage>();
    //toastMessages.Add(new ToastMessage("Hello", "This is a test", Toasttype.info));
    //toastMessages.Add(new ToastMessage("I have Apples", "This is a warning", Toasttype.warning));
    //ViewBag.toasts = toastMessages;

    public class HomeController : Controller
    {
        StockGamesDatabaseEntities context = new StockGamesDatabaseEntities();

        public ActionResult Index()
        {
            VM_LandingPage main = new VM_LandingPage();

            // den Abschnitt finde ich nicht gut X3
            // gibt mir die ersten 3 Product IDs der meist verkauftesten Produkte
            List<int> Top3Products = new List<int>();

            if (context.OrderLine.Count() > 3)
            {
                Top3Products = context.OrderLine
                    .GroupBy(x => x.ProductId)
                    .Select(x => new { ProductId = x.Key, QuantitySum = x.Sum(a => a.Amount) })
                    .OrderByDescending(x => x.QuantitySum)
                    .Select(x => x.ProductId)
                    .Take(3)
                    .ToList();
            }
            else
            {
                if (context.OrderLine.Count() == 3)
                {
                    Top3Products = context.OrderLine.Select(x => x.ProductId).Take(3).ToList();
                }
                else
                {
                    Top3Products = context.Product.Select(x => x.Id).Take(3).ToList();
                }
            }


            List<int> Top3Recommends = new List<int>() { 15, 145, 130, 68, 57, 50 };








            #region GET TOP3

            main.TopSeller = new List<VM_ProductDetail>();

            foreach (int productId in Top3Products)
            {
                // Finde die Produkt Daten
                Product product = context.Product.Where(x => x.Id == productId).FirstOrDefault();
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

                main.TopSeller.Add(orderedProduct);

            }
            #endregion


            #region GET RECOMENDED

            main.OurRecommends = new List<VM_ProductDetail>();

            foreach (int productId in Top3Recommends)
            {
                // Finde die Produkt Daten
                Product product = context.Product.Where(x => x.Id == productId).FirstOrDefault();
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

                main.OurRecommends.Add(orderedProduct);

            }
            #endregion



            return View(main);
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult AGB()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }




        #region Only need For Import Products
        // Import Stuff
        [HttpGet]
        [ActionName("Importer")]
        public ActionResult ImporterGet(string title = "",int errors = 0,int files = 0)
        {
            ViewBag.Result = "Want to import Files?";

            if (!string.IsNullOrWhiteSpace(title))
            {
                ViewBag.Result = title;
                ViewBag.Errors = errors;
                ViewBag.Files = files;
            }

            return View();
        }

        [HttpPost]
        [ActionName("RemoveGuests")]
        public ActionResult RemoveGuests()
        {
            // Rufe eine GuestAccount Liste ab
            List<Customer> guests = context.Customer.Where(x => x.LastName == "GUEST").ToList();

            int files = 0;
            int errors = 0;

            foreach (Customer guest in guests)
            {
                try
                {
                    // da ein Guest nur eine bestellung haben kann bevor er zum kunde wird,
                    // ist nichts anderes notwendig

                    // erfasse alle guestbestellungen
                    Order guestOrder = context.Order.Where(x => x.CustomerId == guest.Id).FirstOrDefault();
                    if (guestOrder != null)
                    {
                        List<OrderLine> allguestorders = context.OrderLine.Where(x => x.OrderId == guestOrder.Id).ToList();

                        // entfernt alle bestellungen
                        foreach (var item in allguestorders)
                        {
                            context.OrderLine.Remove(item);
                        }

                        // entfernt die Bestellung
                        context.Order.Remove(guestOrder);
                    }

                    // entfernt die GastAccounts
                    context.Customer.Remove(guest);
                    context.SaveChanges();
                    files++;
                }
                catch (Exception)
                {
                    errors++;
                }
            }

            return RedirectToAction("Importer",
                new
                {
                    title = (errors == 0) ? "Cleared all Guests" : "Cleared Guests with Problems",
                    errors = errors,
                    files = files
                });

        }

        [HttpPost]
        [ActionName("Clearer")]
        public ActionResult OrderClearer()
        {
            List<Order> allOrders = context.Order.ToList();
            List<OrderLine> allOrderLines = context.OrderLine.ToList();
            int orderCount = allOrders.Count;
            int fileCount = 0;
            int errorCount = 0;

            foreach (OrderLine orderLine in allOrderLines)
            {
                try
                {
                    context.OrderLine.Remove(orderLine);
                    context.SaveChanges();
                    fileCount++;
                }
                catch (Exception)
                {
                    errorCount++;
                }
            }

            foreach (Order order in allOrders)
            {
                try
                {
                    context.Order.Remove(order);
                    context.SaveChanges();
                    fileCount++;
                }
                catch (Exception)
                {
                    errorCount++;
                }
            }



            ViewBag.Result = (errorCount == 0)?"Cleared all Orders":"Cleared Orders with Problems";
            ViewBag.Errors = errorCount;
            ViewBag.Files = fileCount;

            return RedirectToAction("Importer",
                new
                {
                    title = (errorCount == 0) ? "Cleared all Orders" : "Cleared Orders with Problems",
                    errors = errorCount,
                    files = fileCount
                });
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
                string sqlnewDatabaseScript = System.IO.File.ReadAllText(programmPath + "/CreateDatabase.sql");

                //context.Database.SqlQuery<string>(sqlnewDatabaseScript);

                foreach (string filePath in allFiles)
                {

                    if (System.IO.File.Exists(filePath))
                    {
                        // create an instance for a new Product
                        Product newProduct = new Product();
                        // creates a list for all the Images the Product has
                        List<ProductImages> newImages = new List<ProductImages>();

                        // read out the Textfile which describes the Product
                        string[] allLines = System.IO.File.ReadAllLines(filePath);

                        // Order of filedata:
                        // GameName
                        // Prize
                        // short description
                        // Manufacturer
                        // Category
                        // Taxrate = 20
                        // Imagepaths split with a '|'
                        // Trailerpath split with a '|'

                        if (allLines.Length >= 5)
                        {
                            string[] imagepaths = allLines[6].Split('|');
                            foreach (string image in imagepaths)
                            {
                                if (!string.IsNullOrWhiteSpace(image))
                                {
                                    newImages.Add(new ProductImages() { ImagePath = image });
                                }
                            }
                            // add header Image to database
                            string path = imagepaths[0].Replace(Path.GetFileName(imagepaths[0]), "");
                            newImages.Add(new ProductImages() { ImagePath = path + "headerimage.png" });
                        }

                        if (allLines.Length >= 6)
                        {
                            newProduct.TrailerPath = allLines[7].Split('|')[0];
                        }

                        #region Prepare Category

                        Category newCategory;



                        string toSearch = allLines[4];

                        if (context.Category.Count() != 0 && context.Category.Any(x => x.Name == toSearch))
                        {
                            newCategory = context.Category.Where(x => x.Name == toSearch).FirstOrDefault();
                        }
                        else
                        {
                            // add the Category to the Database and get it with his new ID
                            newCategory = context.Category.Add(new Category() { Name = allLines[4], TaxRate = 20 });
                            context.SaveChanges();
                        }

                        #endregion

                        #region Prepare Manufacturer


                        // creates a new Manufactur if it doesn't exist
                        Manufacturer newManufacturer = new Manufacturer();

                        // bacause of the Entity Search we need to put the array into a string
                        toSearch = allLines[3];

                        if (context.Manufacturer.Count() != 0 && context.Manufacturer.Any(x => x.Name == toSearch))
                        {
                            newManufacturer = context.Manufacturer.Where(x => x.Name == toSearch).FirstOrDefault();
                        }
                        else
                        {
                            // set the name of the new created one
                            newManufacturer.Name = allLines[3];
                            // and add it to the Database, also get it from the database so i get the ID from the database
                            newManufacturer = context.Manufacturer.Add(newManufacturer);
                            context.SaveChanges();
                        }




                        #endregion


                        newProduct.ProductName = allLines[0];
                        newProduct.NetUnitPrice = (allLines[1].ToLower() != "free")?decimal.Parse(Regex.Replace(allLines[1], "[^0-9^,^.]", "")) : 0;

                        newProduct.NetUnitPrice = (newProduct.NetUnitPrice / 120) * 100;

                        newProduct.Description = allLines[2];

                        newProduct.ManufactureId = newManufacturer.Id;
                        newProduct.CategoryId = newCategory.Id;



                        newProduct = context.Product.Add(newProduct);
                        context.SaveChanges();

                        foreach (ProductImages image in newImages)
                        {
                            image.ProductId = newProduct.Id;
                            context.ProductImages.Add(image);
                        }
                        context.SaveChanges();

                        // increase the Counter for the done FIles
                        fileCount++;
                        // TODO: Workout no Double Categorys
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

        #endregion
    }
}