using System;
using System.Collections.Generic;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Razor;
using System.Web.Routing;
using System.Web.WebPages;
using databaseviewengine.ViewEngines;
using databaseviewengine.Views.@default.Page;
using databaseviewengine.VirtualPathProviders;

namespace databaseviewengine
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Page", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            RazorCodeLanguage.Languages.Add("dbhtml", new CSharpRazorCodeLanguage());
            WebPageHttpHandler.RegisterExtension("dbhtml");

            System.Web.Mvc.ViewEngines.Engines.Clear();
            System.Web.Mvc.ViewEngines.Engines.Insert(0, new DatabaseViewEngine());
            System.Web.Mvc.ViewEngines.Engines.Insert(1, new PreCompiledViewEngine(new Dictionary<string, WebViewPage> { { "default/Page/Precompiled", new Precompiled() } }));
            System.Web.Mvc.ViewEngines.Engines.Insert(2, new PluginViewEngine());

            HostingEnvironment.RegisterVirtualPathProvider(new DatabaseVirtualPathProvider());
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }
    }
}