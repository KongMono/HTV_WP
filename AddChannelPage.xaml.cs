using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Diagnostics;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace News
{

    public partial class AddChannelPage : PhoneApplicationPage
    {
        private WebClient webClient;
        MediaHighlightItem item = new MediaHighlightItem();
        bool selected = false;
        ObservableCollection<MediaHighlightItem> AddChannelAllList = new ObservableCollection<MediaHighlightItem>();
        const string url_ChannelAll = "http://api-movie.truelife.com/wrap_api/mod/tv/livetv?method=getlist&category=1&offset=0&limit=100";
        int SelectCount = 0;
        ProgressIndicator progressIndicator = new ProgressIndicator();
        public AddChannelPage()
        {
            InitializeComponent();
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

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MediaHighlightItem data = (sender as ListBox).SelectedItem as MediaHighlightItem;
           
            if (ListBox.SelectedIndex != -1)
            {
                if (AddChannelAllList[ListBox.SelectedIndex].PicSelected.Equals("Assets/btn_select_active.png"))
                {
                    AddChannelAllList[ListBox.SelectedIndex].PicSelected = "Assets/btn_select.png";
                    SelectCount--;
                }
                else
                {
                    int i = 1;
                    foreach (var item in AddChannelAllList)
                    {
                        if (item.PicSelected.Equals("Assets/btn_select_active.png"))
                        {
                            i++;
                        }
                    }

                    if (i > 20)
                    {
                        MessageBox.Show("คุณสามารถเลือกช่องรายการโปรดได้สูงสุด 20 ช่องค่ะ");
                        return;
                    }

                    AddChannelAllList[ListBox.SelectedIndex].PicSelected = "Assets/btn_select_active.png";
                    SelectCount++;
                }

                PanoramaItem.Header = "เพิ่มช่องโปรด (" + SelectCount + "/20)";

                this.ListBox.ItemsSource = AddChannelAllList;
            }

            ListBox.SelectedIndex = -1;

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {

            base.OnNavigatedTo(e);

            ShowProgressIndicator("Loading...");

            HClusivePanorama.Title = (Application.Current as App).AppName;

            this.callapi();
            ListBox.ItemsSource = AddChannelAllList;

            //reset itemList
            ListBox.SelectedIndex = -1;
        }

        public void callapi()
        {
            webClient = new WebClient();
            ShowProgressIndicator("Loading...");
            
            try
            {
                webClient.Headers["id"] = "liveTv";
                Debug.WriteLine("URL url_ChannelAll = " + url_ChannelAll);
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(GetList_Completed);
                webClient.DownloadStringAsync(new Uri(url_ChannelAll));

            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetList_Completed(object sender, DownloadStringCompletedEventArgs e)
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
                        AddChannelAllList.Clear();
                    }
                    //-----
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
                            selected = false;

                            for (int i = 0; i < (Application.Current as App).FavoriteIndexList.Count; i++)
                            {
                                if ((Application.Current as App).FavoriteIndexList[i].content_id.Equals(tmp_item.content_id))
                                {
                                    selected = true;
                                }
                            }

                            if (selected)
                            {
                                tmp_item.PicSelected = "Assets/btn_select_active.png";
                                SelectCount++;
                            }
                            else
                            {
                                tmp_item.PicSelected = "Assets/btn_select.png";
                            }
                            AddChannelAllList.Add(tmp_item);   
                        }

                        
                    }
                    PanoramaItem.Header = "เพิ่มช่องโปรด (" + SelectCount +"/20)";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("HomePage : MediaHighlightItemList_DownloadStringCompleted ; " + ex.Message);
            }
            HideProgressIndicator();
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

        }

        private void Save_Click(object sender, EventArgs e)
        {

            (Application.Current as App).FavoriteIndexList.Clear();

            foreach (var item in AddChannelAllList)
            {
                if (item.PicSelected.Equals("Assets/btn_select_active.png"))
                {
                    (Application.Current as App).FavoriteIndexList.Add(item);
                }
            }

            MessageBoxResult result = MessageBox.Show("เพิ่มช่องโปรดเรียบร้อยแล้วค่ะ",(Application.Current as App).AppName, MessageBoxButton.OK);

            if (result == MessageBoxResult.OK)
            {
                NavigationService.GoBack();
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }  
        }
    }

    
}