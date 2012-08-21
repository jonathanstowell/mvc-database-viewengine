using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using databaseviewengine.Data;
using databaseviewengine.Entities;
using databaseviewengine.Extensions;
using databaseviewengine.RequestContext;

namespace databaseviewengine.ViewEngines
{
    public class MongoViewEngine : RazorViewEngine
    {
        internal static readonly string ViewStartFileName = "~/Views/_ViewStart";

        private readonly ProvideRequestContext requestContext;
        private readonly PageRepository repository;

        private IDictionary<string, WebViewPage> precompiledViews;

        public MongoViewEngine(IDictionary<string, WebViewPage> precompiledViews)
            : this(null, precompiledViews)
        {
        }

        public MongoViewEngine(IViewPageActivator viewPageActivator, IDictionary<string, WebViewPage> precompiledViews)
            : base(viewPageActivator)
        {
            requestContext = new ProvideRequestContext();
            repository = new PageRepository();

            this.precompiledViews = precompiledViews;

            ViewLocationFormats = new[]
                                {
                                    "~/Plugins/Views/" + requestContext.CurrentHost + "/{1}/{0}.cshtml",
                                    "~/Plugins/Views/default/{1}/{0}.cshtml",
                                    "~/Views/" + requestContext.CurrentHost + "/{1}/{0}.cshtml",
                                    "~/Views/default/{1}/{0}.cshtml",
                                    "~/Plugins/Views/" + requestContext.CurrentHost + "/Shared/{0}.cshtml",
                                    "~/Plugins/Views/default/Shared/{0}.cshtml",
                                    "~/Views/" + requestContext.CurrentHost + "/Shared/{0}.cshtml",
                                    "~/Views/default/Shared/{0}.cshtml"
                                    
                                };

            PartialViewLocationFormats = new[]
                                        {
                                            "~/Plugins/Views/" + requestContext.CurrentHost + "/{1}/{0}.cshtml",
                                            "~/Plugins/Views/default/{1}/{0}.cshtml",
                                            "~/Views/" + requestContext.CurrentHost + "/{1}/{0}.cshtml",
                                            "~/Views/default/{1}/{0}.cshtml",
                                            "~/Plugins/Views/" + requestContext.CurrentHost + "/Shared/{0}.cshtml",
                                             "~/Plugins/Views/default/Shared/{0}.cshtml",
                                            "~/Views/" + requestContext.CurrentHost + "/Shared/{0}.cshtml",
                                            "~/Views/default/Shared/{0}.cshtml"
                                        };

            FileExtensions = new [] { "cshtml" };
        }

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            return DatabaseViewExists(virtualPath) || base.FileExists(controllerContext, virtualPath);
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            if(DatabaseViewExists(partialPath))
                return base.CreatePartialView(controllerContext, partialPath);

            if (PrecompiledViewExists(partialPath))
                return GetPrecompiledView(partialPath, false);

            return base.CreatePartialView(controllerContext, partialPath);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            if (DatabaseViewExists(viewPath))
                return base.CreateView(controllerContext, viewPath, masterPath);

            if (PrecompiledViewExists(viewPath))
                return GetPrecompiledView(viewPath, true);

            return base.CreateView(controllerContext, viewPath, masterPath);
        }

        private Page GetDatabaseView(string virtualPath)
        {
            string domain, pagepath;

            TryParseDomainAndPagePathFromVirtualPath(virtualPath, out domain, out pagepath);

            return repository.GetByDomainAndViewName(domain, pagepath);
        }

        private PrecompiledView GetPrecompiledView(string virtualPath, bool runViewStart)
        {
            WebViewPage type;

            bool item = precompiledViews.TryGetValue(GetPrecompiledKey(virtualPath), out type);

            if (!item)
                return null;

            return new PrecompiledView(virtualPath, type, runViewStart, FileExtensions);
        }

        public bool DatabaseViewExists(string virtualPath)
        {
            string domain, pagepath;

            TryParseDomainAndPagePathFromVirtualPath(virtualPath, out domain, out pagepath);

            return repository.GetByDomainAndViewName(domain, pagepath) != null;
        }

        public bool PrecompiledViewExists(string virtualPath)
        {
            return precompiledViews.ContainsKey(GetPrecompiledKey(virtualPath));
        }

        public bool IsPhysicalFileNewer(string virtualPath)
        {
            string path = HttpContext.Current.Request.MapPath(virtualPath);

            if (!File.Exists(path))
                return false;

            WebViewPage page;

            bool item = precompiledViews.TryGetValue(GetPrecompiledKey(virtualPath), out page);

            if (!item)
                return (!DatabaseViewExists(virtualPath) || GetDatabaseView(virtualPath).LastModifiedDateTime < File.GetLastWriteTimeUtc(path));

            Assembly assembly = Assembly.GetAssembly(page.GetType());

            DateTime lastModified = assembly.GetLastWriteTimeUtc(DateTime.MaxValue);

            return (!DatabaseViewExists(virtualPath) || GetDatabaseView(virtualPath).LastModifiedDateTime < File.GetLastWriteTimeUtc(path)) && (lastModified < File.GetLastWriteTimeUtc(path));
        }

        public bool IsPrecompiledFileNewer(string virtualPath)
        {
            if (!PrecompiledViewExists(virtualPath))
                return false;

            WebViewPage page;

            bool item = precompiledViews.TryGetValue(GetPrecompiledKey(virtualPath), out page);

            if (!item)
                return false;

            Assembly assembly = Assembly.GetAssembly(page.GetType());

            DateTime lastModified = assembly.GetLastWriteTimeUtc(DateTime.MaxValue);

            string path = HttpContext.Current.Request.MapPath(virtualPath);

            return (!DatabaseViewExists(virtualPath) || GetDatabaseView(virtualPath).LastModifiedDateTime < lastModified) && (!File.Exists(path) || File.GetLastWriteTimeUtc(path) < lastModified);
        }

        public bool IsDatabaseRecordNewer(string virtualPath)
        {
            if (!DatabaseViewExists(virtualPath))
                return false;

            WebViewPage page;

            bool item = precompiledViews.TryGetValue(GetPrecompiledKey(virtualPath), out page);

            string path = HttpContext.Current.Request.MapPath(virtualPath);

            if (!item)
                return (!File.Exists(path) || File.GetLastWriteTimeUtc(path) < GetDatabaseView(virtualPath).LastModifiedDateTime);

            Assembly assembly = Assembly.GetAssembly(page.GetType());

            DateTime lastModified = assembly.GetLastWriteTimeUtc(DateTime.MaxValue);

            return (!File.Exists(path) || File.GetLastWriteTimeUtc(path) < GetDatabaseView(virtualPath).LastModifiedDateTime) && (lastModified < GetDatabaseView(virtualPath).LastModifiedDateTime);
        }

        private string GetPrecompiledKey(string virtualPath)
        {
            string domain, pagepath;

            TryParseDomainAndPagePathFromVirtualPath(virtualPath, out domain, out pagepath);

            return string.Format("{0}/{1}", domain, pagepath);
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