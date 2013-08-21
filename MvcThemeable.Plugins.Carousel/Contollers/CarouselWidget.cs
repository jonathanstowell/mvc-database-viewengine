using System.Web.Mvc;

namespace MvcThemeable.Plugins.Carousel.Contollers
{
    public class CarouselWidget : Controller
    {
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}
