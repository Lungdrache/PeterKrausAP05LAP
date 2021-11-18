using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PeterKrausAP05LAP.ViewModels
{
    public class VM_ProductDetail
    {
        public int Id { get; set; }
        public string productName { get; set; }
        public decimal price { get; set; }
        public decimal tax { get; set; }
        public string manufactureName { get; set; }
        public int manufactureId { get; set; }
        public string categoryName { get; set; }
        public int categoryId { get; set; }
        public string description { get; set; }
        public List<string> imagePaths { get; set; }
        public string imageHeaderPath { get; set; }
        public string videoPath { get; set; }
    }
}