using System.Collections.Generic;
using MvcThemable.Request.Abstract;
using MvcThemable.ViewPipeline.ViewLocations.Abstract;

namespace MvcThemable.ViewPipeline.ViewLocations.Concrete
{
    using System.Web.Mvc;

    public class ProvideViewLocations : IProvideViewLocations
    {
        private readonly IProvideCurrentRequestContext currentRequestContext;

        public ProvideViewLocations()
        {
            currentRequestContext = DependencyResolver.Current.GetService<IProvideCurrentRequestContext>();
        }

        public string[] ViewLocations(params string[] extensions)
        {
            return Process(extensions);
        }

        public string[] ParitalViewLocations(params string[] extensions)
        {
            return Process(extensions);
        }

        private string[] Process(params string[] extensions)
        {
            var ret = new List<string>();

            foreach (var extension in extensions)
            {
                ret.AddRange(new[]
                {
                    "~/Plugins/Views/" + currentRequestContext.CurrentHost + "/{1}/{0}." + extension,
                    "~/Plugins/Views/default/{1}/{0}." + extension,
                    "~/Views/" + currentRequestContext.CurrentHost + "/{1}/{0}." + extension,
                    "~/Views/default/{1}/{0}." + extension,
                    "~/Plugins/Views/" + currentRequestContext.CurrentHost + "/Shared/{0}." + extension,
                    "~/Plugins/Views/default/Shared/{0}." + extension,
                    "~/Views/" + currentRequestContext.CurrentHost + "/Shared/{0}." + extension,
                    "~/Views/default/Shared/{0}." + extension
                                    
                });
            }

            return ret.ToArray();
        }
    }
}