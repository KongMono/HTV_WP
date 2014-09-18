using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Microsoft.Phone.Controls;

using System.Xml.Linq;
using System.Diagnostics;
using System.Windows.Data;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Net.NetworkInformation;
using System.Collections;
using System.Text;
using System.IO;
using System.Windows.Navigation;
using Microsoft.Xna.Framework.Media;
using Microsoft.Phone.Scheduler;
using CountlySDK;
using Microsoft.Phone.Tasks;

namespace News
{

    public partial class MainPage : PhoneApplicationPage
    {
        static int offset_news = 0;
        const int limit = 30;
        static int total = 0;
        const int loadMoreHeight = 5;
        string Message;
        bool status;
        bool Menu2 = true;
        bool Menu3 = true;
        bool Menu4 = true;
        bool clicked = false;
        string[] imageNames;
        List<string> ImageTile = new List<string>();
        static List<CategoryItem> CategoryChannel = new List<CategoryItem>();
        static int lastContentID = 0;
        static List<TVListAllItem> ListAllChannel = new List<TVListAllItem>();
        static List<MediaHighlightItem> ListfavoriteChannel = new List<MediaHighlightItem>();
        static WebClient webClient;
        ScrollViewer scrollViewer;
        LiveStream stream = new LiveStream();
        ChannelDetailItem channelItem = new ChannelDetailItem();
        ObservableCollection<MediaHighlightItem> TopChannelList = new ObservableCollection<MediaHighlightItem>();
        ObservableCollection<ProgramRecordItem> ProgramRecordItemaList = new ObservableCollection<ProgramRecordItem>();
        ObservableCollection<TVListAllItem> ListSocietyAllList = new ObservableCollection<TVListAllItem>();
        ObservableCollection<ListTodayItem> ListTodayItemList = new ObservableCollection<ListTodayItem>();
        ObservableCollection<MediaHighlightItem> liveChannelAllList = new ObservableCollection<MediaHighlightItem>();
        ObservableCollection<ShotDaraItem> ListTopChannelList = new ObservableCollection<ShotDaraItem>();
        ObservableCollection<HClusiveItem> hClusiveItemList = new ObservableCollection<HClusiveItem>();
        ObservableCollection<TVSocietyItem> TVSocietyList = new ObservableCollection<TVSocietyItem>();
        ObservableCollection<ChannelDetailItem> LiveList = new ObservableCollection<ChannelDetailItem>();

        const string url_TopChannel = "http://api-movie.truelife.com/wrap_api/mod/stream/home?method=getlist";
        const string url_GetInfo = "http://api-movie.truelife.com/wrap_api/mod/tv/livetvinfo?method=getinfo&content_id=";
        const string url_HClusive = "http://api-movie.truelife.com/wrap_api/mod/novel/home?method=getlist";
        const string url_ChannelAll = "http://api-movie.truelife.com/wrap_api/mod/tv/livetv?method=getlist&category=XXXX&offset=0&limit=100";
        const string url_TVSociety = "http://api-movie.truelife.com/wrap_api/mod/news/getlist?method=getlist";
        const string url_ListToday = "http://api-movie.truelife.com/wrap_api/mod/drama/home?method=getlist";
        const string url_ListAll = "http://api-movie.truelife.com/wrap_api/mod/drama/getlist?method=getlist&limit=20&offset=0";

        const string url_ProgramRecord = "http://mstage.truelife.com/api_movietv/program/rerunlist?method=getlist&limit=50&offset=0";
        //const string url_ListShotDara = "http://mstage.truelife.com/api_movietv/instagram/getlist?method=getlist&limit=20";
 
        ProgressIndicator progressIndicator;

        public bool agentsAreEnabled = true;

        public MainPage()
        {
            InitializeComponent();

            /**
             * 
             * add Rating
             * 
             * */
            //MarketplaceReviewTask rev = new MarketplaceReviewTask();
            //rev.Show();

            homePanorama.Title = (Application.Current as App).AppName;
            //Random image background
            int rand_img;
            Random random = new Random();
            rand_img = random.Next(1, 5);
            bg_img.ImageSource = new BitmapImage(new Uri("http://mstage.truelife.com/api_movietv/assets/wp8_bg/bg" + rand_img + ".jpg", UriKind.RelativeOrAbsolute));

            progressIndicator = new ProgressIndicator();

          
        }

        //System Event
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            ShowProgressIndicator("Loading..");
            CallCheckAddChannel();


            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
            {
                //call data
                this.callapiGetHomeStream(1);  // Default select Category ALL (1)
                this.callapiHClusive();
                this.callapiListToday();

                ShowProgressIndicator("Loading...");

                liveTvListBox.ItemsSource = liveChannelAllList;
                TVSocietyListTodayBox.ItemsSource = ListTodayItemList;
                TVSocietyNewsListBox.ItemsSource = TVSocietyList;
                HClusiveListBox.ItemsSource = hClusiveItemList;
                TVSocietyListShotDaraBox.ItemsSource = ProgramRecordItemaList;
                
            }

