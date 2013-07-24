using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Razor;
using System.Web.Routing;
using System.Web.WebPages;
using MvcThemable.ViewPipeline.ViewEngines.Abstract;
using MvcThemable.ViewPipeline.ViewEngines.Concrete;
using MvcThemable.ViewPipeline.VirtualPathProviders;
using MvcThemable.WebUI.Views.@default.Page;

namespace MvcThemable.WebUI
{
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using MvcThemable.Core.DependencyResolver;
    using MvcThemable.Data.Abstract;
    using MvcThemable.Data.Concrete;
    using MvcThemable.Request.Abstract;
    using MvcThemable.Request.Concrete;
    using MvcThemable.ViewPipeline.ViewKeyProcessors.Abstract;
    using MvcThemable.ViewPipeline.ViewKeyProcessors.Concrete;
    using MvcThemable.ViewPipeline.ViewLocations.Abstract;
    using MvcThemable.ViewPipeline.ViewLocations.Concrete;

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

            var container = new WindsorContainer();

            container.Register(
                Component.For<IViewKeyProcessor>().ImplementedBy<ViewKeyProcessor>().LifeStyle.Singleton,
                Component.For<IDatabaseViewRepository>().ImplementedBy<DatabaseViewRepository>().LifeStyle.Singleton,
                Component.For<IProvideViewLocations>().ImplementedBy<ProvideViewLocations>().LifeStyle.Singleton,
                Component.For<IProvideCurrentRequestContext>().ImplementedBy<ProvideRequestContext>().LifeStyle.PerWebRequest);

            DependencyResolver.SetResolver(new WindsorDependencyResolver(container));

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Insert(0, new RazorDatabaseViewEngine());
            ViewEngines.Engines.Insert(1, new RazorPreCompiledViewEngine(new Dictionary<string, WebViewPage> { { "default/Page/Precompiled", new Precompiled() } }));
            ViewEngines.Engines.Insert(2, new RazorPluginViewEngine());

            HostingEnvironment.RegisterVirtualPathProvider(new DatabaseVirtualPathProvider());
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            foreach (var urlThemableViewEngine in ViewEngines.Engines.OfType<IUrlThemableViewEngine>())
            {
                urlThemableViewEngine.SetViewLocations();
            }
        }
    }
}