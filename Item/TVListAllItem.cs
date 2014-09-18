using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace News
{
    public class TVListAllItem
    {
        public int content_id { get; set; }
        public string channel_name { get; set; }
        public string thumbnail { get; set; }
        public string title { get; set; }
        public string view { get; set; }
        public string Share_url { get; set; }

        public TVListAllItem(int content_id, string channel_name, string thumbnail, string title, string view, string Share_url)
        {
            this.content_id = content_id;
            this.channel_name = channel_name;
            this.thumbnail = thumbnail;
            this.title = title;
            this.view = view;
            this.Share_url = Share_url;
        }

        public TVListAllItem()
        {
            // TODO: Complete member initialization
        }
             


    }
}
