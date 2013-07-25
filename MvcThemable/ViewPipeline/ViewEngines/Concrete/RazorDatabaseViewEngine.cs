using System.Web.Mvc;
using MvcThemable.Data.Abstract;
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

        private readonly IDatabaseViewRepository repository;

        public RazorDatabaseViewEngine()
            : this(null)
        {}

        public RazorDatabaseViewEngine(IViewPageActivator viewPageActivator)
            : base(viewPageActivator)
        {
            viewLocations = DependencyResolver.Current.GetService<IProvideViewLocations>();
            viewKeyProcessor = DependencyResolver.Current.GetService<IViewKeyProcessor>();
            repository = DependencyResolver.Current.GetService<IDatabaseViewRepository>();

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