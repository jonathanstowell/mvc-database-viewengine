using System;

namespace databaseviewengine.ViewKeyProcessors
{
    public class DatabaseViewKeyProcessor
    {
        public bool TryParseDomainAndPagePathFromVirtualPath(string virtualPath, out string domain, out string pagePath)
        {
            string[] breakdown = BreakdownVirtualPathIntoDomainControllerAndPage(virtualPath);

            if (breakdown.Length < 3)
            {
                domain = string.Empty;
                pagePath = string.Empty;

                return false;
            }

            domain = breakdown[0];
            pagePath = string.Format("{0}/{1}", breakdown[1], breakdown[2]);

            return true;
        }

        private string[] BreakdownVirtualPathIntoDomainControllerAndPage(string virtualPath)
        {
            int startIndex = virtualPath.LastIndexOf("Views/", StringComparison.Ordinal);

            if (startIndex == -1)
                return new string[0];

            string sub = virtualPath.Replace(".dbhtml", "");

            sub = sub.Substring(startIndex + 6, sub.Length - (startIndex + 6));

            return sub.Split('/');
        }
    }
}