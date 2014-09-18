using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace News
{
    public class ToolItem
    {
        public int id { get; set; }
        public string image_path { get; set; }
        public string title { get; set; }

        public ToolItem(int id, string image_path, string title)
        {
            this.id = id;
            this.image_path = image_path;
            this.title = title;
        }
        public ToolItem()
        {
        }
    }
}
