using CzytnikRSS.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
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
            XElement rss = XElement.Load("http://windows.wvista.pl/rss.xml");
            
                foreach (var item in rss.Descendants("item"))
                {
                    var site = new Site
                    {
                        title = item.Element("title").Value,
                        description = item.Element("description").Value,
                        pubDate = DateTime.Now
                    };
                    dbController.ZapiszDoBazy(site);
                }

            return View();
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