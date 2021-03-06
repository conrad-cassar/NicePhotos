﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;

namespace NicePhotos
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            //log4net.Config.XmlConfigurator.Configure();

            RouteTable.Routes.MapHttpRoute(
                name: "DefaultApi", 
                routeTemplate: "api/{controller}/{username}", 
                defaults: new { username = System.Web.Http.RouteParameter.Optional});

        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (Context.User != null)
            {
                if (!string.IsNullOrEmpty(Context.User.Identity.Name))
                {
                    System.Security.Principal.GenericPrincipal genPr =
                        new System.Security.Principal.GenericPrincipal(
                            Context.User.Identity, 
                            new BL.Roles(Context.User.Identity.Name).GetUserRolesIds(Context.User.Identity.Name));

                    Context.User = genPr;
                }
            }
        }

    }
}
