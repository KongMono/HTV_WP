using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace News
{
    public class ChapterArchivePageItem
    {


        public int chapter_id { get; set; }
        public string chapter_title { get; set; }
        public string thumbnail { get; set; }
        public string parent_id { get; set; }

        public ChapterArchivePageItem()
        {
        }
        public ChapterArchivePageItem(int chapter_id, string chapter_title, string thumbnail, string parent_id)
        {
            this.chapter_id = chapter_id;
            this.chapter_title = chapter_title;
            this.thumbnail = thumbnail;
            this.parent_id = parent_id;
        }

    }
}
