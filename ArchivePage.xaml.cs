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
    public partial class ArchivePage : PhoneApplicationPage
    {
        LiveStream stream = new LiveStream();
        ArchivePageItem archivePageItem = new ArchivePageItem();
        ObservableCollection<ChapterArchivePageItem> ChapterlList = new ObservableCollection<ChapterArchivePageItem>();
        public string ContentID = null;
        public string ParentID = null;
        public string url = null;
        ProgressIndicator progressIndicator;

        public ArchivePage()
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
            base.OnNavigatedTo(e);
            ContentID = this.NavigationContext.QueryString["ContentID"];

            if (ContentID != null)
            {
                this.read_api(ContentID);
            }

            ToolBox.ItemsSource = ChapterlList;
        }

        public void read_api(string content_id)
        {
            WebClient myService = new WebClient();
            String urlApi = null;
            url = null;
            try
            {
                if (content_id != null)
                {
                    ShowProgressIndicator("Loading...");

                    url = "http://mstage.truelife.com/api_movietv/drama/archive?method=getinfo&content_id=xxxx&parent_id=yyyy";
                    url = url.Replace("xxxx", Convert.ToString(content_id));
                    url = url.Replace("yyyy", Convert.ToString(ContentID));
                    urlApi = url;

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
                    archivePageItem = new ArchivePageItem();
                    ChapterlList.Clear();
                    archivePageItem.content_title = itemEntry.Element("content_title").Value;
                    archivePageItem.description = itemEntry.Element("description").Value;
                    archivePageItem.stream = itemEntry.Element("stream").Value;
                    archivePageItem.thumbnail = itemEntry.Element("thumbnail").Value;
                    archivePageItem.parent_id = itemEntry.Element("parent_id").Value;
                    archivePageItem.share_url = itemEntry.Element("share_url").Value;
                    archivePageItem.view = itemEntry.Element("view").Value;

                    
                    if (o.Root.Element("chapter") != null)
                    {
                        archivePageItem.ChapterArchivePageDetailList = new List<ChapterArchivePageItem>();

                        foreach (var v in o.Root.Element("chapter").Descendants("item"))
                        {
                            ChapterArchivePageItem item = new ChapterArchivePageItem();
                            item.chapter_id = XmlValueParser.ParseInteger(v.Element("chapter_id"));
                            item.chapter_title = v.Element("chapter_title").Value;
                            item.thumbnail = v.Element("thumbnail").Value;
                            item.parent_id = v.Element("parent_id").Value;
                            archivePageItem.ChapterArchivePageDetailList.Add(item);
                            ChapterlList.Add(item);
                        }
                    }

                    textHead.Text = archivePageItem.content_title;
                    viewnum.Text = archivePageItem.view;
                    descriptionLabel.Visibility = Visibility.Visible;
                    descriptionLabel.NavigateToString(eNewsDetailPage.StripTagsRegex(archivePageItem.stream));

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
                HideProgressIndicator();
            }
        }

        private void ToolBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChapterArchivePageItem data = (sender as ListBox).SelectedItem as ChapterArchivePageItem;
            if (ToolBox.SelectedIndex != -1)
            {
                read_api(Convert.ToString(data.chapter_id));
            }
            ToolBox.SelectedIndex = -1;
        }
    }
}