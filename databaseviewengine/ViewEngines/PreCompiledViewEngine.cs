using System.Collections.Generic;
using System.Web.Mvc;
using databaseviewengine.RequestContext;
using databaseviewengine.ViewKeyProcessors;

namespace databaseviewengine.ViewEngines
{
    public class PreCompiledViewEngine : VirtualPathProviderViewEngine
    {
        private readonly IDictionary<string, WebViewPage> precompiledViews;
        private readonly ProvideRequestContext requestContext;
        private readonly PrecompiledViewKeyProcessor viewKeyProcessor;

        public PreCompiledViewEngine(IDictionary<string, WebViewPage> precompiledViews)
        {
            requestContext = new ProvideRequestContext();
            this.precompiledViews = precompiledViews;
            viewKeyProcessor = new PrecompiledViewKeyProcessor();

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

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            WebViewPage type;
            return precompiledViews.TryGetValue(viewKeyProcessor.GetPrecompiledKey(virtualPath), out type);
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return GetPrecompiledView(partialPath, false);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return GetPrecompiledView(viewPath, true);
        }

        private PrecompiledView GetPrecompiledView(string virtualPath, bool runViewStart)
        {
            WebViewPage type;

            bool item = precompiledViews.TryGetValue(viewKeyProcessor.GetPrecompiledKey(virtualPath), out type);

            if (!item)
                return null;

            return new PrecompiledView(virtualPath, type, runViewStart, FileExtensions);
        }
    }
}