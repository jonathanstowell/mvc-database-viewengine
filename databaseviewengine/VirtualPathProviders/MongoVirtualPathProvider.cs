using System;
using System.Collections;
using System.Web.Caching;
using System.Web.Hosting;
using databaseviewengine.Data;
using databaseviewengine.Entities;
using databaseviewengine.ViewKeyProcessors;
using databaseviewengine.VirtualFiles;

namespace databaseviewengine.VirtualPathProviders
{
    public class MongoVirtualPathProvider : VirtualPathProvider
    {
        private readonly PageRepository repository;
        private readonly DatabaseViewKeyProcessor viewKeyProcessor;

        public MongoVirtualPathProvider()
        {
            repository = new PageRepository();
            viewKeyProcessor = new DatabaseViewKeyProcessor();
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
            if (FindPage(virtualPath) != null)
            {
                return null;
            }

            return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        protected Page FindPage(string virtualPath)
        {
            string domain, pagepath;

            bool result = viewKeyProcessor.TryParseDomainAndPagePathFromVirtualPath(virtualPath, out domain, out pagepath);

            if (result)
                return repository.GetByDomainAndViewName(domain, pagepath);

            return null;
        }
    }
}