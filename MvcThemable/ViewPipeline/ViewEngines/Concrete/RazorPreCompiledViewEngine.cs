using System.Collections.Generic;
using System.Web.Mvc;
using MvcThemable.ViewPipeline.ViewEngines.Abstract;
using MvcThemable.ViewPipeline.ViewKeyProcessors.Abstract;
using MvcThemable.ViewPipeline.ViewKeyProcessors.Concrete;
using MvcThemable.ViewPipeline.ViewLocations.Abstract;
using MvcThemable.ViewPipeline.ViewLocations.Concrete;
using MvcThemable.ViewPipeline.Views;

namespace MvcThemable.ViewPipeline.ViewEngines.Concrete
{
    public class RazorPreCompiledViewEngine : VirtualPathProviderViewEngine, IUrlThemableViewEngine
    {
        private readonly IDictionary<string, WebViewPage> precompiledViews;
        private readonly IProvideViewLocations viewLocations;
        private readonly IViewKeyProcessor viewKeyProcessor;

        public RazorPreCompiledViewEngine(IDictionary<string, WebViewPage> precompiledViews)
            : this(precompiledViews, new ProvideViewLocations(), new ViewKeyProcessor())
        {
        }

        public RazorPreCompiledViewEngine(IDictionary<string, WebViewPage> precompiledViews, IProvideViewLocations viewLocations, IViewKeyProcessor viewKeyProcessor)
        {
            this.precompiledViews = precompiledViews;

            this.viewLocations = viewLocations;
            this.viewKeyProcessor = viewKeyProcessor;

            FileExtensions = new[] { "cshtml" };
        }

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            WebViewPage type;
            return precompiledViews.TryGetValue(viewKeyProcessor.Retrieve(virtualPath), out type);
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

            bool item = precompiledViews.TryGetValue(viewKeyProcessor.Retrieve(virtualPath), out type);

            if (!item)
                return null;

            return new PrecompiledView(virtualPath, type, runViewStart, FileExtensions);
        }

        public void SetViewLocations()
        {
            ViewLocationFormats = viewLocations.ViewLocations("cshtml");
            PartialViewLocationFormats = viewLocations.ParitalViewLocations("cshtml");
        }
    }
}