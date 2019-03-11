using AngleSharp.Html.Parser;
using PortfolioWebApi.Models;
using PortfolioWebApi.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace PortfolioWebApi.Services
{
    public class ScraperServices
    {
        public List<Tip> Scrape()
        {
            var tips = new List<Tip>();
            var client = new WebClient();
            var html = client.DownloadString("https://www.epicurious.com/expert-advice/best-cooking-advice-ever-article");
            var parser = new HtmlParser();
            var document = parser.ParseDocument(html);
            var bodies = document.QuerySelectorAll("p");
            var titles = document.QuerySelectorAll("h4");
            // counter for p elements
            int j = 1;
            // Loop Over array of questions.
            for (int i = 0; i < titles.Length; i++)
            {
                var tip = new Tip();
                tip.Title = titles[i].TextContent;
                tip.Body = bodies[j].TextContent;
                j += 1;
                tips.Add(tip);
            }
            return tips;
        }
    }
}