using CzytnikRSS.RssService;
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
        public WebServiceSoapClient rssService = new RssService.WebServiceSoapClient();
        public ActionResult Index()
        {
            List<Source> linki = rssService.PobierzLinkiZBazy().ToList();

            return View(linki);
        }

        public ActionResult PokazStrony(string link)
        {
            List<RssItem> items = rssService.PobierzStronyWgLinku(link).ToList();
            return View(items);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}