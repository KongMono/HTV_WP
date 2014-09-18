using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Diagnostics;
using System.Xml.Linq;
using System.Windows.Media.Imaging;

namespace News
{
    public partial class LiveTvDetailPage : PhoneApplicationPage
    {
        //Variable
        LiveTvDetailItem channelItem = new LiveTvDetailItem();
        string RawImagePath = "";

        //URL
        
        //const string URL_LIVE_TV_DETAIL = "http://mstage.truelife.com/api_movietv/tv/livetvinfo?method=getinfo&content_id=";
        const string URL_LIVE_TV_DETAIL = "http://api-movie.truelife.com/wrap_api/mod/tv/livetvinfo?method=getinfo&content_id=";
        const string URL_SHARE_COMMENT = "http://mstage.truelife.com/movietv/tv";

        //Constructor
        public LiveTvDetailPage()
        {
            InitializeComponent();
        }

        //System Event
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
            {
                //ContentID
                int content_id = 0;
                if (this.NavigationContext.QueryString.ContainsKey("ContentID"))
                {
                    int.TryParse(this.NavigationContext.QueryString["ContentID"], out content_id);
                    //MylifeLike.Instance.GetCheckLike(content_id, this);
                }
                else
                {
                    Debug.WriteLine("not found ContentID");
                }

                //CategoryName
                if (this.NavigationContext.QueryString.ContainsKey("CategoryName"))
                {
                    channelPivot.Title = "LIVE TV, " + this.NavigationContext.QueryString["CategoryName"].ToUpper();
                }
                else
                {
                    Debug.WriteLine("not found CategoryName");
                }

                //call data
                if (content_id > 0)
                {
                    CallLiveTvDetail(content_id);
                }
                else
                {
                    Debug.WriteLine("not support ContentID : " + this.NavigationContext.QueryString["ContentID"]);
                }
            }

            //-----
            base.OnNavigatedTo(e);
        }

        //Command
        private void CallLiveTvDetail(int ContentID)
        {
            SetLoadingPanelVisibility(true);

            string url = "";
            url = URL_LIVE_TV_DETAIL + ContentID;

            WebClient channelDetailWebClient = new WebClient();
            channelDetailWebClient.DownloadStringCompleted += channelDetailWebClient_DownloadStringCompleted;
            channelDetailWebClient.DownloadStringAsync(new Uri(url, UriKind.Absolute));
        }

        //GUI
        private void SetLoadingPanelVisibility(bool isVisible)
        {
            if (isVisible)
            {
                LoadingPanel.Visibility = Visibility.Visible;
                loadingProgressBar.IsIndeterminate = true;

                ApplicationBar.IsVisible = false;
            }
            else
            {
                LoadingPanel.Visibility = Visibility.Collapsed;
                loadingProgressBar.IsIndeterminate = false;

                ApplicationBar.IsVisible = true;
            }
        }

        private void SetChannelDetail(LiveTvDetailItem item)
        {
            titleLabel.Text = item.Title;
            channelImage.Source = new BitmapImage(new Uri(item.ImagePath, UriKind.Absolute));
            detailLabel.Text = HttpUtility.HtmlDecode(item.Detail);

            SetLoadingPanelVisibility(false);
        }

