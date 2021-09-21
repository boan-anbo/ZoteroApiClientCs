using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using ZoteroApiClientCs.ZoteroApiEntities;

namespace ZoteroApiClientCs.Utils
{
    public class ZoteroResponseHeaderReader
    {
        private HttpResponseHeaders _headers;

        public ZoteroResponseHeaderReader(HttpResponseMessage response)
        {
            _headers = response.Headers;

            // Load results.
            _headers.TryGetValues("Total-Results", out IEnumerable<string> totalResults);
            var firstTotalResult = totalResults?.FirstOrDefault();
            TotalResults = firstTotalResult != null ? int.Parse(firstTotalResult) : null;

            _headers.TryGetValues("Link", out IEnumerable<string> links);
            Links = links?.First();

            if (Links?.Length > 0)
            {
                this.LoadLinks(Links);
            }
        }

        public int? TotalResults { get; set; } = null;
        public string? Links { get; set; } = null;

        public string? Next { get; set; } = null;
        public string? Prev { get; set; } = null;
        public string? First { get; set; } = null;
        public string? Last { get; set; } = null;
        public string? Alternate { get; set; } = null;

        private void LoadLinks(string linksString)
        {
            var links = linksString.Split(",");
            foreach (var link in links)
            {
                if (link.Contains("rel=\"next\""))
                {
                    Next = GetLink(link);
                }

                if (link.Contains("rel=\"last\""))
                {
                    Last = GetLink(link);
                }

                if (link.Contains("rel=\"prev\""))
                {
                    Prev = GetLink(link);
                }

                if (link.Contains("rel=\"first\""))
                {
                    First = GetLink(link);
                }

                if (link.Contains("rel=\"alternate\""))
                {
                    Alternate = GetLink(link);
                }
            }
        }

        private string GetLink(string linkItemString)
        {
            var linkItemArray = linkItemString.Split(";");
            var link = linkItemArray[0] ?? "";
            var message = linkItemArray[1] ?? "";
            var pureLink = Regex.Replace(link, @"[<>]", String.Empty);
            return pureLink.Trim();
        }
    }
}