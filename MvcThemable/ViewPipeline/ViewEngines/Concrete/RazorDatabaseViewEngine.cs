using System.Web.Mvc;
using MvcThemable.Service.Abstract;
using MvcThemable.ViewPipeline.ViewEngines.Abstract;
using MvcThemable.ViewPipeline.ViewKeyProcessors.Abstract;
using MvcThemable.ViewPipeline.ViewLocationCache;
using MvcThemable.ViewPipeline.ViewLocations.Abstract;

namespace MvcThemable.ViewPipeline.ViewEngines.Concrete
{
    public class RazorDatabaseViewEngine : RazorViewEngine, IUrlThemableViewEngine
    {
        private readonly IProvideViewLocations viewLocations;
        private readonly IViewKeyProcessor viewKeyProcessor;

        private readonly IDatabaseViewService service;

        public RazorDatabaseViewEngine()
            : this(null)
        {}

        public RazorDatabaseViewEngine(IViewPageActivator viewPageActivator)
            : base(viewPageActivator)
        {
            viewLocations = DependencyResolver.Current.GetService<IProvideViewLocations>();
            viewKeyProcessor = DependencyResolver.Current.GetService<IViewKeyProcessor>();
            service = DependencyResolver.Current.GetService<IDatabaseViewService>();

            FileExtensions = new[] { "dbhtml" };

            ViewLocationCache = new NullViewLocationCache();
        }

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            var ret = DatabaseViewExists(virtualPath);
            return ret;
        }

        public bool DatabaseViewExists(string virtualPath)
        {
            string key = viewKeyProcessor.Retrieve(virtualPath);

            if (!string.IsNullOrEmpty(key))
                return service.GetByViewKey(key) != null;

            return false;
        }

        public void SetViewLocations()
        {
            ViewLocationFormats = viewLocations.ViewLocations("dbhtml");
            PartialViewLocationFormats = viewLocations.ParitalViewLocations("dbhtml");
        }
    }
}