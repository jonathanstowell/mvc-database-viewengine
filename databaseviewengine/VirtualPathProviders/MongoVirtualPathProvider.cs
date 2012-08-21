using System;
using System.Collections;
using System.Web.Caching;
using System.Web.Hosting;
using databaseviewengine.Data;
using databaseviewengine.Entities;
using databaseviewengine.VirtualFiles;

namespace databaseviewengine.VirtualPathProviders
{
    public class MongoVirtualPathProvider : VirtualPathProvider
    {
        private readonly PageRepository repository;

        public MongoVirtualPathProvider()
        {
            repository = new PageRepository();
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

            TryParseDomainAndPagePathFromVirtualPath(virtualPath, out domain, out pagepath);

            return repository.GetByDomainAndViewName(domain, pagepath);
        }

        private void TryParseDomainAndPagePathFromVirtualPath(string virtualPath, out string domain, out string pagePath)
        {
            string[] breakdown = BreakdownVirtualPathIntoDomainControllerAndPage(virtualPath);

            if (breakdown.Length < 3)
            {
                domain = string.Empty;
                pagePath = string.Empty;

                return;
            }

            domain = breakdown[0];
            pagePath = string.Format("{0}/{1}", breakdown[1], breakdown[2]);
        }

        private string[] BreakdownVirtualPathIntoDomainControllerAndPage(string virtualPath)
        {
            int startIndex = virtualPath.LastIndexOf("Views/", StringComparison.Ordinal);

            if (startIndex == -1)
                return new string[0];

            string sub = virtualPath.Replace(".cshtml", "");

            sub = sub.Substring(startIndex + 6, sub.Length - (startIndex + 6));

            return sub.Split('/');
        }
    }
}