        //Event
        void channelDetailWebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    throw new Exception(e.Error.Message);
                }
                //----------
                XDocument xdoc = XDocument.Parse(e.Result, LoadOptions.None);
                if (xdoc.Root == null)
                {
                    throw new Exception("Root is null");
                }

                if (xdoc.Root.Element("status_code") == null)
                {
                    throw new Exception("status_code is null");
                }

                if (xdoc.Root.Element("entry") == null)
                {
                    throw new Exception("entry is null");
                }
                //----------
                if (xdoc.Root.Element("status_code").Value != "200")
                {
                    throw new Exception("code is " + xdoc.Root.Element("status_code").Value + " ~ " + xdoc.Root.Element("status_txt").Value);
                }
                else
                {
                    if (xdoc.Root.Element("entry").Value == "")
                    {
                        MessageBox.Show("ไม่พบข้อมูล กรุณาลองใหม่อีกครั้งภายหลัง");
                        this.NavigationService.GoBack();
                    }

                    var item = xdoc.Root.Element("entry");

                    //parse
                    channelItem.ContentID = XmlValueParser.ParseInteger(item.Element("content_id"));
                    channelItem.Title = XmlValueParser.ParseString(item.Element("channel_title"));
                    channelItem.ImagePath = ResizeImage.Instance.GetResizeUrl(92, 128) + XmlValueParser.ParseString(item.Element("thumbnail"));
                    RawImagePath = XmlValueParser.ParseString(item.Element("thumbnail"));
                    channelItem.Detail = XmlValueParser.ParseString(item.Element("detail"));
                    channelItem.Link = XmlValueParser.ParseString(item.Element("link"));
                    channelItem.Rating = XmlValueParser.ParseDouble(item.Element("rating")) / 10;//change max value from 100 to 10
                    channelItem.StreamID = XmlValueParser.ParseString(item.Element("stream_id"));

                    channelItem.StreamingData = new StreamingItem();
                    if (item.Element("stream") != null)
                    {
                        channelItem.StreamingData.ProjectID = XmlValueParser.ParseInteger(item.Element("stream").Element("project_id"));
                        channelItem.StreamingData.Scope = XmlValueParser.ParseString(item.Element("stream").Element("scope"));
                        channelItem.StreamingData.ContentType = XmlValueParser.ParseString(item.Element("stream").Element("content_type"));
                        channelItem.StreamingData.ContentID = XmlValueParser.ParseString(item.Element("stream").Element("content_id"));
                        //channelItem.StreamingData.ContentID = channelItem.StreamID;
                        channelItem.StreamingData.ProductID = XmlValueParser.ParseInteger(item.Element("stream").Element("product_id"));
                        channelItem.StreamingData.LifeStyle = XmlValueParser.ParseString(item.Element("stream").Element("lifestyle"));
                    }

                    //set data
                    SetChannelDetail(channelItem);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OnDemandDetailPage : movieDetailWebClient_DownloadStringCompleted ; " + ex.Message);
            }
        }

        void stream_GetLiveStreamUrlCompleted(LiveStreamEventArgs e)
        {
            try
            {
                (Application.Current as App).SmoothStreamURL = e.Result;
                Debug.WriteLine(e.Result);
                this.NavigationService.Navigate(new Uri("/PlayerPage.xaml", UriKind.Relative));
            }
            catch
            {
                throw new Exception("GetLiveStreamUrl error");
            }
           
        }

        
        private void playStreamingButton_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            LiveStream stream = new LiveStream();
            stream.GetLiveStreamUrlCompleted += stream_GetLiveStreamUrlCompleted;
            stream.GetLiveStreamUrlAsyn(channelItem.StreamingData);
        }

        private void ApplicationBarIconButton_Click_2(object sender, EventArgs e)
        {
            string url = "/ShareContentPage.xaml";
            url += "?content_id=" + channelItem.ContentID;
            url += "&title=" + HttpUtility.UrlEncode(channelItem.Title);
            url += "&description=" + HttpUtility.UrlEncode(channelItem.Detail);
            url += "&url=" + HttpUtility.UrlEncode(URL_SHARE_COMMENT + channelItem.ContentID);
            url += "&img=" + HttpUtility.UrlEncode(RawImagePath);

            this.NavigationService.Navigate(new Uri(url, UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click_3(object sender, EventArgs e)
        {
            string url = "/RatingPage.xaml?ContentID=";
            url += channelItem.ContentID;
            url += "&MediaName=" + HttpUtility.UrlEncode(channelItem.Title);
            url += "&ImageUrl=" + HttpUtility.UrlEncode(RawImagePath);
            url += "&PageTitle=" + HttpUtility.UrlEncode("LIVE TV");
            //-----
            this.NavigationService.Navigate(new Uri(url, UriKind.Relative));
        }

    }
}