            //reset selected item
            liveTvFavoriteListBox.SelectedIndex = -1;
            HClusiveListBox.SelectedIndex = -1;
            liveTvListBox.SelectedIndex = -1;
            TVSocietyNewsListBox.SelectedIndex = -1;
            TVSocietyListTodayBox.SelectedIndex = -1;
            TVSocietyListShotDaraBox.SelectedIndex = -1;
            //-----
            base.OnNavigatedTo(e);

            ShellTile AppShell = ShellTile.ActiveTiles.First();
            StandardTileData AppTile = new StandardTileData();
            AppTile.Title = (Application.Current as App).AppName;
            AppTile.BackgroundImage = new Uri("Assets/icn_HTV_173_2.png", UriKind.Relative);
            AppTile.BackTitle = (Application.Current as App).AppName;
            AppTile.BackBackgroundImage = new Uri("Assets/background.jpg", UriKind.Relative);
            AppTile.BackContent = (Application.Current as App).AppName;
            AppShell.Update(AppTile);

        }

        private void CallCheckAddChannel()
        {
            TopChannelList.Clear();

            if ((Application.Current as App).FavoriteIndexList.Count > 0)
            {
                for (int i = 0; i < (Application.Current as App).FavoriteIndexList.Count; i++)
                {
                    TopChannelList.Add((Application.Current as App).FavoriteIndexList[i]);
                }

                if (TopChannelList.Count < 20)
                {
                    MediaHighlightItem tmp_item = new MediaHighlightItem();
                    //parse
                    tmp_item.content_id = 0;
                    tmp_item.channel_name = "";
                    tmp_item.thumbnail = "/Assets/btn_more.png";
                    tmp_item.view = "";
                    tmp_item.Share_url = "";
                    TopChannelList.Add(tmp_item);

                }
            }
            else
            {
                this.callapiTopChannel();
            }

            liveTvFavoriteListBox.ItemsSource = TopChannelList;
            HideProgressIndicator();

        }

        private void TopChannelList_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
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

                if (xdoc.Root.Element("data") == null)
                {
                    throw new Exception("data is null");
                }

                if (xdoc.Root.Element("data").Element("contents") == null)
                {
                    throw new Exception("data/contents is null");
                }
                //----------
                if (xdoc.Root.Element("status_code").Value != "200")
                {
                    throw new Exception("code is " + xdoc.Root.Element("status_code").Value + " ~ " + xdoc.Root.Element("status_txt").Value);
                }
                else
                {
                    string tag = "";

                    if ((sender as WebClient).Headers["id"] == "liveTv")
                    {
                        tag = "entry";// item
                        TopChannelList.Clear();
                        (Application.Current as App).FavoriteIndexList.Clear();
                    }
                    //-----

                    var list = xdoc.Root.Element("data").Element("contents").Elements(tag);

                    MediaHighlightItem tmp_item = new MediaHighlightItem();

                    foreach (var item in list)
                    {
                        tmp_item = new MediaHighlightItem();

                        //parse
                        tmp_item.content_id = XmlValueParser.ParseInteger(item.Element("content_id"));
                        tmp_item.channel_name = XmlValueParser.ParseString(item.Element("channel_name"));
                        tmp_item.thumbnail = XmlValueParser.ParseString(item.Element("thumbnail"));
                        tmp_item.view = XmlValueParser.ParseString(item.Element("view"));
                        tmp_item.Share_url = XmlValueParser.ParseString(item.Element("share_url"));


                        //add

                        if ((sender as WebClient).Headers["id"] == "liveTv")
                        {
                            TopChannelList.Add(tmp_item);
                            (Application.Current as App).FavoriteIndexList.Add(tmp_item);
                            HideProgressIndicator();
                            //SetLoadingSteamingVisibility(false);
                        }
                    }
                    if (TopChannelList.Count < 20)
                    {
                        tmp_item = new MediaHighlightItem();
                        //parse
                        tmp_item.content_id = 0;
                        tmp_item.channel_name = "";
                        tmp_item.thumbnail = "/Assets/btn_more.png";
                        tmp_item.view = "";
                        tmp_item.Share_url = "";
                        TopChannelList.Add(tmp_item);

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("HomePage : MediaHighlightItemList_DownloadStringCompleted ; " + ex.Message);
            }
            finally
            {
                setApplicationBarMenu();
            }
        }


        private void liveChannelAllList_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
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

                if (xdoc.Root.Element("data") == null)
                {
                    throw new Exception("data is null");
                }

