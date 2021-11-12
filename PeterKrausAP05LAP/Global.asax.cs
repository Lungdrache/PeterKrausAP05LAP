using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace PeterKrausAP05LAP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_AuthenticateRequest(Object o, EventArgs eventArgs)
        {
            var cookie = Request.Cookies.Get(FormsAuthentication.FormsCookieName);

            if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value))
            {
                return;
            }


            var encryptedCookie = cookie.Value;


            var Ticket = FormsAuthentication.Decrypt(encryptedCookie);


            IIdentity useridentity = new GenericIdentity(Ticket.Name);

            HttpContext.Current.User = new GenericPrincipal(useridentity,null);
        }
    }
}
