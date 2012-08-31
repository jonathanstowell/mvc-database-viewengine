using System.Web.Mvc;
using MvcThemable.Data.Abstract;
using MvcThemable.Data.Concrete;
using MvcThemable.ViewPipeline.ViewEngines.Abstract;
using MvcThemable.ViewPipeline.ViewKeyProcessors.Abstract;
using MvcThemable.ViewPipeline.ViewKeyProcessors.Concrete;
using MvcThemable.ViewPipeline.ViewLocationCache;
using MvcThemable.ViewPipeline.ViewLocations.Abstract;
using MvcThemable.ViewPipeline.ViewLocations.Concrete;

namespace MvcThemable.ViewPipeline.ViewEngines.Concrete
{
    public class RazorDatabaseViewEngine : RazorViewEngine, IUrlThemableViewEngine
    {
        private readonly IProvideViewLocations viewLocations;
        private readonly IViewKeyProcessor viewKeyProcessor;

        private readonly IDatabaseViewRepository repository;

        public RazorDatabaseViewEngine()
            : this(null)
        {}

        public RazorDatabaseViewEngine(IViewPageActivator viewPageActivator)
            : this(viewPageActivator, new ProvideViewLocations(), new ViewKeyProcessor(), new DatabaseViewRepository())
        {}

        public RazorDatabaseViewEngine(IViewPageActivator viewPageActivator, IProvideViewLocations viewLocations, IViewKeyProcessor viewKeyProcessor, IDatabaseViewRepository repository)
            : base(viewPageActivator)
        {
            this.viewLocations = viewLocations;
            this.viewKeyProcessor = viewKeyProcessor;
            this.repository = repository;

            FileExtensions = new[] { "dbhtml" };

            ViewLocationCache = new NullViewLocationCache();
        }

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            return DatabaseViewExists(virtualPath);
        }

        public bool DatabaseViewExists(string virtualPath)
        {
            string key = viewKeyProcessor.Retrieve(virtualPath);

            if (!string.IsNullOrEmpty(key))
                return repository.GetByViewKey(key) != null;

            return false;
        }

        public void SetViewLocations()
        {
            ViewLocationFormats = viewLocations.ViewLocations("dbhtml");
            PartialViewLocationFormats = viewLocations.ParitalViewLocations("dbhtml");
        }
    }
}