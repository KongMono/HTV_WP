using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace News
{
    public class MediaHighlightItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public int content_id { get; set; }
        public string channel_name { get; set; }
        public string thumbnail { get; set; }
        public string thumbnail_App { get; set; }
        public string category { get; set; }
        public string rating { get; set; }
        public string view { get; set; }
        public string Share_url { get; set; }

        private string _PicSelected;
        public string PicSelected
        {
            get
            {
                return this._PicSelected;
            }
            set
            {
                if (value != this._PicSelected)
                {
                    this._PicSelected = value;
                    this.OnPropertyChanged("PicSelected");
                }
            }
        }

        public int ParentID { get; set; }
        public int ReferenceID { get; set; }
    }
}
