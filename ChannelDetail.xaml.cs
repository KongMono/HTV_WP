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

// Using for xml and json
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.IO;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Phone.Shell;
using System.Windows.Controls.Primitives;

namespace News
{
    public partial class ChannelDetail : PhoneApplicationPage
    {
        public string Page = null;
        LiveStream stream = new LiveStream();
        ChannelDetailItem channelItem = new ChannelDetailItem();
        public List<string> listMenuName = new List<string>();
        public List<string> listMenu = new List<string>();
        public List<ToolItem> toolItemlist = new List<ToolItem>();
        public string content_id = null;
        public string Active = "1";
        ObservableCollection<ChannelDetailItem> ChannellList = new ObservableCollection<ChannelDetailItem>();
        const string URL_SHARE = "http://mstage.truelife.com/movietv/eupdate/detail/";
        const string UrlApiDetail = "http://mstage.truelife.com/api_movietv/tv/livetvinfo?method=getinfo&content_id=";
        string menu1, menu2, menu3, menu4, menu5, menu6;
        ProgressIndicator progressIndicator = new ProgressIndicator();
        public ChannelDetail()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
    
            if ((e.Orientation & PageOrientation.Portrait) == (PageOrientation.Portrait))
            {
                textHead.Visibility = Visibility.Visible;
                ToolBox.Visibility = Visibility.Visible;
                stackPanel.Visibility = Visibility.Visible;
                Debug.WriteLine("Portrait");
            }
            else
            {
                textHead.Visibility = Visibility.Collapsed;
                ToolBox.Visibility = Visibility.Collapsed;
                stackPanel.Visibility = Visibility.Collapsed;
                Debug.WriteLine("Landscape");
            }
        }

        private void ShowProgressIndicator(String msg)
        {
            if (progressIndicator == null)
            {
                progressIndicator = new ProgressIndicator();
                progressIndicator.IsIndeterminate = true;
            }
            SystemTray.Opacity = 0;
            progressIndicator.Text = msg;
            progressIndicator.IsVisible = true;
            progressIndicator.IsIndeterminate = true;
            SystemTray.SetProgressIndicator(this, progressIndicator);
        }


        private void HideProgressIndicator()
        {
            progressIndicator.IsVisible = false;
            progressIndicator.IsIndeterminate = false;
            SystemTray.SetProgressIndicator(this, progressIndicator);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            content_id = this.NavigationContext.QueryString["ContentID"];

            Page = this.NavigationContext.QueryString["Page"];

            if (content_id != null)
            {
                this.read_api(content_id);
            }
        }

