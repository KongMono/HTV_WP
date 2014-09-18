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

namespace News
{

    public partial class DaraNewsPage : PhoneApplicationPage
    {
        int page = 0;
        int total = 0;
        int ContentID = 0;
        static WebClient EpisodeListWebClient;
        EpisodeItem item = new EpisodeItem();
        ObservableCollection<EpisodeItem> EpisodetemList;
        ProgressIndicator progressIndicator = new ProgressIndicator();
        static string url = null;
       

        public DaraNewsPage()
        {
            InitializeComponent();
        }


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EpisodeItem data = (sender as ListBox).SelectedItem as EpisodeItem;
            if (ListBox.SelectedIndex != -1)
            {
                this.NavigationService.Navigate(new Uri("/DaraNewsDetailPage.xaml?ContentID=" + data.ContentID + "&ParentID=" + data.parent_id + "&Pic=" + data.ImagePath + "&Title=" + this.NavigationContext.QueryString["Title"] + "&Page=" + page, UriKind.Relative));
            }

            ListBox.SelectedIndex = -1;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            EpisodetemList = new ObservableCollection<EpisodeItem>();

            HClusivePanorama.Title = this.NavigationContext.QueryString["Title"];
            if (int.TryParse(this.NavigationContext.QueryString["Page"], out page))
            {
                switch (page)
                {
                    case 3:
                        PanoramaItem.Header = "เกร็ดละคร";
                        url = "http://mstage.truelife.com/api_movietv/drama/quote?method=getlist&content_id=";
                        break;
                    case 5:
                        PanoramaItem.Header = "ข่าวละคร";
                        url = "http://mstage.truelife.com/api_movietv/drama/news?method=getlist&content_id=";
                        break;
                   
                }
            }
            if (int.TryParse(this.NavigationContext.QueryString["ContentID"], out ContentID))
            {
                if (ContentID != 0)
                {
                    this.callapiEpisode(ContentID);
                }
            }

            ListBox.ItemsSource = EpisodetemList;

            //reset itemList
            ListBox.SelectedIndex = -1;
        }

        public void callapiEpisode(int ContentID)
        {
            EpisodeListWebClient = new WebClient();
            ShowProgressIndicator("Loading...");
            
            try
            {
               
                if (ContentID != 0)
                {
                    Debug.WriteLine(url + ContentID);
                    EpisodeListWebClient.DownloadStringAsync(new Uri(url + ContentID));
                    EpisodeListWebClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(GetListEpisode_Completed);
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

        public void GetListEpisode_Completed(object sender, DownloadStringCompletedEventArgs e)
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
                    total = XmlValueParser.ParseInteger(o.Root.Element("data").Element("qoute_total"));

                    foreach (var v in o.Descendants("item"))
                    {
                        if (v.Element("chapter_title") != null)
                        {
                            item = new EpisodeItem();
                            item.ContentID = XmlValueParser.ParseInteger(v.Element("chapter_id"));
                            item.Title = v.Element("chapter_title").Value;
                            item.ImagePath = v.Element("thumbnail").Value;
                            item.parent_id = v.Element("parent_id").Value;
                            EpisodetemList.Add(item);
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
                Debug.WriteLine("DaraNewsPage.xaml.cs : callapiEpisode ; " + ex.Message);
                MessageBox.Show("เกิดเหตุขัดข้อง กรุณาลองใหม่อีกครั้งภายหลัง");
                this.NavigationService.GoBack();
                NavigationService.Navigate(new Uri("/MainPage.xaml?Refresh=true", UriKind.Relative));
            }

            HideProgressIndicator();
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

    }

    
}