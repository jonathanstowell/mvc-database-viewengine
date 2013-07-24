using System;
using Bootstrap;
using Bootstrap.Extensions.StartupTasks;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using MvcThemable.Core.Castle;
using Raven.Client;
using Raven.Client.Document;

namespace MvcThemable.WebUI.Installers
{
    public class RegisterRaven : IStartupTask
    {
        public void Run()
        {
            ((IWindsorContainer)Bootstrapper.Container).Register(
                Component.For<IDocumentStore>().Instance(CreateDocumentStore()).LifeStyle.Singleton,
                Component.For<IDocumentSession>().UsingFactoryMethod(GetDocumentSesssion).LifestyleScoped<HybridPerWebRequestScopedAccessor>()
                );
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        static IDocumentStore CreateDocumentStore()
        {
            var store = new DocumentStore
                {
                    ConnectionStringName = "ViewStore"
                };

            store.Initialize();

            return store;
        }

        static IDocumentSession GetDocumentSesssion(IKernel kernel)
        {
            var store = kernel.Resolve<IDocumentStore>();
            return store.OpenSession();
        }
    }
}