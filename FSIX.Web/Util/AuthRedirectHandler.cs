using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using System.Security.Principal;

namespace FSIX.Web
{
    class AuthRedirectHandler : IHttpModule
    {

        #region IHttpModule Members

        public void Dispose() { }

        public void Init(HttpApplication context)
        {
            context.EndRequest += new EventHandler(context_EndRequest);
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            if (app.Response.StatusCode == 302)
            {
                app.Response.StatusCode = 401;
                app.Response.StatusDescription = "Authentication required";
                app.Response.SuppressFormsAuthenticationRedirect = true;
            }
        }

        #endregion
    }
}