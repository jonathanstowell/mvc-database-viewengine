using System.Web.Mvc;
using databaseviewengine.Data;
using databaseviewengine.RequestContext;
using databaseviewengine.ViewKeyProcessors;

namespace databaseviewengine.ViewEngines
{
    public class DatabaseViewEngine : RazorViewEngine
    {
        private readonly ProvideRequestContext requestContext;
        private readonly PageRepository repository;
        private readonly DatabaseViewKeyProcessor viewKeyProcessor;

        public DatabaseViewEngine()
            : this(null)
        {}

        public DatabaseViewEngine(IViewPageActivator viewPageActivator)
            : base(viewPageActivator)
        {
            requestContext = new ProvideRequestContext();
            repository = new PageRepository();
            viewKeyProcessor = new DatabaseViewKeyProcessor();

            ViewLocationFormats = new[]
                                {
                                    "~/Plugins/Views/" + requestContext.CurrentHost + "/{1}/{0}.dbhtml",
                                    "~/Plugins/Views/default/{1}/{0}.dbhtml",
                                    "~/Views/" + requestContext.CurrentHost + "/{1}/{0}.dbhtml",
                                    "~/Views/default/{1}/{0}.dbhtml",
                                    "~/Plugins/Views/" + requestContext.CurrentHost + "/Shared/{0}.dbhtml",
                                    "~/Plugins/Views/default/Shared/{0}.dbhtml",
                                    "~/Views/" + requestContext.CurrentHost + "/Shared/{0}.dbhtml",
                                    "~/Views/default/Shared/{0}.dbhtml"
                                    
                                };

            PartialViewLocationFormats = new[]
                                        {
                                            "~/Plugins/Views/" + requestContext.CurrentHost + "/{1}/{0}.dbhtml",
                                            "~/Plugins/Views/default/{1}/{0}.dbhtml",
                                            "~/Views/" + requestContext.CurrentHost + "/{1}/{0}.dbhtml",
                                            "~/Views/default/{1}/{0}.dbhtml",
                                            "~/Plugins/Views/" + requestContext.CurrentHost + "/Shared/{0}.dbhtml",
                                             "~/Plugins/Views/default/Shared/{0}.dbhtml",
                                            "~/Views/" + requestContext.CurrentHost + "/Shared/{0}.dbhtml",
                                            "~/Views/default/Shared/{0}.dbhtml"
                                        };

            FileExtensions = new[] { "dbhtml" };
        }

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            return DatabaseViewExists(virtualPath);
        }

        public bool DatabaseViewExists(string virtualPath)
        {
            string domain, pagepath;

            bool result = viewKeyProcessor.TryParseDomainAndPagePathFromVirtualPath(virtualPath, out domain, out pagepath);

            if (result)
                return repository.GetByDomainAndViewName(domain, pagepath) != null;

            return false;
        }
    }
}