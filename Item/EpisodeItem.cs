using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace News
{
    public class EpisodeItem
    {
        public string section { get; set; }
        public int ContentID { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string parent_id { get; set; }
    }
}
