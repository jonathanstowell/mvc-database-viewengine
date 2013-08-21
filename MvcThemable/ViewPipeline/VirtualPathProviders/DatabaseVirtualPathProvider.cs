using System;
using System.Collections;
using System.Web.Caching;
using System.Web.Hosting;
using MvcThemable.Entities.Concrete;
using MvcThemable.Service.Abstract;
using MvcThemable.ViewPipeline.ViewKeyProcessors.Abstract;
using MvcThemable.ViewPipeline.VirtualFiles;

namespace MvcThemable.ViewPipeline.VirtualPathProviders
{
    using System.Web.Mvc;

    public class DatabaseVirtualPathProvider : VirtualPathProvider
    {
        private readonly IViewKeyProcessor viewKeyProcessor;
        private readonly IDatabaseViewService service;

        public DatabaseVirtualPathProvider()
        {
            viewKeyProcessor = DependencyResolver.Current.GetService<IViewKeyProcessor>();
            service = DependencyResolver.Current.GetService<IDatabaseViewService>();
        }

        public override bool FileExists(string virtualPath)
        {
            return FindPage(virtualPath) != null || Previous.FileExists(virtualPath);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            var page = FindPage(virtualPath);
            return page == null ? Previous.GetFile(virtualPath) : new DatabaseFile(virtualPath, page.Body, page.ViewModel != null ? page.ViewModel.FullName : null, page.Layout);
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            return FindPage(virtualPath) != null ? null : Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        public override string GetFileHash(string virtualPath, IEnumerable virtualPathDependencies)
        {
            var file = FindPage(virtualPath);

            if (file != null)
            {
                return file.ViewKey + file.LastModifiedDateTime;
            }

            return base.GetFileHash(virtualPath, virtualPathDependencies);
        }

        protected DatabaseView FindPage(string virtualPath)
        {
            string key = viewKeyProcessor.Retrieve(virtualPath);

            if (!string.IsNullOrEmpty(key))
                return service.GetByViewKey(key);

            return null;
        }
    }
}