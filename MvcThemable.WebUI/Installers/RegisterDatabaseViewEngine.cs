using System;
using System.Collections.Generic;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Razor;
using System.Web.WebPages;
using Bootstrap;
using Bootstrap.Extensions.StartupTasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using MvcThemable.Core.DatabaseViewRepositories;
using MvcThemable.Data.Abstract;
using MvcThemable.Request.Abstract;
using MvcThemable.Request.Concrete;
using MvcThemable.ViewPipeline.ViewEngines.Concrete;
using MvcThemable.ViewPipeline.ViewKeyProcessors.Abstract;
using MvcThemable.ViewPipeline.ViewKeyProcessors.Concrete;
using MvcThemable.ViewPipeline.ViewLocations.Abstract;
using MvcThemable.ViewPipeline.ViewLocations.Concrete;
using MvcThemable.ViewPipeline.VirtualPathProviders;
using MvcThemable.Views.Models.Abstract;
using MvcThemable.Views.Models.Concrete;
using MvcThemable.WebUI.Views.@default.Page;

namespace MvcThemable.WebUI.Installers
{
    public class RegisterDatabaseViewEngine : IStartupTask
    {
        public void Run()
        {
            RazorCodeLanguage.Languages.Add("dbhtml", new CSharpRazorCodeLanguage());
            WebPageHttpHandler.RegisterExtension("dbhtml");

            ((IWindsorContainer)Bootstrapper.Container).Register(
                Component.For<IViewKeyProcessor>().ImplementedBy<ViewKeyProcessor>().LifeStyle.Singleton,
                Component.For<IDatabaseViewRepository>().ImplementedBy<RavenDatabaseViewRepository>().LifeStyle.Singleton,
                Component.For<IProvideViewLocations>().ImplementedBy<ProvideViewLocations>().LifeStyle.Singleton,
                Component.For<IProvideCurrentRequestContext>().ImplementedBy<ProvideRequestContext>().LifeStyle.Singleton,
                Component.For<IProvideViewModels>().ImplementedBy<ProvideViewModels>().LifeStyle.Singleton);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Insert(0, new RazorDatabaseViewEngine());
            ViewEngines.Engines.Insert(1, new RazorPreCompiledViewEngine(new Dictionary<string, WebViewPage> { { "default/Page/Precompiled", new Precompiled() } }));
            ViewEngines.Engines.Insert(2, new RazorPluginViewEngine());

            HostingEnvironment.RegisterVirtualPathProvider(new DatabaseVirtualPathProvider());
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}