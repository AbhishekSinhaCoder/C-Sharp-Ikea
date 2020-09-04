using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IkeaCataloger {
    class Program {
        static void Main (string[] args) {
            var regex = new Regex ("\"downloadPdfUrl\":\"([^\"]+)\"", RegexOptions.Compiled);
            Parallel.ForEach (Enumerable.Range (1950, DateTime.Now.Year + 1 - 1950), new ParallelOptions () { MaxDegreeOfParallelism = 8 }, year => {
                using (var wc = new WebClient ()) {
                    var pageData = wc.DownloadString ($"https://ikeacatalogues.ikea.com/sv-{year}/page/1");
                    var matches = regex.Matches (pageData);
                    if (matches.Count > 0) {
                        Console.WriteLine ($"Downloading {matches[0].Groups[1].Value} to Ikea -{ year}.pdf");
                        wc.DownloadFile (matches[0].Groups[1].Value, $"Ikea-{year}.pdf");
                    }
                }
            });
        }
    }
}