                if (xdoc.Root.Element("data").Element("contents") == null)
                {
                    throw new Exception("data/contents is null");
                }
                //----------
                if (xdoc.Root.Element("status_code").Value != "200")
                {
                    throw new Exception("code is " + xdoc.Root.Element("status_code").Value + " ~ " + xdoc.Root.Element("status_txt").Value);
                }
                else
                {
                    string tag = "";

                    if ((sender as WebClient).Headers["id"] == "liveTv")
                    {
                        tag = "entry";// item
                        liveChannelAllList.Clear();
                    }
                    //-----

                    var listMenu = xdoc.Root.Element("menu").Elements("item");
                    foreach (var item in listMenu)
                    {
                        CategoryItem listMenuitem = new CategoryItem();
                        listMenuitem.id = item.Element("id").Value;
                        listMenuitem.title = item.Element("title").Value;
                        CategoryChannel.Add(listMenuitem);
                    }


                    var list = xdoc.Root.Element("data").Element("contents").Elements(tag);
                    foreach (var item in list)
                    {
                        MediaHighlightItem tmp_item = new MediaHighlightItem();

                        //parse
                        tmp_item.content_id = XmlValueParser.ParseInteger(item.Element("content_id"));
                        tmp_item.channel_name = XmlValueParser.ParseString(item.Element("channel_name"));
                        tmp_item.thumbnail = XmlValueParser.ParseString(item.Element("thumbnail"));
                        tmp_item.thumbnail_App = XmlValueParser.ParseString(item.Element("thumbnail_app"));
                        tmp_item.category = XmlValueParser.ParseString(item.Element("category"));
                        tmp_item.rating = XmlValueParser.ParseString(item.Element("rating"));
                        tmp_item.view = XmlValueParser.ParseString(item.Element("view"));
                        tmp_item.Share_url = XmlValueParser.ParseString(item.Element("share_url"));

                        if ((sender as WebClient).Headers["id"] == "liveTv")
                        {
                            liveChannelAllList.Add(tmp_item);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("HomePage : MediaHighlightItemList_DownloadStringCompleted ; " + ex.Message);
            }
            finally
            {
                clicked = false;
                HideProgressIndicator();
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

        public void callapiTopChannel()
        {
            webClient = new WebClient();
            try
            {
                webClient.Headers["id"] = "liveTv";
                Debug.WriteLine("URL url_TopChannel = " + url_TopChannel);
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(TopChannelList_DownloadStringCompleted);
                webClient.DownloadStringAsync(new Uri(url_TopChannel));

            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        public void callapiGetHomeStream(int CateID)
        {
            webClient = new WebClient();
            string url = null;
            try
            {
                ShowProgressIndicator("LoadingStreamChannel...");
                //SetLoadingSteamingVisibility(true);

                webClient.Headers["id"] = "liveTv";
                url = url_ChannelAll.Replace("XXXX", Convert.ToString(CateID));

                Debug.WriteLine("URL url_ChannelAll = " + url);
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(liveChannelAllList_DownloadStringCompleted);
                webClient.DownloadStringAsync(new Uri(url));

            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        public void callapiGetInfoStream(int content_id)
        {
            webClient = new WebClient();

            String url = "";
            try
            {
                url = url_GetInfo + content_id;
                webClient.Headers["type"] = "tv";

                Debug.WriteLine("URL url_GetInfo = " + url);

                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(getApiInfoStream_Completed);
                webClient.DownloadStringAsync(new Uri(url));
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void callapiProgramRecord()
        {
            webClient = new WebClient();

            String url = url_ProgramRecord;
            try
            {
                webClient.Headers["type"] = "tv";

                Debug.WriteLine("URL url_ProgramRecord = " + url);

                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(getapiProgramRecord_Completed);
                webClient.DownloadStringAsync(new Uri(url));


            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void callapiListALL()
        {
            webClient = new WebClient();

            String url = url_ListAll;
            try
            {
                webClient.Headers["type"] = "tv";

                Debug.WriteLine("URL url_ListAll = " + url);

                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(getApiListAll_Completed);
                webClient.DownloadStringAsync(new Uri(url));


            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void callapiListToday()
        {
            webClient = new WebClient();

            String url = url_ListToday;
            try
            {
                webClient.Headers["type"] = "tv";

                Debug.WriteLine("URL url_ListToday = " + url);

                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(getApiListToday_Completed);
                webClient.DownloadStringAsync(new Uri(url));


            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void callapiTVSociety()
        {
            webClient = new WebClient();
            //SetLoadingTVSocietyVisibility(true);
            ShowProgressIndicator("LoadHTVSociety...");

            String url = "";
            try
            {
                url = url_TVSociety + "&category=tv" + "&offset=" + offset_news + "&limit=" + limit;
                webClient.Headers["type"] = "tv";

                Debug.WriteLine("URL eNews = " + url);

                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(getApiTVSociety_Completed);
                webClient.DownloadStringAsync(new Uri(url));
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void callapiHClusive()
        {
            webClient = new WebClient();
            //SetLoadingTVSocietyVisibility(true);
            ShowProgressIndicator("LoadHClusive...");
            String urlHClusive = null;
            try
            {
                webClient.Headers["type"] = "tv";

                if (isTrueCellularMobileOperator())
                {
                    urlHClusive = url_HClusive + "&carrier_name=true" + "&version=" + (Application.Current as App).AppVersion;
                }
                else
                {
                    urlHClusive = url_HClusive + "&carrier_name=false" + "&version=" + (Application.Current as App).AppVersion;
                }

                Debug.WriteLine("URL callapiHClusive = " + urlHClusive);

                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(getApiHClusive_Completed);
                webClient.DownloadStringAsync(new Uri(urlHClusive));

            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void getApiInfoStream_Completed(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    throw new Exception(e.Error.Message);
                }
                var o = XDocument.Parse(e.Result);

                if (o.Root.Element("status_code").Value != "200")
                {
                    throw new Exception("code is " + o.Root.Element("status_code").Value + " ~ " + o.Root.Element("status_txt").Value);
                }
                if (o.Root.Element("status_code").Value == "200")
                {
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

                    LiveList.Add(channelItem);

                    LiveStream stream = new LiveStream();
                    stream.GetLiveStreamUrlCompleted += stream_GetLiveStreamUrlCompleted;
                    stream.GetLiveStreamUrlAsyn(channelItem.StreamingData);
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
        }

        void stream_GetLiveStreamUrlCompleted(LiveStreamEventArgs e)
        {
            try
            {
                (Application.Current as App).SmoothStreamURL = e.Result;

                Debug.WriteLine(e.Result);
                NavigationService.Navigate(new Uri("/PlayerPage.xaml", UriKind.Relative));
                HideProgressIndicator();
            }
            catch
            {
                throw new Exception("GetLiveStreamUrl error");
            }

        }

        public void getApiTVSociety_Completed(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    throw new Exception(e.Error.Message);
                }
                var o = XDocument.Parse(e.Result);
                if (o.Root.Element("status_code").Value == "200")
                {
                    total = XmlValueParser.ParseInteger(o.Root.Element("data").Element("total"));

                    foreach (var v in o.Descendants("item"))
                    {
                        total = XmlValueParser.ParseInteger(o.Root.Element("data").Element("total"));
                        TVSocietyItem item = new TVSocietyItem();

                        item.Title = v.Element("title").Value;
                        item.ImagePath = v.Element("thumbnail").Value;
                        item.ContentID = XmlValueParser.ParseInteger(v.Element("id"));

                        TVSocietyList.Add(item);

                        TVSocietyNewsListBox.Loaded += LoadmoreListBox_Loaded;
                    }

                    //set num
                    offset_news = TVSocietyList.Count;

                }
                else
                {
                    throw new Exception("code is " + o.Root.Element("status_code").Value);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MainPage.xaml.cs : getApiTVSociety_Completed ; " + ex.Message);
                MessageBox.Show("เกิดเหตุขัดข้อง กรุณาลองใหม่อีกครั้งภายหลัง");
                this.NavigationService.GoBack();
                NavigationService.Navigate(new Uri("/MainPage.xaml?Refresh=true", UriKind.Relative));
            }
            finally
            {
                HideProgressIndicator();
            }

            
        }

        public void getApiListAll_Completed(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    throw new Exception(e.Error.Message);
                }

                var o = XDocument.Parse(e.Result);

                if (o.Root.Element("status_code").Value == "200")
                {
                    foreach (var v in o.Descendants("section"))
                    {
                        string channel_name = v.Element("channel_name").Value;

                        foreach (var i in v.Descendants("item"))
                        {
                            TVListAllItem item = new TVListAllItem();

                            item.content_id = XmlValueParser.ParseInteger(i.Element("content_id"));
                            item.thumbnail = i.Element("thumbnail").Value;
                            item.title = i.Element("title").Value;
                            item.view = i.Element("view").Value;
                            item.Share_url = i.Element("share_url").Value;

                            item.channel_name = channel_name;
                            ImageTile.Add(item.thumbnail);
                            ListSocietyAllList.Add(item);


                            ListAllChannel.Add(new TVListAllItem
                            {
                                channel_name = item.channel_name,
                                content_id = item.content_id,
                                thumbnail = item.thumbnail,
                                title = item.title,
                                view = item.view,
                                Share_url = item.Share_url
                            });
                        }

                    }

                    this.longListSelector.ItemsSource = this.GetListGroups(ListAllChannel);
                }
                else
                {
                    throw new Exception("code is " + o.Root.Element("status_code").Value);
                }

                imageNames = ImageTile.ToArray();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("MainPage.xaml.cs : getApiListAll_Completed ; " + ex.Message);
                MessageBox.Show("เกิดเหตุขัดข้อง กรุณาลองใหม่อีกครั้งภายหลัง");
                this.NavigationService.GoBack();
                NavigationService.Navigate(new Uri("/MainPage.xaml?Refresh=true", UriKind.Relative));
            }
            finally
            {
                HideProgressIndicator();
            }

        }

        public void getapiProgramRecord_Completed(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    throw new Exception(e.Error.Message);
                }

                var o = XDocument.Parse(e.Result);

                if (o.Root.Element("status_code").Value == "200")
                {
                    total = XmlValueParser.ParseInteger(o.Root.Element("data").Element("total"));

                    foreach (var v in o.Descendants("content"))
                    {
                        ProgramRecordItem item = new ProgramRecordItem();
                        item.content_id = v.Element("content_id").Value;
                        item.title = v.Element("title").Value;
                        item.thumbnail = v.Element("thumbnail").Value;
                        item.description = v.Element("description").Value;
                        item.synopsis = v.Element("synopsis").Value;
                        item.trailer = v.Element("trailer").Value;
                        item.view = v.Element("view").Value;
                        item.episode_total = v.Element("episode_total").Value;

                        ProgramRecordItemaList.Add(item);
                    }
                }
                else
                {
                    throw new Exception("code is " + o.Root.Element("status_code").Value);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("MainPage.xaml.cs : getApiListShotDara_Completed ; " + ex.Message);
                MessageBox.Show("เกิดเหตุขัดข้อง กรุณาลองใหม่อีกครั้งภายหลัง");
                this.NavigationService.GoBack();
                NavigationService.Navigate(new Uri("/MainPage.xaml?Refresh=true", UriKind.Relative));
            }
            finally
            {
                HideProgressIndicator();
            }

        }

        private List<Group<TVListAllItem>> GetListGroups(List<TVListAllItem> ListAllChannel)
        {
            IEnumerable<TVListAllItem> cityList = ListAllChannel;
            return GetItemGroups(cityList, c => c.channel_name);
        }

        private static List<Group<T>> GetItemGroups<T>(IEnumerable<T> itemList, Func<T, string> getKeyFunc)
        {
            IEnumerable<Group<T>> groupList = from item in itemList
                                              group item by getKeyFunc(item) into g
                                              orderby g.Key
                                              select new Group<T>(g.Key, g);

            return groupList.ToList();
        }

        public class Group<T> : List<T>
        {
            public Group(string name, IEnumerable<T> items)
                : base(items)
            {
                this.Channel = name;
            }

            public string Channel
            {
                get;
                set;
            }
        }

        public void getApiListToday_Completed(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    throw new Exception(e.Error.Message);
                }
                var o = XDocument.Parse(e.Result);
                ListTodayItem item = new ListTodayItem();
                if (o.Root.Element("status_code").Value == "200")
                {
                    total = XmlValueParser.ParseInteger(o.Root.Element("data").Element("total"));

                    foreach (var v in o.Descendants("entry"))
                    {
                        item = new ListTodayItem();
                        item.content_id = v.Element("content_id").Value;
                        item.thumbnail = v.Element("thumbnail").Value;
                        item.title = v.Element("title").Value;
                        item.view = v.Element("view").Value;
                        item.Share_url = v.Element("share_url").Value;

                        ListTodayItemList.Add(item);
                    }
                }
                else
                {
                    throw new Exception("code is " + o.Root.Element("status_code").Value);
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine("MainsPage.xaml.cs : getApiListToday_Completed ; " + ex.Message);
                MessageBox.Show("เกิดเหตุขัดข้อง กรุณาลองใหม่อีกครั้งภายหลัง");
                this.NavigationService.GoBack();
                NavigationService.Navigate(new Uri("/MainPage.xaml?Refresh=true", UriKind.Relative));
            }
            finally
            {
                HideProgressIndicator();
            }


            

        }

        public void getApiHClusive_Completed(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    throw new Exception(e.Error.Message);
                }
                var o = XDocument.Parse(e.Result);

                if (o.Root.Element("status_code").Value == "200")
                {
                    total = XmlValueParser.ParseInteger(o.Root.Element("data").Element("total"));

                    foreach (var v in o.Descendants("item"))
                    {
                        HClusiveItem item = new HClusiveItem();
                        item.Title = v.Element("title").Value;
                        item.ImagePath = v.Element("thumbnail").Value;
                        item.ContentID = XmlValueParser.ParseInteger(v.Element("id"));

                        hClusiveItemList.Add(item);
                    }

                    foreach (var v in o.Descendants("allow"))
                    {
                        String Status = v.Element("status").Value;
                        Message = v.Element("message").Value;
                        status = Status.Equals("yes");
                    }
                }
                else
                {
                    throw new Exception("code is " + o.Root.Element("status_code").Value);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MainPage.xaml.cs : getApiHClusive_Completed ; " + ex.Message);
                MessageBox.Show("เกิดเหตุขัดข้อง กรุณาลองใหม่อีกครั้งภายหลัง");
                this.NavigationService.GoBack();
                NavigationService.Navigate(new Uri("/MainPage.xaml?Refresh=true", UriKind.Relative));
            }
            HideProgressIndicator();
        }


        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            //remove all page in stack
            while (this.NavigationService.BackStack.Count() > 0)
            {
                this.NavigationService.RemoveBackEntry();
            }

            base.OnBackKeyPress(e);
        }


        private void panorama_SelectChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as Panorama).SelectedIndex >= 0)
            {
                PanoramaItem item = (sender as Panorama).SelectedItem as PanoramaItem;
                GoogleAnalytics.EasyTracker.GetTracker().SendView((Application.Current as App).pathAnalytic.Replace("title", Convert.ToString(item.Header)).Replace("level", "A"));

                setApplicationBarMenu();
            }
        }

        private void setApplicationBarMenu()
        {
            String Resource = null;

            switch (homePanorama.SelectedIndex)
            {
                case 0:
                    if (pivotSteam.SelectedIndex > 0)
                    {
                        Resource = "StreamBar";
                    }
                    else
                    {
                        if (ApplicationBar != null)
                        {
                            ApplicationBar.IsVisible = false;
                        }
                        else
                        {
                            Resource = null;
                        }
                    }

                    break;
                case 1:
                    Resource = "HTVSocietyBar";
                    break;
                case 2:
                    ApplicationBar.IsVisible = false;
                    break;
                case 3:
                    ApplicationBar.IsVisible = false;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (Resource != null)
            {
                ApplicationBar = (ApplicationBar)Resources[Resource];
                ApplicationBar.IsVisible = true;
            }
        }

        private void TVSocietyNewsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TVSocietyItem data = (sender as ListBox).SelectedItem as TVSocietyItem;

            if (TVSocietyNewsListBox.SelectedIndex != -1)
            {
                Segmentation segmentation = new Segmentation();
                segmentation.Add("page_count", data.Title);
                Countly.RecordEvent("htv_society", 1, segmentation);

                this.NavigationService.Navigate(new Uri("/eNewsDetailPage.xaml?ContentID=" + data.ContentID, UriKind.Relative));
            }

            TVSocietyNewsListBox.SelectedIndex = -1;
        }

        private void HClusiveListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HClusiveItem data = (sender as ListBox).SelectedItem as HClusiveItem;

            if (HClusiveListBox.SelectedIndex != -1)
            {
                if (status)
                {
                    Segmentation segmentation = new Segmentation();
                    segmentation.Add("page_count", data.Title);
                    Countly.RecordEvent("hclusive", 1, segmentation);

                    this.NavigationService.Navigate(new Uri("/HClusiveDetailPage.xaml?ContentID=" + data.ContentID + "&Title=" + data.Title, UriKind.Relative));
                }
                else
                {
                    MessageBox.Show(Message);
                }
            }
            HClusiveListBox.SelectedIndex = -1;
        }

        //GUI Event
        void LoadmoreListBox_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            element.Loaded -= LoadmoreListBox_Loaded;
            scrollViewer = ChildFinder.FindChildOfType<ScrollViewer>(element);

            if (scrollViewer == null)
            {
                throw new InvalidOperationException("ScrollViewer not found.");
            }

            Binding binding = new Binding();
            binding.Source = scrollViewer;
            binding.Path = new PropertyPath("VerticalOffset");
            binding.Mode = BindingMode.OneWay;
            this.SetBinding(ListVerticalOffsetProperty, binding);
        }


        public readonly DependencyProperty ListVerticalOffsetProperty = DependencyProperty.Register("ListVerticalOffset",
        typeof(double), typeof(MainPage), new PropertyMetadata(new PropertyChangedCallback(OnListVerticalOffsetChanged)));

        public double ListVerticalOffset
        {
            get { return (double)this.GetValue(ListVerticalOffsetProperty); }
            set { this.SetValue(ListVerticalOffsetProperty, value); }
        }

        private static void OnListVerticalOffsetChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            MainPage page = obj as MainPage;
            ScrollViewer viewer = page.scrollViewer;

            //Checks if the Scroll has reached the last item based on the ScrollableHeight 
            Debug.WriteLine("viewer.VerticalOffset = " + viewer.VerticalOffset + "viewer.ScrollableHeight = " + (viewer.ScrollableHeight - loadMoreHeight));
            if (viewer.VerticalOffset >= viewer.ScrollableHeight - loadMoreHeight)
            {
                //ignore if loading
                if (SystemTray.GetProgressIndicator(page).IsVisible)
                {
                    return;
                }
                ProgressIndicator indicator = new ProgressIndicator();
                indicator.IsVisible = true;
                indicator.IsIndeterminate = true;
                indicator.Text = "Loading...";
                SystemTray.SetProgressIndicator(page, indicator);
                //get url
                string url = "";
                //call data

                url = url_TVSociety + "&offset=" + offset_news + "&limit=" + limit + "&category=tv";


                webClient.Headers["type"] = "tv";
                Debug.WriteLine("URL LOAD MORE TV : " + url);

                if (offset_news >= total)
                {
                    SystemTray.SetProgressIndicator(page, null);

                    return;
                }

                //call data
                webClient.DownloadStringAsync(new Uri(url, UriKind.Absolute));
                offset_news += limit;

            }
        }

        private void liveTvListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListBox).SelectedIndex >= 0)
            {
                ShowProgressIndicator("Loading...");

                MediaHighlightItem item = (sender as ListBox).SelectedItem as MediaHighlightItem;
                if (item.content_id == 0)
                {
                    HideProgressIndicator();
                    this.NavigationService.Navigate(new Uri("/AddChannelPage.xaml", UriKind.Relative));
                }
                else
                {
                    /**
                     * 
                     * Add Stat Countly  streaming
                     * 
                     **/
                    Segmentation segmentation = new Segmentation();
                    segmentation.Add("page_count", item.channel_name);
                    Countly.RecordEvent("streaming", 1, segmentation);

                    lastContentID = item.content_id;
                    callapiGetInfoStream(lastContentID);
                }
            }
            liveTvListBox.SelectedIndex = -1;
        }

        private void liveTvFavoriteListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListBox).SelectedIndex >= 0)
            {
                ShowProgressIndicator("Loading...");

                MediaHighlightItem item = (sender as ListBox).SelectedItem as MediaHighlightItem;
                if (item.content_id == 0)
                {
                    HideProgressIndicator();
                    this.NavigationService.Navigate(new Uri("/AddChannelPage.xaml", UriKind.Relative));
                }
                else
                {
                    lastContentID = item.content_id;
                    callapiGetInfoStream(lastContentID);
                }
            }
            liveTvListBox.SelectedIndex = -1;
        }