        public void read_api(string content_id)
        {
            WebClient myService = new WebClient();

            try
            {
                if (content_id != null)
                {
                    ShowProgressIndicator("Loading...");

                    Debug.WriteLine(UrlApiDetail + content_id);

                    myService.DownloadStringAsync(new Uri(UrlApiDetail + content_id));
                    myService.DownloadStringCompleted += new DownloadStringCompletedEventHandler(GetTotal_Completed);
                }
                else
                {
                    throw new Exception("Content id is NULL !");
                }

            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void GetTotal_Completed(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    throw new Exception(e.Error.Message);
                }

                XDocument o = XDocument.Parse(e.Result, LoadOptions.None);

               
                if (o.Root.Element("status_code").Value != "200")
                {
                    throw new Exception("code is " + o.Root.Element("status_code").Value + " ~ " + o.Root.Element("status_txt").Value);
                }
                if (o.Root.Element("status_code").Value == "200")
                {
                    channelItem.suggestion = o.Root.Element("suggestion").Value;
                    channelItem.allowfullmovie = o.Root.Element("allowfullmovie").Value;

                    if (o.Root.Element("entry").Value == "")
                    {
                        MessageBox.Show("ไม่พบข้อมูล กรุณาลองใหม่อีกครั้งภายหลัง");
                        this.NavigationService.GoBack();
                    }
                    var itemEntry = o.Root.Element("entry");

                    channelItem.content_id = XmlValueParser.ParseInteger(itemEntry.Element("content_id"));
                    channelItem.channel_title = itemEntry.Element("channel_title").Value;
                    channelItem.thumbnail = itemEntry.Element("thumbnail").Value;
                    channelItem.thumbnail_app = itemEntry.Element("thumbnail_app").Value;
                    channelItem.thumbnail_live = itemEntry.Element("thumbnail_live").Value;
                    channelItem.detail = itemEntry.Element("detail").Value;
                    channelItem.link = itemEntry.Element("link").Value;
                    channelItem.rating = itemEntry.Element("rating").Value;
                    channelItem.embed_url = itemEntry.Element("embed_url").Value;
                    channelItem.stream_id = itemEntry.Element("stream_id").Value;

                    channelItem.view = itemEntry.Element("views").Value;

                    channelItem.menu_synopsis = itemEntry.Element("menu_synopsis").Value;
                    channelItem.menu_gallery = itemEntry.Element("menu_gallery").Value;
                    channelItem.menu_archive = itemEntry.Element("menu_archive").Value;
                    channelItem.menu_quote = itemEntry.Element("menu_quote").Value;
                    channelItem.menu_music = itemEntry.Element("menu_music").Value;
                    channelItem.menu_news = itemEntry.Element("menu_news").Value;

                    if (itemEntry.Element("menu_synopsis").Value.Equals(Active))
                        {
                            menu1 = "/Assets/icn_submenu01_act.png";
                        }else{
                            menu1 = "/Assets/icn_submenu01_inact.png";
                        }

                        if (itemEntry.Element("menu_gallery").Value.Equals(Active))
                        {
                            menu2 = "/Assets/icn_submenu02_act.png";
                        }else{
                            menu2 = "/Assets/icn_submenu02_inact.png";
                        }

                        if (itemEntry.Element("menu_archive").Value.Equals(Active))
                        {
                            menu3 = "/Assets/icn_submenu03_act.png";
                        }else{
                            menu3 = "/Assets/icn_submenu03_inact.png";
                        }

                        if (itemEntry.Element("menu_quote").Value.Equals(Active))
                        {
                            menu4 = "/Assets/icn_submenu04_act.png";
                        }else{
                            menu4 = "/Assets/icn_submenu04_inact.png";
                        }

                        if (itemEntry.Element("menu_music").Value.Equals(Active))
                        {
                            menu5 = "/Assets/icn_submenu05_act.png";
                        }else{
                            menu5 = "/Assets/icn_submenu05_inact.png";
                        }

                        if (itemEntry.Element("menu_news").Value.Equals(Active))
                        {
                            menu6 = "/Assets/icn_submenu06_act.png";
                        }else{
                            menu6 = "/Assets/icn_submenu06_inact.png";
                        }

                        listMenu.Add(menu1);
                        listMenu.Add(menu2);
                        listMenu.Add(menu3);
                        listMenu.Add(menu4);
                        listMenu.Add(menu5);
                        listMenu.Add(menu6);

                        listMenuName.Add("เรื่องย่อละคร");
                        listMenuName.Add("อัลบั้มภาพ");
                        listMenuName.Add("ชมย้อนหลัง");
                        listMenuName.Add("เกร็ดละคร");
                        listMenuName.Add("เพลงละคร");
                        listMenuName.Add("ข่าวละคร");

                        channelItem.StreamingData = new StreamingItem();

                    if (itemEntry.Element("stream") != null)
                    {
                        channelItem.StreamingData.ProjectID = XmlValueParser.ParseInteger(itemEntry.Element("stream").Element("project_id"));
                        channelItem.StreamingData.Scope = XmlValueParser.ParseString(itemEntry.Element("stream").Element("scope"));
                        channelItem.StreamingData.ContentType = XmlValueParser.ParseString(itemEntry.Element("stream").Element("content_type"));
                        channelItem.StreamingData.ContentID = XmlValueParser.ParseString(itemEntry.Element("stream").Element("content_id"));
                        channelItem.StreamingData.ProductID = XmlValueParser.ParseInteger(itemEntry.Element("stream").Element("product_id"));
                        channelItem.StreamingData.LifeStyle = XmlValueParser.ParseString(itemEntry.Element("stream").Element("lifestyle"));
                    }
                    channelItem.Streaming_LiveData = new StreamingItem();
                    if (itemEntry.Element("stream_live") != null)
                    {
                        channelItem.Streaming_LiveData.ProjectID = XmlValueParser.ParseInteger(itemEntry.Element("stream_live").Element("project_id"));
                        channelItem.Streaming_LiveData.Scope = XmlValueParser.ParseString(itemEntry.Element("stream_live").Element("scope"));
                        channelItem.Streaming_LiveData.ContentType = XmlValueParser.ParseString(itemEntry.Element("stream_live").Element("content_type"));
                        channelItem.Streaming_LiveData.ContentID = XmlValueParser.ParseString(itemEntry.Element("stream_live").Element("content_id"));
                        channelItem.Streaming_LiveData.ProductID = XmlValueParser.ParseInteger(itemEntry.Element("stream_live").Element("product_id"));
                        channelItem.Streaming_LiveData.LifeStyle = XmlValueParser.ParseString(itemEntry.Element("stream_live").Element("lifestyle"));
                    }

                    ChannellList.Add(channelItem);

                    foreach (ChannelDetailItem channel in ChannellList)
                    {
                        int i = 0;
                        foreach (string value in listMenu)
	                    {
                            toolItemlist.Add(new ToolItem 
                            { 
                                id = i, 
                                image_path = value, 
                                title = listMenuName[i] 
                            });
                          i++;
	                    }
                        textHead.Text = channelItem.channel_title;
                        viewnum.Text = channelItem.view;
                        ToolBox.ItemsSource = toolItemlist;
                    }

                    if (Page.Equals("TodayList"))
                    {
                        PicTop.Source = new BitmapImage(new Uri(channelItem.thumbnail_live, UriKind.Absolute));

                        if (itemEntry.Element("stream_live") != null)
                        {
                            iconPlay.Visibility = Visibility.Visible;
                            PicTop.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            iconPlay.Visibility = Visibility.Collapsed;
                            player.Visibility = Visibility.Collapsed;
                            PicTop.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        if (channelItem.embed_url != null && channelItem.embed_url != "")
                        {
                            descriptionLabel.Visibility = Visibility.Visible;
                            descriptionLabel.NavigateToString(eNewsDetailPage.StripTagsRegex(channelItem.embed_url));
                            player.Visibility = Visibility.Collapsed;
                            PicTop.Visibility = Visibility.Collapsed;

                        }
                        else
                        {
                            PicTop.Source = new BitmapImage(new Uri(channelItem.thumbnail_live, UriKind.Absolute));
                            PicTop.Visibility = Visibility.Visible;
                            player.Visibility = Visibility.Collapsed;
                        }
                    }
                }
                else
                {
                    throw new Exception("code is " + o.Root.Element("status_code").Value);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ChannelDetail.xaml.cs : GetTotal_Completed ; " + ex.Message);
                this.NavigationService.GoBack();
                NavigationService.Navigate(new Uri("/MainPage.xaml?Refresh=true", UriKind.Relative));
            }
            finally
            {
                HideProgressIndicator();
            }
        }

        private void iconPlay_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            LiveStream stream = new LiveStream();
            stream.GetLiveStreamUrlCompleted += stream_GetLiveStreamUrlCompleted;
            stream.GetLiveStreamUrlAsyn(channelItem.Streaming_LiveData);
            PicTop.Visibility = Visibility.Collapsed;
            player.Visibility = Visibility.Visible;
        }


        void stream_GetLiveStreamUrlCompleted(LiveStreamEventArgs e)
        {
            try
            {
                (Application.Current as App).SmoothStreamURL = e.Result;
                Debug.WriteLine(e.Result);
                player.Source = new Uri((Application.Current as App).SmoothStreamURL, UriKind.Absolute);
                
            }
            catch
            {
                throw new Exception("GetLiveStreamUrl error");
            }

        }

        private void ToolBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToolItem data = (sender as ListBox).SelectedItem as ToolItem;

            if (ToolBox.SelectedIndex != -1)
            {
                switch (data.id)
	            {
	                case 0:
                        if (channelItem.menu_synopsis.Equals(Active))
                        {
                            this.NavigationService.Navigate(new Uri("/SynopsisDetailPage.xaml?ContentID=" + channelItem.content_id, UriKind.Relative));
                        }
		            break;
	                case 1:
                        if (channelItem.menu_gallery.Equals(Active))
                        {
                            this.NavigationService.Navigate(new Uri("/AlbumsPage.xaml?ContentID=" + channelItem.content_id + "&Title=" + channelItem.channel_title, UriKind.Relative));
                        }
		            break;
                    case 2:
                        if (channelItem.menu_archive.Equals(Active))
                        {
                            this.NavigationService.Navigate(new Uri("/ArchivePage.xaml?ContentID=" + channelItem.content_id + "&Title=" + channelItem.channel_title, UriKind.Relative));
                        }
		            break;
                    case 3:
                        if (channelItem.menu_quote.Equals(Active))
                        {
                            this.NavigationService.Navigate(new Uri("/DaraNewsPage.xaml?ContentID=" + channelItem.content_id + "&Title=" + channelItem.channel_title + "&Page=" + data.id, UriKind.Relative));
                        }
                    
		            break;
                    case 4:
                        if (channelItem.menu_music.Equals(Active))
                        {
                            this.NavigationService.Navigate(new Uri("/MusicPage.xaml?ContentID=" + channelItem.content_id, UriKind.Relative));
                        }
		            break;
                    case 5:
                        if (channelItem.menu_news.Equals(Active))
                        {
                            this.NavigationService.Navigate(new Uri("/DaraNewsPage.xaml?ContentID=" + channelItem.content_id + "&Title=" + channelItem.channel_title + "&Page=" + data.id, UriKind.Relative));
                        }
                   
		            
		            break;
	            }
            }
            ToolBox.SelectedIndex = -1;
        }

        private void btn_info_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(channelItem.detail.Replace("&nbsp;", " "), (Application.Current as App).Info, MessageBoxButton.OK);

            if (result == MessageBoxResult.OK) { }
        }

        private void btn_share_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ShareContentPage.xaml?content_id=" + this.content_id + "&title=" + channelItem.channel_title + "&description=" + channelItem.channel_title + "&url=" + channelItem.thumbnail_live + "&img=" + channelItem.thumbnail_live, UriKind.Relative));
        }
    }
}