using System;
using System.Collections;
using System.Web.Caching;
using System.Web.Hosting;
using MvcThemable.Data.Abstract;
using MvcThemable.Data.Concrete;
using MvcThemable.Entities.Abstract;
using MvcThemable.ViewPipeline.ViewKeyProcessors.Abstract;
using MvcThemable.ViewPipeline.ViewKeyProcessors.Concrete;
using MvcThemable.ViewPipeline.VirtualFiles;

namespace MvcThemable.ViewPipeline.VirtualPathProviders
{
    public class DatabaseVirtualPathProvider : VirtualPathProvider
    {
        private readonly IViewKeyProcessor viewKeyProcessor;

        private readonly IDatabaseViewRepository repository;

        public DatabaseVirtualPathProvider()
            : this(new ViewKeyProcessor(), new DatabaseViewRepository())
        {}

        public DatabaseVirtualPathProvider(IViewKeyProcessor viewKeyProcessor, IDatabaseViewRepository repository)
        {
            this.viewKeyProcessor = viewKeyProcessor;
            this.repository = repository;
        }

        public override bool FileExists(string virtualPath)
        {
            return FindPage(virtualPath) != null || Previous.FileExists(virtualPath);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            var page = FindPage(virtualPath);
            return page == null ? Previous.GetFile(virtualPath) : new DatabaseFile(virtualPath, page.Body);
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            return null;
        }

        protected IDatabaseView FindPage(string virtualPath)
        {
            string key = viewKeyProcessor.Retrieve(virtualPath);

            if (!string.IsNullOrEmpty(key))
                return repository.GetByViewKey(key);

            return null;
        }
    }
}