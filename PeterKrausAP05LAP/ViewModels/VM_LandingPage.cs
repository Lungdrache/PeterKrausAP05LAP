using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PeterKrausAP05LAP.ViewModels
{
    public class VM_LandingPage
    {
        public List<VM_ProductDetail> TopSeller { get; set; }
        public List<VM_ProductDetail> OurRecommends { get; set; }
        

    }
}