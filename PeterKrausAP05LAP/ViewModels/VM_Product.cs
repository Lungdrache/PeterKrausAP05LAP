using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PeterKrausAP05LAP
{
    public class VM_Product
    {
		private int _id;

		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

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
		private int _pageVisits;

		public int PageVisits
		{
			get { return _pageVisits; }
			set { _pageVisits = value; }
		}
		private int _rate;

		public int Rate
		{
			get { return _rate; }
			set { _rate = value; }
		}




		public VM_Product()
		{

		}
		public VM_Product(int id, string name, string headerImgPath,string shortDescription,int rating = 0,int pagevisits = 0)
		{
			Id = id;
			ProductName = name;
			HeaderImgPath = headerImgPath;
			ShortDescription = shortDescription;
			PageVisits = pagevisits;
			Rate = rating;
		}





	}
}