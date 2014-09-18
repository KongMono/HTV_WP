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

namespace News
{
    public partial class DaraNewsDetailPage : PhoneApplicationPage
    {
        public int page = 0;
        public int parent_id = 0;
        public int content_id = 0;
        public String Share_Url = "";
        public String Share_Image = "";
        public String Share_Description = "";
        public String AccessToken = "";
        public String SSOID = "";
        public String Username = "";
        public String UserImage = "";
        public string url = null;
        const string url_share = "http://tv.truelife.com/detail/yyyy/drama/news/xxxx";
        ProgressIndicator progressIndicator = new ProgressIndicator();
        public DaraNewsDetailPage()
        {
            InitializeComponent();
            ApplicationBar.ForegroundColor = (Application.Current as App).AppBarForeground;
            ApplicationBar.BackgroundColor = (Application.Current as App).AppBarBackground;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MylifeLike.Instance.ActiveLikeContent(content_id, eNewsTitle.Title.ToString(), Share_Url, Share_Image, this);
        }

        private void btn_share(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ShareContentPage.xaml?content_id=" + this.content_id + "&title=" + Share_Description + "&description=" + Share_Description + "&url=" + Share_Url + "&img=" + Share_Image, UriKind.Relative));
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

            if (int.TryParse(this.NavigationContext.QueryString["ContentID"], out content_id) && int.TryParse(this.NavigationContext.QueryString["ParentID"], out parent_id))
            {
                if (content_id != 0 && parent_id!= 0)
                {
                    this.read_api(content_id,parent_id);
                }
            }

           
            Share_Image = this.NavigationContext.QueryString["Pic"];
        }

        public void read_api(int content_id,int parent_id)
        {
            WebClient myService = new WebClient();
            string url_api = null;
            url = null; 
            try
            {
                if (content_id != 0)
                {
                    ShowProgressIndicator("Loading...");

                    if (int.TryParse(this.NavigationContext.QueryString["Page"], out page))
                    {
                        switch (page)
                        {
                            case 3:
                                PivotItem.Header = "เกร็ดละคร";
                                url = "http://mstage.truelife.com/api_movietv/drama/quoteinfo?method=getinfo&content_id=xxxx&parent_id=yyyy";
                                GoogleAnalytics.EasyTracker.GetTracker().SendView((Application.Current as App).pathAnalytic.Replace("title", "เกร็ดละคร").Replace("level", "D"));
                                break;
                            case 5:
                                PivotItem.Header = "ข่าวละคร";
                                url = "http://mstage.truelife.com/api_movietv/drama/newsinfo?method=getinfo&content_id=xxxx&parent_id=yyyy";
                                GoogleAnalytics.EasyTracker.GetTracker().SendView((Application.Current as App).pathAnalytic.Replace("title", "ข่าวละคร").Replace("level", "D"));
                                break;

                        }
                    }
                    url = url.Replace("xxxx", Convert.ToString(content_id));
                    url = url.Replace("yyyy", Convert.ToString(parent_id));
                    url_api = url;
                    Debug.WriteLine(url_api);
                    myService.DownloadStringAsync(new Uri(url_api));
                    myService.DownloadStringCompleted += new DownloadStringCompletedEventHandler(Get_Detail_Completed);
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

        public void Get_Detail_Completed(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    throw new Exception(e.Error.Message);
                }

                var o = XDocument.Parse(e.Result);
                Debug.WriteLine(o.ToString());

                if (o.Root.Element("status_code").Value == "200" && o.Root.Element("entry").Value != null)
                {
                    if (o.Root.Element("entry").Value == "")
                    {
                        MessageBox.Show("ไม่พบข้อมูล กรุณาลองใหม่อีกครั้งภายหลัง");
                        this.NavigationService.GoBack();
                    }

                    var item = o.Root.Element("entry");

                    imageDetail.Source = new BitmapImage(new Uri(this.NavigationContext.QueryString["Pic"], UriKind.Absolute));
                    textDetail.Text = item.Element("description").Value;

                    //Share
                    url = url.Replace("xxxx", Convert.ToString(content_id));
                    url = url.Replace("yyyy", Convert.ToString(parent_id));
                    Share_Url = url; 
                    Share_Description = item.Element("content_title").Value;
                    Debug.WriteLine("Share : " + Share_Url + "----" + Share_Image);
                    
                    // Get Comment
                    Debug.WriteLine("Get Comment id =" + this.content_id);


                }
                else
                {
                    throw new Exception("code is " + o.Root.Element("status_code").Value);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("eNewsDetailPage.xaml.cs : Get_Detail_Completed ; " + ex.Message);
                MessageBox.Show("เกิดเหตุขัดข้อง กรุณาลองใหม่อีกครั้งภายหลัง");
                this.NavigationService.GoBack();
            }

            HideProgressIndicator();
        }

    }
}