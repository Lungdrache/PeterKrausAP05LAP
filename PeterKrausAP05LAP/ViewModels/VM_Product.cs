using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PeterKrausAP05LAP
{
    public class VM_Product
    {
		private string _productName;

		public string ProductName
		{
			get { return _productName; }
			set { _productName = value; }
		}
		private string _headerImgPath;

		public string HeaderImgPath
		{
			get { return _headerImgPath; }
			set { _headerImgPath = value; }
		}
		private string _shortDescription;

		public string ShortDescription
		{
			get { return _shortDescription; }
			set { _shortDescription = value; }
		}


		public VM_Product()
		{

		}
		public VM_Product(string name, string headerImgPath,string shortDescription)
		{
			ProductName = name;
			HeaderImgPath = headerImgPath;
			ShortDescription = shortDescription;
		}





	}
}