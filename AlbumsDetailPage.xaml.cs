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

    public partial class AlbumsDetailPage : PhoneApplicationPage
    {
        int total = 0;
        int ContentID = 0;
        int AlbumID = 0;
        static WebClient WebClient;

        ObservableCollection<AlbumItem> List;

        static string url = null;

        public AlbumsDetailPage()
        {
            InitializeComponent();
        }


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EpisodeItem data = (sender as ListBox).SelectedItem as EpisodeItem;

            if (ListBox.SelectedIndex != -1)
            {
                //this.NavigationService.Navigate(new Uri("/ChapterPage.xaml?ContentID=" + data.ContentID + "&Title=" + data.Title, UriKind.Relative));
            }
            ListBox.SelectedIndex = -1;

        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            List = new ObservableCollection<AlbumItem>();

            HClusivePanorama.Title = this.NavigationContext.QueryString["Title"];
            PanoramaItem.Header = "อัลบั้มภาพ";

            if (int.TryParse(this.NavigationContext.QueryString["ContentID"], out ContentID) && int.TryParse(this.NavigationContext.QueryString["AlbumID"], out AlbumID))
            {
                if (ContentID != 0 && AlbumID != 0)
                {
                    this.callapiAlbum(ContentID, AlbumID);
                }
            }

            ListBox.ItemsSource = List;

            //reset itemList
            ListBox.SelectedIndex = -1;
        }

        public void callapiAlbum(int ContentID, int AlbumID)
        {
            WebClient = new WebClient();
            SetLoadingEpisodeListVisibility(true);
            String urlApi = null;
            url = null;
            try
            {
               
                if (ContentID != 0)
                {
                    url = "http://mstage.truelife.com/api_movietv/drama/gallery?method=getalbum&content_id=xxxx&album_id=yyyy";
                    url = url.Replace("xxxx", Convert.ToString(ContentID));
                    url = url.Replace("yyyy", Convert.ToString(AlbumID));
                    urlApi = url;

                    Debug.WriteLine(urlApi);
                    WebClient.DownloadStringAsync(new Uri(urlApi));
                    WebClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(GetList_Completed);
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

        public void GetList_Completed(object sender, DownloadStringCompletedEventArgs e)
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
                    total = XmlValueParser.ParseInteger(o.Root.Element("entry").Element("total"));

                    foreach (var v in o.Descendants("item"))
                    {
                        if (v.Element("pic_id") != null)
                        {
                            AlbumItem item = new AlbumItem();
                            item.pic_title = v.Element("pic_title").Value;
                            item.pic_thumbnail = v.Element("pic_thumbnail").Value;

                            List.Add(item);
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
                Debug.WriteLine("AlbumPage.xaml.cs : GetList_Completed ; " + ex.Message);
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
                loadingLabel_List.Visibility = Visibility.Visible;
                loadingProgressBar_List.IsIndeterminate = true;
            }
            else
            {
                loadingLabel_List.Visibility = Visibility.Collapsed;
                loadingProgressBar_List.IsIndeterminate = false;

                SystemTray.SetProgressIndicator(this, null);
            }
        }

    }

    
}