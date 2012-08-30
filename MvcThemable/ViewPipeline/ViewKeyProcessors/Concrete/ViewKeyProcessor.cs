using System;
using MvcThemable.ViewPipeline.ViewKeyProcessors.Abstract;

namespace MvcThemable.ViewPipeline.ViewKeyProcessors.Concrete
{
    public class ViewKeyProcessor : IViewKeyProcessor
    {
        public string Generate(string domain, string controller, string action)
        {
            return GetKey(domain, controller, action);
        }

        public string Retrieve(string virtualPath)
        {
            string domain;
            string controller;
            string action;

            var result = TryParse(virtualPath, out domain, out controller, out action);

            if (!result)
                return string.Empty;

            return GetKey(domain, controller, action);
        }

        private string GetKey(string domain, string controller, string action)
        {
            return string.Format("{0}.{1}.{2}", domain, controller, action);
        }

        private bool TryParse(string virtualPath, out string domain, out string controller, out string action)
        {
            domain = string.Empty;
            controller = string.Empty;
            action = string.Empty;

            string[] breakdown = BreakdownVirtualPathIntoDomainControllerAndPage(virtualPath);

            if (breakdown.Length < 3)
                return false;

            domain = breakdown[0];
            controller = breakdown[1];
            action = breakdown[2];

            return true;
        }

        private string[] BreakdownVirtualPathIntoDomainControllerAndPage(string virtualPath)
        {
            int startIndex = virtualPath.LastIndexOf("Views/", StringComparison.Ordinal);

            if (startIndex == -1)
                return new string[0];

            int fileExtPos = virtualPath.LastIndexOf(".", StringComparison.Ordinal);

            if (fileExtPos == -1)
                return new string[0];

            string sub = virtualPath.Substring(0, fileExtPos);

            sub = sub.Substring(startIndex + 6, sub.Length - (startIndex + 6));

            return sub.Split('/');
        }
    }
}