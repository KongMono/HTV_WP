using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace News
{
    public class ArchivePageItem
    {
        public int content_id { get; set; }
        public string content_title { get; set; }
        public string description { get; set; }
        public string stream { get; set; }
        public string thumbnail { get; set; }
        public string parent_id { get; set; }
        public string share_url { get; set; }
        public string view { get; set; }
        public string embed_url { get; set; }
        public string stream_id { get; set; }
        public List<ChapterArchivePageItem> ChapterArchivePageDetailList { get; set; }
    }
}
