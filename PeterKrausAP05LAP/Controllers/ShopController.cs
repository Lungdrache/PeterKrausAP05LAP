using PeterKrausAP05LAP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

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
            return View(cartProducts);
        }
        [HttpPost]
        [ActionName("ShopCart")]
        public ActionResult ShopCartEdit(int id)
        {
            List<VM_ProductDetail> products = new List<VM_ProductDetail>();



            return View(products);
        }




        [HttpGet]
        [ActionName("ShopPage")]
        public ActionResult ShopIndex()
        {
            List<VM_Product> someProducts = new List<VM_Product>();
            List<Product> importedProducts = context.Product.Take(50).ToList();

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
    }
}