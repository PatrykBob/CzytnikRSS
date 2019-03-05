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
        // GET: Database
        public void ZapiszStroneDoBazy(Site site)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString();
            path += "/dane.db";
            using (var db = new LiteDatabase(path))
            {
                var col = db.GetCollection<Site>("sites");

                col.Insert(site);               
            }
        }
    }
}