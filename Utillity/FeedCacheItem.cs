using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MovieTV
{
    public class FeedCacheItem
    {
        public string url { get; set; }
        public DateTime expireDate { get; set; }
    }
}
