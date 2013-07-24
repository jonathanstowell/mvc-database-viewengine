using System;
using System.Linq;
using System.Web.Mvc;
using Bootstrap;
using Bootstrap.Extensions.StartupTasks;
using Bootstrap.Windsor;
using MvcThemable.ViewPipeline.ViewEngines.Abstract;
using MvcThemable.WebUI.Installers;

namespace MvcThemable.WebUI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Bootstrapper.With.Windsor()
                .And.StartupTasks()
                .UsingThisExecutionOrder(s => s
                        .First<RegisterDependencyResolver>()
                        .Then<RegisterRaven>()
                        .Then().TheRest())
                .Start();

            AreaRegistration.RegisterAllAreas();
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