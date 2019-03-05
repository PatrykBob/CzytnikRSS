using CzytnikRSS.Models;
using HtmlAgilityPack;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace CzytnikRSS.Controllers
{
    public class HomeController : Controller
    {
        DatabaseController dbController = new DatabaseController();
        public ActionResult Index()
        {
            List<Site> lista = new List<Site>();

            PobierzLinkiStron();

            return View();
        }

        public void PobierzLinkiStron()
        {
            string link = "http://www.rss.lostsite.pl/index.php?rss=32";

            using (var client = new WebClient())
            {
                string html = client.DownloadString(link);

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                List<HtmlAttribute> lista = new List<HtmlAttribute>();
                foreach(HtmlNode lin in doc.DocumentNode.SelectNodes("//a[@href]"))
                {
                    //HtmlAttribute href = link.Attributes["href"];
                    HtmlAttribute rel = lin.Attributes["rel"];
                    HtmlAttribute id = lin.Attributes["id"]; 
                    if (rel != null && id == null)
                    {
                        if (lin.Attributes["rel"].Value == "nofollow")
                        {
                            Source l = new Source()
                            {
                                name = "test",
                                link = lin.Attributes["href"].Value
                            };
                            dbController.ZapiszLinkDoBazy(l);
                        }
                    }
                }
            }
        }

        public void PobierzElementyZeStrony (string link)
        {
            XElement rss = XElement.Load(link);

            foreach (var item in rss.Descendants("item"))
            {
                var site = new Site
                {
                    title = item.Element("title").Value,
                    description = item.Element("description").Value,
                    pubDate = DateTime.Now
                };
                dbController.ZapiszStroneDoBazy(site);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}