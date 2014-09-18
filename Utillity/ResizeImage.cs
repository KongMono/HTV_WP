using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace News
{
    public class ResizeImage
    {
        //Variable
        public static ResizeImage Instance = new ResizeImage();

        public ResizeImage()
        {

        }

        public String  GetResizeUrl(int h = 179,int w = 125)
        {
            int rand = 0;
            String resize_url;
            Random n = new Random();
            rand = n.Next(4) + 1;
            resize_url = "http://dynamic.tlcdn" + rand + ".com/api/image/get?h=" + h + "&w=" + w + "&url=";

            return resize_url;
        }

        public String GetResizeAutoHeight(int width)
        {
            String resize_url = "";
            if (width != 0)
            {
                int w = width;
                int rand = 0;              
                Random n = new Random();
                rand = n.Next(4) + 1;
                resize_url = "http://dynamic.tlcdn" + rand + ".com/api/image/get?w=" + w + "&url=";
            }

            return resize_url;
        }

        public String GetResizeAutoWidth(int height)
        {
            String resize_url = "";
            if (height != 0)
            {
                int h = height;
                int rand = 0;
                Random n = new Random();
                rand = n.Next(4) + 1;
                resize_url = "http://dynamic.tlcdn" + rand + ".com/api/image/get?h=" + height + "&url=";
            }

            return resize_url;
        }


    }
}
