using CzytnikRSS.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CzytnikRSS.Controllers
{
    public class DatabaseController : Controller
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "/dane.db";
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

        public List<Source> PobierzLinkiZBazy()
        {
            using (var db = new LiteDatabase(path))
            {
                var col = db.GetCollection<Source>("links");

                return col.FindAll().ToList();
            }
        }
        // GET: Database
        public void ZapiszStroneDoBazy(Site site)
        {
            using (var db = new LiteDatabase(path))
            {
                var col = db.GetCollection<Site>("sites");

                var exist = col.Exists(Query.EQ("title", site.title));

                if(!exist)
                    col.Insert(site);               
            }
        }
    }
}