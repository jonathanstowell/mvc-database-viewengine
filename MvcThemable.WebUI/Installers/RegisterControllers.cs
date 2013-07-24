using System;
using System.Web.Mvc;
using Bootstrap;
using Bootstrap.Extensions.StartupTasks;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace MvcThemable.WebUI.Installers
{
    public class RegisterControllers : IStartupTask
    {
        public void Run()
        {
            ((IWindsorContainer) Bootstrapper.Container).Register(
                Classes.FromThisAssembly().BasedOn<IController>().Configure(
                    component =>
                        {
                            component.Named(component.Implementation.Name);
                            component.LifeStyle.Is(LifestyleType.Transient);
                        }).WithService.Base());
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}