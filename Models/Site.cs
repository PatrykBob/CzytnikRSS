using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CzytnikRSS.Models
{
    public class Site
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime pubDate { get; set; }
    }
}