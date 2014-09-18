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
    public partial class MusicPage : PhoneApplicationPage
    {

        ObservableCollection<MusicItem> List = new ObservableCollection<MusicItem>();
        public string ContentID = null;
        public string url = null;

        public MusicPage()
        {
            InitializeComponent();
        }

        private void SetLoadingPanelVisibility(bool isVisible)
        {
            if (isVisible)
            {
                LoadingPanel.Visibility = Visibility.Visible;
                performanceProgressBarCustomized.IsIndeterminate = true;

            }
            else
            {
                LoadingPanel.Visibility = Visibility.Collapsed;
                performanceProgressBarCustomized.IsIndeterminate = false;
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ContentID = this.NavigationContext.QueryString["ContentID"];

            if (ContentID != null)
            {
                this.read_api(ContentID);
            }

           
        }

        public void read_api(string content_id)
        {
            WebClient myService = new WebClient();
            url = null;
            try
            {
                if (content_id != null)
                {
                    SetLoadingPanelVisibility(true);

                    url = "http://mstage.truelife.com/api_movietv/drama/music?method=getinfo&content_id=";


                    Debug.WriteLine(url + content_id);

                    myService.DownloadStringAsync(new Uri(url + content_id));
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
                    if (o.Root.Element("entry").Value == "")
                    {
                        MessageBox.Show("ไม่พบข้อมูล กรุณาลองใหม่อีกครั้งภายหลัง");
                        this.NavigationService.GoBack();
                    }
                    var itemEntry = o.Root.Element("entry");
                    MusicItem musicEntry = new MusicItem();
                    musicEntry.content_title = itemEntry.Element("content_title").Value;
                    musicEntry.description = itemEntry.Element("description").Value;
                    musicEntry.thumbnail = itemEntry.Element("thumbnail").Value;
                    musicEntry.rating = itemEntry.Element("rating").Value;
                    musicEntry.view = itemEntry.Element("view").Value;

                    List.Add(musicEntry);




                    textHead.Text = musicEntry.content_title;
                    viewnum.Text = musicEntry.view;
                    descriptionLabel.Visibility = Visibility.Visible;
                    descriptionLabel.NavigateToString(eNewsDetailPage.StripTagsRegex(musicEntry.description));

                }
                else
                {
                    throw new Exception("code is " + o.Root.Element("status_code").Value);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ArchivePage.xaml.cs : GetTotal_Completed ; " + ex.Message);
                this.NavigationService.GoBack();
                NavigationService.Navigate(new Uri("/MainPage.xaml?Refresh=true", UriKind.Relative));
            }
            finally
            {
                SetLoadingPanelVisibility(false);
            }
        }
    }
}