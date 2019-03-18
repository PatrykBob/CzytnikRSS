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

            List<Source> linki = dbController.PobierzLinkiZBazy();

            foreach (Source link in linki)
            {
                PobierzElementyZeStrony(link.link);
            }

            //Pobieranie z bazy i wrzucanie do widoku/indexu
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "/dane.db";
            using (var db = new LiteDatabase(path))
            {
                var col = db.GetCollection<Source>("links");

                var query = col.FindAll().Select(item =>
                        new Source
                        {
                            id = item.id,
                            link = item.link
                        });
                var items = query.ToList();

                return View(items);
            }


            //return View();
        }

        public ActionResult PokazStrony(string link)
        {
            return (View(dbController.PobierzStronyWgLinku(link)));
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
                foreach (HtmlNode lin in doc.DocumentNode.SelectNodes("//a[@href]"))
                {
                    HtmlAttribute rel = lin.Attributes["rel"];
                    HtmlAttribute id = lin.Attributes["id"];
                    if (rel != null && id == null)
                    {
                        if (lin.Attributes["rel"].Value == "nofollow")
                        {
                            Source l = new Source()
                            {
                                link = lin.Attributes["href"].Value
                            };
                            dbController.ZapiszLinkDoBazy(l);
                        }
                    }
                }
            }
        }

        public bool CzyStronaOnline(string link)
        {
            try
            {
                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(link);
                httpReq.AllowAutoRedirect = false;

                HttpWebResponse httpRes = (HttpWebResponse)httpReq.GetResponse();

                if (httpRes.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }

                // Close the response.
                httpRes.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public void PobierzElementyZeStrony(string link)
        {
            if (CzyStronaOnline(link))
            {
                try
                {
                    XElement rss = XElement.Load(link);
                    foreach (var item in rss.Descendants("item"))
                    {
                        var site = new Site
                        {
                            title = item.Element("title").Value,
                            description = item.Element("description").Value,
                            pubDate = DateTime.Now,
                            link = link
                        };
                        dbController.ZapiszStroneDoBazy(site);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
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