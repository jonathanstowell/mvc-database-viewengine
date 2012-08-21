using System.Web.Mvc;
using databaseviewengine.RequestContext;

namespace databaseviewengine.ViewEngines
{
    public class MongoViewEngine : RazorViewEngine
    {
        internal static readonly string ViewStartFileName = "~/Views/_ViewStart";

        private readonly ProvideRequestContext requestContext;

        public MongoViewEngine() : this(null)
        {
        }

        public MongoViewEngine(IViewPageActivator viewPageActivator) : base(viewPageActivator)
        {
            requestContext = new ProvideRequestContext();

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
    }
}