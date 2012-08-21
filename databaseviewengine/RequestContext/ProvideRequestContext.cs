using System.Web;

namespace databaseviewengine.RequestContext
{
    public class ProvideRequestContext
    {
        public string CurrentHost { get { return HttpContext.Current.Request.Url.Host; } }
    }
}