        private void TVSocietyListTodayBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListBox).SelectedIndex >= 0)
            {
                ListTodayItem item = (sender as ListBox).SelectedItem as ListTodayItem;
                /**
                * 
                * Add Stat Countly  htv_society
                * 
                **/
                Segmentation segmentation = new Segmentation();
                segmentation.Add("page_count", item.title);
                Countly.RecordEvent("htv_society", 1, segmentation);

                this.NavigationService.Navigate(new Uri("/ChannelDetail.xaml?ContentID=" + item.content_id + "&Page=TodayList", UriKind.Relative));
            }

            TVSocietyListTodayBox.SelectedIndex = -1;
        }

        private void TVSocietyListShotDaraBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListBox).SelectedIndex >= 0)
            {
                ProgramRecordItem item = (sender as ListBox).SelectedItem as ProgramRecordItem;
                /**
                * 
                * Add Stat Countly  htv_rerun
                * 
                **/
                Segmentation segmentation = new Segmentation();
                segmentation.Add("page_count", item.title);
                Countly.RecordEvent("htv_rerun", 1, segmentation);

                this.NavigationService.Navigate(new Uri("/ProgramListDetailPage.xaml?ContentID=" + item.content_id + "&Title=" + item.title, UriKind.Relative));
            }

            TVSocietyListShotDaraBox.SelectedIndex = -1;
        }

        private void LLS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IList addedItems = e.AddedItems;
            if (addedItems == null)
            {
                return;
            }

            var item = longListSelector.SelectedItem as TVListAllItem;
            this.NavigationService.Navigate(new Uri("/ChannelDetail.xaml?ContentID=" + item.content_id + "&Page=AllList", UriKind.Relative));
        }

        public Boolean isTrueCellularMobileOperator()
        {
            var operatorName = DeviceNetworkInformation.CellularMobileOperator ?? "(No Network)";
            return operatorName.ToLower().Contains("true") || operatorName.ToLower().Contains("52000");
        }

        private void Menu1_Click(object sender, EventArgs e)
        {
            TextHead.Text = "รายการวันนี้";

            TVSocietyNewsListBox.Visibility = System.Windows.Visibility.Collapsed;
            TVSocietyListTodayBox.Visibility = System.Windows.Visibility.Visible;
            TVSocietyGridAll.Visibility = System.Windows.Visibility.Collapsed;
            TVSocietyListShotDaraBox.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Menu2_Click(object sender, EventArgs e)
        {
            TextHead.Text = "ข่าวบันเทิง";
            if (Menu2)
            {
                this.callapiTVSociety();
                ShowProgressIndicator("Loading..");
                Menu2 = false;
            }

            TVSocietyNewsListBox.Visibility = System.Windows.Visibility.Visible;
            TVSocietyListTodayBox.Visibility = System.Windows.Visibility.Collapsed;
            TVSocietyGridAll.Visibility = System.Windows.Visibility.Collapsed;
            TVSocietyListShotDaraBox.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Menu3_Click(object sender, EventArgs e)
        {

            TextHead.Text = "รายการทั้งหมด";
            if (Menu3)
            {
                this.callapiListALL();
                ShowProgressIndicator("Loading..");
                Menu3 = false;
            }

            TVSocietyNewsListBox.Visibility = System.Windows.Visibility.Collapsed;
            TVSocietyListTodayBox.Visibility = System.Windows.Visibility.Collapsed;
            TVSocietyGridAll.Visibility = System.Windows.Visibility.Visible;
            TVSocietyListShotDaraBox.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Menu4_Click(object sender, EventArgs e)
        {
            TextHead.Text = "รายการย้อนหลัง";
            if (Menu4)
            {
                this.callapiProgramRecord();
                ShowProgressIndicator("Loading..");
                Menu4 = false;
            }

            TVSocietyNewsListBox.Visibility = System.Windows.Visibility.Collapsed;
            TVSocietyListTodayBox.Visibility = System.Windows.Visibility.Collapsed;
            TVSocietyGridAll.Visibility = System.Windows.Visibility.Collapsed;
            TVSocietyListShotDaraBox.Visibility = System.Windows.Visibility.Visible;
        }


        private void DeleteChannel_Click(object sender, EventArgs e)
        {
            if (liveTvFavoriteListBox.ItemContainerGenerator == null) return;
            var selectedListBoxItem = liveTvFavoriteListBox.ItemContainerGenerator.ContainerFromItem(((MenuItem)sender).DataContext) as ListBoxItem;
            if (selectedListBoxItem == null) return;
            var selectedIndex = liveTvFavoriteListBox.ItemContainerGenerator.IndexFromContainer(selectedListBoxItem);

            MediaHighlightItem item = TopChannelList[selectedIndex];


            for (int i = 0; i < (Application.Current as App).FavoriteIndexList.Count; i++)
            {
                if ((Application.Current as App).FavoriteIndexList[i].content_id.Equals(item.content_id))
                {
                    (Application.Current as App).FavoriteIndexList.RemoveAt(i);
                    CallCheckAddChannel();
                }
            }
        }

        private void AddChannel_Click(object sender, EventArgs e)
        {
            if (liveTvListBox.ItemContainerGenerator == null) return;
            var selectedListBoxItem = liveTvListBox.ItemContainerGenerator.ContainerFromItem(((MenuItem)sender).DataContext) as ListBoxItem;
            if (selectedListBoxItem == null) return;
            var selectedIndex = liveTvListBox.ItemContainerGenerator.IndexFromContainer(selectedListBoxItem);

            MediaHighlightItem item = liveChannelAllList[selectedIndex];

            bool ExistItem = false;
            foreach (var v in (Application.Current as App).FavoriteIndexList)
            {
                if (v.content_id.Equals(item.content_id))
                {
                    ExistItem = true;
                }
            }
            if (ExistItem)
            {
                MessageBoxResult result = MessageBox.Show("มีช่องนี้ในรายการโปรดแล้วค่ะ", (Application.Current as App).AppName, MessageBoxButton.OK);

                if (result == MessageBoxResult.OK) { }
            }
            else
            {
                if ((Application.Current as App).FavoriteIndexList.Count < 20 && item != null)
                {
                    (Application.Current as App).FavoriteIndexList.Add(item);

                    MessageBoxResult result = MessageBox.Show("เพิ่มช่องโปรดเรียบร้อยแล้วค่ะ", (Application.Current as App).AppName, MessageBoxButton.OK);

                    if (result == MessageBoxResult.OK)
                    {
                        CallCheckAddChannel();
                    }
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("คุณสามารถเลือกช่องรายการโปรดได้สูงสุด 20 ช่องค่ะ", (Application.Current as App).AppName, MessageBoxButton.OK);

                    if (result == MessageBoxResult.OK) { }
                }
            }
        }

        private void pivotSteam_SelectionChangedChanged(object sender, SelectionChangedEventArgs e)
        {
            String Resource = null;

            switch (pivotSteam.SelectedIndex)
            {
                case 0:
                    if (ApplicationBar != null)
                    {
                        ApplicationBar.IsVisible = false;
                    }
                    Resource = null;
                    break;
                case 1:
                    Resource = "StreamBar";
                    break;

                default:
                    Resource = null;
                    break;
                //throw new ArgumentOutOfRangeException();
            }

            if (Resource != null)
            {
                ApplicationBar = (ApplicationBar)Resources[Resource];
                ApplicationBar.IsVisible = true;
                if (ApplicationBar.MenuItems.Count < 1)
                {
                    ApplicationBar.MenuItems.Clear();
                    foreach (var item in CategoryChannel)
                    {
                        ApplicationBarMenuItem menuItem = new ApplicationBarMenuItem();
                        menuItem.Text = item.title;
                        menuItem.Click += new EventHandler(menuItem_Click);
                        ApplicationBar.MenuItems.Add(menuItem);
                    }
                }

            }
        }

        private void menuItem_Click(object sender, EventArgs e)
        {
            if (!clicked)
            {
                clicked = true;
                ApplicationBarMenuItem mi = (ApplicationBarMenuItem)sender as ApplicationBarMenuItem;
                foreach (var item in CategoryChannel)
                {
                    if (item.title.Contains(mi.Text))
                    {
                        TextHeadChannel.Text = mi.Text;
                        callapiGetHomeStream(int.Parse(item.id));
                        return;
                    }
                }

            }
        }

        private void CreateLiveTile(HubTile hubtile)
        {
            StandardTileData LiveTile = new StandardTileData
            {
                BackgroundImage = ((System.Windows.Media.Imaging.BitmapImage)hubtile.Source).UriSource,
                Title = hubtile.Title,
                BackTitle = hubtile.Title,
                BackContent = hubtile.Message
            };
        }

        
    }

}