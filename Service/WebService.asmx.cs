using CzytnikRSS;
using CzytnikRSS.Models;
using HtmlAgilityPack;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Service
{
    /// <summary>
    /// Opis podsumowujący dla WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WebService : System.Web.Services.WebService
    {
        string path = "dane.db";

        [WebMethod]
        public void Odswiez()
        {
            PobierzLinkiStron();
            List<Source> linki = PobierzLinkiZBazy();
            foreach(Source link in linki)
            {
                PobierzElementyZeStrony(link.link);
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
                        var rssItem = new RssItem
                        {
                            title = item.Element("title").Value,
                            description = item.Element("description").Value,
                            pubDate = item.Element("pubDate").Value,
                            link = link
                        };
                        ZapiszStroneDoBazy(rssItem);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        public void ZapiszLinkDoBazy(Source link)
        {
            using (var db = new LiteDatabase(path))
            {
                var col = db.GetCollection<Source>("links");

                var exist = col.Exists(Query.EQ("link", link.link));

                if (!exist)
                    col.Insert(link);
            }
        }
        [WebMethod]
        public List<Source> PobierzLinkiZBazy()
        {
            using (var db = new LiteDatabase(path))
            {
                var col = db.GetCollection<Source>("links");

                return col.FindAll().ToList();
            }
        }
        [WebMethod]
        public List<RssItem> PobierzStronyWgLinku(string link)
        {
            using (var db = new LiteDatabase(path))
            {
                var col = db.GetCollection<RssItem>("rssitems");

                return col.Find(Query.EQ("link", link)).ToList();
            }
        }

        public void ZapiszStroneDoBazy(RssItem RssItem)
        {
            using (var db = new LiteDatabase(path))
            {
                var col = db.GetCollection<RssItem>("rssitems");

                var exist = col.Exists(Query.EQ("title", RssItem.title));

                if (!exist)
                    col.Insert(RssItem);
            }
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
                            ZapiszLinkDoBazy(l);
                        }
                    }
                }
            }
        }

        protected static Timer timer;
        public WebService()
        {
            if(timer == null)
            timer = new Timer(MyRoutineToCall, null, TimeSpan.FromMinutes(0), TimeSpan.FromMinutes(15));
        }

        protected void MyRoutineToCall(Object state)
        {
            Odswiez();
        }
    }
}
