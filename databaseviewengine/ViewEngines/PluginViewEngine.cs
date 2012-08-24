using System.Web.Mvc;
using databaseviewengine.RequestContext;

namespace databaseviewengine.ViewEngines
{
    public class PluginViewEngine : RazorViewEngine
    {
        private readonly ProvideRequestContext requestContext;

        public PluginViewEngine()
            : this(null)
        {}

        public PluginViewEngine(IViewPageActivator viewPageActivator)
            : base(viewPageActivator)
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

            FileExtensions = new[] { "cshtml" };
        }
    }
}