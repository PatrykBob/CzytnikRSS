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
            using (var db = new LiteDatabase(@"D:\dane.db"))
            {
                var col = db.GetCollection<Site>("sites");

                col.Insert(site);               
            }
        }
    }
}