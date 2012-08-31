using System.Web;
using System.Web.Mvc;

namespace MvcThemable.ViewPipeline.ViewLocationCache
{
    public class NullViewLocationCache : IViewLocationCache
    {
        public string GetViewLocation(HttpContextBase httpContext, string key)
        {
            return null;
        }

        public void InsertViewLocation(HttpContextBase httpContext, string key, string virtualPath)
        {
            
        }
    }
}
