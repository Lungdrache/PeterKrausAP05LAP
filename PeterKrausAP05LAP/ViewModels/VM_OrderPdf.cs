using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PeterKrausAP05LAP.ViewModels
{
    public class VM_OrderPdf
    {
        public decimal FullNetPrice { get; set; }
        public decimal SumTax { get; set; } // alle Taxes zusammengerechnet
        public decimal FullBrutPrice { get; set; }
        public List<VM_ProductDetail> OrderedProducts { get; set; }
        public Customer Kunde { get; set; }
    }
}