using System;

namespace databaseviewengine.ViewKeyProcessors
{
    public class PrecompiledViewKeyProcessor
    {
        public string GetPrecompiledKey(string virtualPath)
        {
            string domain, pagepath;

            TryParseDomainAndPagePathFromVirtualPath(virtualPath, out domain, out pagepath);

            return string.Format("{0}/{1}", domain, pagepath);
        }

        private void TryParseDomainAndPagePathFromVirtualPath(string virtualPath, out string domain, out string pagePath)
        {
            string[] breakdown = BreakdownVirtualPathIntoDomainControllerAndPage(virtualPath);

            if (breakdown.Length < 3)
            {
                domain = string.Empty;
                pagePath = string.Empty;

                return;
            }

            domain = breakdown[0];
            pagePath = string.Format("{0}/{1}", breakdown[1], breakdown[2]);
        }

        private string[] BreakdownVirtualPathIntoDomainControllerAndPage(string virtualPath)
        {
            int startIndex = virtualPath.LastIndexOf("Views/", StringComparison.Ordinal);

            if (startIndex == -1)
                return new string[0];

            string sub = virtualPath.Replace(".cshtml", "");

            sub = sub.Substring(startIndex + 6, sub.Length - (startIndex + 6));

            return sub.Split('/');
        }
    }
}