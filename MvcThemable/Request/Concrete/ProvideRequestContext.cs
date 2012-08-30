using System.Web;
using MvcThemable.Request.Abstract;

namespace MvcThemable.Request.Concrete
{
    public class ProvideRequestContext : IProvideCurrentRequestContext
    {
        public string CurrentHost { get { return HttpContext.Current.Request.Url.Host; } }
    }
}