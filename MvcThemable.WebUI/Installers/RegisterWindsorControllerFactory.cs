using System;
using System.Web.Mvc;
using Bootstrap;
using Bootstrap.Extensions.StartupTasks;
using Castle.Windsor;
using MvcThemable.Core.ControllerFactory;

namespace MvcThemable.WebUI.Installers
{
    public class RegisterWindsorControllerFactory : IStartupTask
    {
        public void Run()
        {
            var controllerFactory = new WindsorControllerFactory((IWindsorContainer)Bootstrapper.Container);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}