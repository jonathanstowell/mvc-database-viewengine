using System.Web.Routing;
using MvcThemable.Installation;

namespace MvcThemeable.Plugins.Carousel.Installers
{
    public class RegisterWidget : IRegisterWidget
    {
        public string Name { get { return "Carousel"; } }
        public string Description { get { return "A revolving set of images"; } }
        public string Controller { get { return "Carousel"; } }
        public string Action { get { return "Index"; } }

        public void Routes(RouteTable routeTable)
        {
            throw new System.NotImplementedException();
        }
    }
}
