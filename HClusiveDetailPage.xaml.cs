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

    public partial class HClusiveDetailPage : PhoneApplicationPage
    {
        int total = 0;
        int ContentID = 0;
        static WebClient EpisodeListWebClient;

        ObservableCollection<EpisodeItem> EpisodetemList;

        const string url_Episode = "http://mstage.truelife.com/api_movietv/novel/getlist?method=getlist&content_id=";

        public HClusiveDetailPage()
        {
            InitializeComponent();
        }


        private void HClusiveListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EpisodeItem data = (sender as ListBox).SelectedItem as EpisodeItem;

            if (HClusiveListBox.SelectedIndex != -1)
            {
                this.NavigationService.Navigate(new Uri("/ChapterPage.xaml?ContentID=" + data.ContentID + "&Title=" + data.Title, UriKind.Relative));
            }

            HClusiveListBox.SelectedIndex = -1;

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            EpisodetemList = new ObservableCollection<EpisodeItem>();

            String title  = this.NavigationContext.QueryString["Title"];
            string lastPart = title.Split(' ').Last();
            HClusivePanorama.Title = lastPart;

            if (int.TryParse(this.NavigationContext.QueryString["ContentID"], out ContentID))
            {
                if (ContentID != 0)
                {
                    this.callapiEpisode(ContentID);
                }
            }

            HClusiveListBox.ItemsSource = EpisodetemList;

            //reset itemList
            HClusiveListBox.SelectedIndex = -1;
        }

        public void callapiEpisode(int ContentID)
        {
            EpisodeListWebClient = new WebClient();
            SetLoadingEpisodeListVisibility(true);
            
            try
            {
               
                if (ContentID != 0)
                {
                    Debug.WriteLine(url_Episode + ContentID);
                    EpisodeListWebClient.DownloadStringAsync(new Uri(url_Episode + ContentID));
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
                    total = XmlValueParser.ParseInteger(o.Root.Element("data").Element("total"));

                    foreach (var v in o.Descendants("item"))
                    {
                        total = XmlValueParser.ParseInteger(o.Root.Element("data").Element("total"));
                        EpisodeItem item = new EpisodeItem();

                        item.Title = v.Element("title").Value;
                        item.ImagePath = v.Element("thumbnail").Value;
                        item.ContentID = XmlValueParser.ParseInteger(v.Element("id"));

                        EpisodetemList.Add(item);
                    }

                }
                else
                {
                    throw new Exception("code is " + o.Root.Element("status_code").Value);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("eNewsPage.xaml.cs : Get_XML1_Completed ; " + ex.Message);
                MessageBox.Show("เกิดเหตุขัดข้อง กรุณาลองใหม่อีกครั้งภายหลัง");
                this.NavigationService.GoBack();
                NavigationService.Navigate(new Uri("/MainPage.xaml?Refresh=true", UriKind.Relative));
            }

            SetLoadingEpisodeListVisibility(false);
        }

         private void SetLoadingEpisodeListVisibility(bool isVisible)
        {
            if (isVisible)
            {
                loadingLabel_HClusiveList.Visibility = Visibility.Visible;
                loadingProgressBar_HClusiveList.IsIndeterminate = true;
            }
            else
            {
                loadingLabel_HClusiveList.Visibility = Visibility.Collapsed;
                loadingProgressBar_HClusiveList.IsIndeterminate = false;

                SystemTray.SetProgressIndicator(this, null);
            }
        }

    }

    
}