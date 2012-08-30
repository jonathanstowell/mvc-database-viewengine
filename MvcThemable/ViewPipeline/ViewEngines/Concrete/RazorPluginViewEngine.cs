using System.Web.Mvc;
using MvcThemable.ViewPipeline.ViewEngines.Abstract;
using MvcThemable.ViewPipeline.ViewLocations.Abstract;
using MvcThemable.ViewPipeline.ViewLocations.Concrete;

namespace MvcThemable.ViewPipeline.ViewEngines.Concrete
{
    public class RazorPluginViewEngine : RazorViewEngine, IUrlThemableViewEngine
    {
        private readonly IProvideViewLocations viewLocations;

        public RazorPluginViewEngine()
            : this(null, new ProvideViewLocations())
        {}

        public RazorPluginViewEngine(IViewPageActivator viewPageActivator, IProvideViewLocations viewLocations)
            : base(viewPageActivator)
        {
            this.viewLocations = viewLocations;

            FileExtensions = new[] { "cshtml" };
        }

        public void SetViewLocations()
        {
            ViewLocationFormats = viewLocations.ViewLocations("cshtml");
            PartialViewLocationFormats = viewLocations.ParitalViewLocations("cshtml");
        }
    }
}