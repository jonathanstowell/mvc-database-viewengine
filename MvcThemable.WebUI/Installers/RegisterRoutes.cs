using System;
using System.Web.Mvc;
using System.Web.Routing;
using Bootstrap.Extensions.StartupTasks;

namespace MvcThemable.WebUI.Installers
{
    public class RegisterRoutes : IStartupTask
    {
        public void Run()
        {
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Page", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                );
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}