using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace News
{
    public class ChannelDetailItem
    {
        public string allowfullmovie { get; set; }
        public int content_id { get; set; }
        public string channel_title { get; set; }
        public string thumbnail { get; set; }
        public string thumbnail_app { get; set; }
        public string thumbnail_live { get; set; }
        public string detail { get; set; }
        public string link { get; set; }
        public string rating { get; set; }
        public string embed_url { get; set; }
        public string stream_id { get; set; }

        public StreamingItem StreamingData { get; set; }
        public StreamingItem Streaming_LiveData { get; set; }

        public string view { get; set; }
        public string menu_archive { get; set; }
        public string menu_synopsis { get; set; }
        public string menu_music { get; set; }
        public string menu_gallery { get; set; }
        public string menu_news { get; set; }
        public string menu_quote { get; set; }

        public string suggestion { get; set; }

    }
}
