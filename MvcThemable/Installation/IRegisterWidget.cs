using System.Web.Routing;

namespace MvcThemable.Installation
{
    public interface IRegisterWidget
    {
        string Name { get; }
        string Description { get; }
        string Controller { get; }
        string Action { get; }
        void Routes(RouteTable routeTable);
    }
}
