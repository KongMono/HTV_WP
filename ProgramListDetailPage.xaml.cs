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
    public partial class ProgramListDetailPage : PhoneApplicationPage
    {

        public string content_id = null;
        ObservableCollection<ProgramDetailItem> ProgramDetailList = new ObservableCollection<ProgramDetailItem>();
        const string UrlApiDetail = "http://mstage.truelife.com/api_movietv/program/rerunepisode?method=getepisode&content_id=";
        ProgressIndicator progressIndicator;

        public ProgramListDetailPage()
        {
            InitializeComponent();
            progressIndicator = new ProgressIndicator();
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
           

            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
            {
                //call data
                content_id = this.NavigationContext.QueryString["ContentID"];
                if (content_id != null)
                {
                    this.read_api(content_id);
                }

                ShowProgressIndicator("Loading...");

                HClusivePanorama.Title = "รายการย้อนหลัง";
                PanoramaItem.Header = this.NavigationContext.QueryString["Title"];
                GoogleAnalytics.EasyTracker.GetTracker().SendView((Application.Current as App).pathAnalytic.Replace("title", this.NavigationContext.QueryString["Title"]).Replace("level", "D"));

                ProgramListBox.ItemsSource = ProgramDetailList;

            }

            //reset data
            ProgramListBox.SelectedIndex = -1;
            

            base.OnNavigatedTo(e);

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
                var o = XDocument.Parse(e.Result);

                if (o.Root.Element("status_code").Value == "200")
                {
                    foreach (var v in o.Descendants("content"))
                    {
                        ProgramDetailItem item = new ProgramDetailItem();
                        item.content_id = v.Element("content_id").Value;
                        item.title = v.Element("title").Value;
                        item.thumbnail = v.Element("thumbnail").Value;
                        item.description = v.Element("description").Value;
                        item.chapter_total = v.Element("chapter_total").Value;
                        item.view = v.Element("view").Value;
                      
                        ProgramDetailList.Add(item);
                    }
                }
                else
                {
                    throw new Exception("code is " + o.Root.Element("status_code").Value);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ShotDaraDetail.xaml.cs : GetTotal_Completed ; " + ex.Message);
                this.NavigationService.GoBack();
                NavigationService.Navigate(new Uri("/ShotDaraDetail.xaml?Refresh=true", UriKind.Relative));
            }

            HideProgressIndicator();

        }

        private void ProgramListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListBox).SelectedIndex >= 0)
            {
                ProgramDetailItem item = (sender as ListBox).SelectedItem as ProgramDetailItem;
                this.NavigationService.Navigate(new Uri("/ProgramDetailPage.xaml?ContentID=" + item.content_id, UriKind.Relative));
            }

            ProgramListBox.SelectedIndex = -1;
        }

    }
}