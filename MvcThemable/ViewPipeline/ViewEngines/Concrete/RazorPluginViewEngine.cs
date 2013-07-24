using System.Web.Mvc;
using MvcThemable.ViewPipeline.ViewEngines.Abstract;
using MvcThemable.ViewPipeline.ViewLocationCache;
using MvcThemable.ViewPipeline.ViewLocations.Abstract;

namespace MvcThemable.ViewPipeline.ViewEngines.Concrete
{
    public class RazorPluginViewEngine : RazorViewEngine, IUrlThemableViewEngine
    {
        private readonly IProvideViewLocations viewLocations;

        public RazorPluginViewEngine()
            : this(null)
        {}

        public RazorPluginViewEngine(IViewPageActivator viewPageActivator)
            : base(viewPageActivator)
        {
            viewLocations = DependencyResolver.Current.GetService<IProvideViewLocations>();

            FileExtensions = new[] { "cshtml" };

            ViewLocationCache = new NullViewLocationCache();
        }

        public void SetViewLocations()
        {
            ViewLocationFormats = viewLocations.ViewLocations("cshtml");
            PartialViewLocationFormats = viewLocations.ParitalViewLocations("cshtml");
        }
    }
}