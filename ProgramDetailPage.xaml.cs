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
    public partial class ProgramDetailPage : PhoneApplicationPage
    {
        LiveStream stream = new LiveStream();
        ProgramDetailItem programDetailItem = new ProgramDetailItem();
        ObservableCollection<ProgramDetailItem> ChapterlList = new ObservableCollection<ProgramDetailItem>();
        public string ContentID = null;
        public string ParentID = null;
        public string url = null;
        ProgressIndicator progressIndicator;

        public ProgramDetailPage()
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
                ContentID = this.NavigationContext.QueryString["ContentID"];

                if (ContentID != null)
                {
                    this.read_api(ContentID);
                }

                ToolBox.ItemsSource = ChapterlList;
            }

            ToolBox.SelectedIndex = -1;

            base.OnNavigatedTo(e);
        }

        public void read_api(string content_id)
        {
            WebClient myService = new WebClient();
            String urlApi = null;
            try
            {
                if (content_id != null)
                {
                    ShowProgressIndicator("Loading...");

                    urlApi = "http://mstage.truelife.com/api_movietv/program/rerunchapter?method=getchapter&content_id=";

                    urlApi = urlApi + content_id;

                    Debug.WriteLine(urlApi);

                    myService.DownloadStringAsync(new Uri(urlApi));
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
             

                if (o.Root.Element("status_code").Value == "200")
                {
                    foreach (var v in o.Descendants("content"))
                    {
                        programDetailItem = new ProgramDetailItem();
                        programDetailItem.content_id = v.Element("content_id").Value;
                        programDetailItem.title = v.Element("title").Value;
                        programDetailItem.description = v.Element("description").Value;
                        programDetailItem.thumbnail = v.Element("thumbnail").Value;
                        programDetailItem.embed_code = v.Element("embed_code").Value;
                        programDetailItem.view = v.Element("view").Value;

                        ChapterlList.Add(programDetailItem);
                    }

                    textHead.Text = programDetailItem.title;
                    viewnum.Text = programDetailItem.view;

                    descriptionLabel.Visibility = Visibility.Visible;
                    descriptionLabel.NavigateToString(eNewsDetailPage.StripTagsRegex(programDetailItem.embed_code));
                }
                else
                {
                    throw new Exception("code is " + o.Root.Element("status_code").Value);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ProgramDetailPage.xaml.cs : GetTotal_Completed ; " + ex.Message);
                this.NavigationService.GoBack();
                NavigationService.Navigate(new Uri("/MainPage.xaml?Refresh=true", UriKind.Relative));
            }
            finally
            {
                HideProgressIndicator();
            }
        }

        private void ToolBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProgramDetailItem data = (sender as ListBox).SelectedItem as ProgramDetailItem;
            if (ToolBox.SelectedIndex != -1)
            {
                textHead.Text = data.title;
                viewnum.Text = data.view;
                descriptionLabel.NavigateToString(eNewsDetailPage.StripTagsRegex(data.embed_code));
            }
           
        }
    }
}