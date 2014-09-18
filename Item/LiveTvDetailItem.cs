using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace News
{
    public class LiveTvDetailItem
    {
        public int ContentID { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string Detail { get; set; }
        public string Link { get; set; }
        public double Rating { get; set; }
        public string StreamID { get; set; }
        public StreamingItem StreamingData { get; set; }
       
    }
}
