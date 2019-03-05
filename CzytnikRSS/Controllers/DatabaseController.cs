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

                col.Insert(link);
            }
        }
        // GET: Database
        public void ZapiszStroneDoBazy(Site site)
        {
            using (var db = new LiteDatabase(path))
            {
                var col = db.GetCollection<Site>("sites");

                col.Insert(site);               
            }
        }
    }
}