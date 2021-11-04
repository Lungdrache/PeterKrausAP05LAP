using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PeterKrausAP05LAP.Tools
{
	public enum Toasttype
	{
		success,
		error,
		warning,
		info
	}

    public class ToastMessage
    {
		#region Felder

		private string _title;
		private string _message;
		private Toasttype _type;
		#endregion

		#region Eigenschaften

		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}
		public string Message
		{
			get { return _message; }
			set { _message = value; }
		}
		public Toasttype Type
		{
			get { return _type; }
			set { _type = value; }
		}
		#endregion


		#region Konstruktor
		public ToastMessage(string title,string message, Toasttype type)
		{
			Title = title;
			Message = message;
			Type = type;
		}
		#endregion

	}
}