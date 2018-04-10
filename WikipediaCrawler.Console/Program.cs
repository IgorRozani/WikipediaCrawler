using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikipediaCrawler.Console
{
    class Program
    {
        const string URL = @"https://pt.wikipedia.org/wiki/";

        static void Main(string[] args)
        {
            var origin = GetOrigin();
            var destiny = GetDestiny();

            var links = GetLinks(origin);

            if (links.Contains(destiny))
                System.Console.WriteLine($"{origin} > {destiny}");
            else
            {
                foreach (var link in links)
                {
                    var sublinks = GetLinks(link);
                    if (sublinks.Contains(destiny))
                    {
                        System.Console.WriteLine($"{origin} > {link} > {destiny}");
                    }
                }
            }

            System.Console.ReadLine();
        }

        private static string GetOrigin()
        {
            System.Console.WriteLine("Origin:");
            return System.Console.ReadLine();
        }

        private static string GetDestiny()
        {
            System.Console.WriteLine("Destiny:");
            return System.Console.ReadLine();
        }

        private static List<string> GetLinks(string subDomain)
        {
            var fullUrl = $"{URL}{subDomain}";

            var htmlWeb = new HtmlWeb();
            var htmlPage = htmlWeb.Load(fullUrl);

            var content = htmlPage.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[3]/div[3]");

            var links = content.SelectNodes("//a[@href]");

            var linksFromPage = new List<string>();
            foreach (var link in links)
            {
                var hrefValue = link.GetAttributeValue("href", string.Empty);

                if (hrefValue.StartsWith(@"/wiki/") && !hrefValue.Contains(":"))
                    linksFromPage.Add(hrefValue.Replace(@"/wiki/", string.Empty));
            }

            return linksFromPage.Distinct().ToList();
        }
    }
}
