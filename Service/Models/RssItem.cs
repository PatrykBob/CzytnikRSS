using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CzytnikRSS
{
    public class RssItem
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string pubDate { get; set; }
        public string link { get; set; }
    }
}