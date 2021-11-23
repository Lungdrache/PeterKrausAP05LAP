using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PeterKrausAP05LAP.ViewModels
{
    public class VM_ShopView
    {
        public int PageNumber { get; set; } // Current Page
        public int PageCount { get; set; } // How many Pages there are
        public List<VM_Product> AllProducts { get; set; }
    }
}