using System;
using System.Web.Mvc;
using Bootstrap;
using Bootstrap.Extensions.StartupTasks;
using Castle.Windsor;
using MvcThemable.Core.DependencyResolver;

namespace MvcThemable.WebUI.Installers
{
    public class RegisterDependencyResolver : IStartupTask
    {
        public void Run()
        {
            DependencyResolver.SetResolver(new WindsorDependencyResolver((IWindsorContainer)Bootstrapper.Container));
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}