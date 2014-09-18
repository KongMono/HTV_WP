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
    public partial class SynopsisDetailPage : PhoneApplicationPage
    {

        public int content_id = 0;
        public String Share_Url = "";
        public String Share_Image = "";
        public String Share_Description = "";
        public String AccessToken = "";
        public String SSOID = "";
        public String Username = "";
        public String UserImage = "";

        const string UrlAPI = "http://mstage.truelife.com/api_movietv/drama/synopsisinfo?method=getinfo&content_id=";
        const string URL_SHARE = "http://mstage.truelife.com/movietv/eupdate/detail/";

        public SynopsisDetailPage()
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

            NavigationService.Navigate(new Uri("/ShareContentPage.xaml?content_id=" + this.content_id + "&title=" + eNewsTitle.Title + "&description=" + Share_Description + "&url=" + Share_Url + "&img=" + Share_Image, UriKind.Relative));
        }

        private void SetLoadingPanelVisibility(bool isVisible)
        {
            if (isVisible)
            {
                LoadingPanel.Visibility = Visibility.Visible;
                loadingProgressBar.IsIndeterminate = true;

                ApplicationBar.IsVisible = false;
            }
            else
            {
                LoadingPanel.Visibility = Visibility.Collapsed;
                loadingProgressBar.IsIndeterminate = false;

                ApplicationBar.IsVisible = true;
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (int.TryParse(this.NavigationContext.QueryString["ContentID"], out content_id))
            {
                if (content_id != 0)
                {
                    this.read_api(content_id);
                }
            }

            GoogleAnalytics.EasyTracker.GetTracker().SendView((Application.Current as App).pathAnalytic.Replace("title", "เรื่องย่อละคร").Replace("level", "D"));
        }

        public void read_api(int content_id)
        {
            WebClient myService = new WebClient();

            try
            {
                if (content_id != 0)
                {
                    SetLoadingPanelVisibility(true);

                    Debug.WriteLine(UrlAPI + content_id);
                    myService.DownloadStringAsync(new Uri(UrlAPI + content_id));
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

                    //eNewsTitle.Title = item.Element("title").Value;

                    //titleLabel.Text = item.Element("title").Value;
                    //imageDetail.Source = new BitmapImage(new Uri(item.Element("thumbnail").Value, UriKind.Absolute));

                    // Send description to WebBrowser

                    //Debug.WriteLine("StripTagsRegex : " + StripTagsRegex(item.Element("description").Value));
                    //Debug.WriteLine("StripTagsRegexCompiled : " + StripTagsRegexCompiled(item.Element("description").Value));
                    // Debug.WriteLine("StripTagsCharArray : " + StripTagsCharArray(item.Element("description").Value));

                    descriptionLabel.NavigateToString(StripTagsRegex(item.Element("description").Value));

                    //Share
                    Share_Url = URL_SHARE + this.content_id;
                    Share_Image = item.Element("thumbnail").Value;
                    Share_Description = item.Element("title").Value;

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

            SetLoadingPanelVisibility(false);
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
            }
        }

        // Function String Tags //
        public static string StripTagsRegex(string source)
        {
            //return Regex.Replace(source, "<.*?>", string.Empty);
            // cut iframe
            //String resize_text = "src=\"" + ResizeImage.Instance.GetResizeAutoHeight(300);
            String resize_text = ResizeImage.Instance.GetResizeAutoHeight(300);
            String pre = @"<html><meta name='viewport' content='initial-scale=0,user-scalable=no,width=device-width'><style>body{color:#EFECE9 !important;background-color:black;font-weight:normal !important; font-size: 18px !important;}a,p,span,strong{font-size: 18px !important; color:#EFECE9 !important;}A:link{color:#EFECE9 !important;}
                         A:visited{color:white !important;}</style><body>";
            String last = "</body></html>";
            String center = source;
            //String center = Regex.Replace(source, "<a .*?>", string.Empty);
            //center = Regex.Replace(center, "<iframe .*?>", string.Empty);

            //Check iFrame
            foreach (Match i in Regex.Matches(center, @"<iframe.*src=\""(.*?)\"""))
            {
                Match match = Regex.Match(i.Groups[1].Value, @"youtube", RegexOptions.IgnoreCase);

                // Here we check the Match instance.
                if (match.Success)
                {
                    var youtube_url = i.Groups[1].Value;
                    //var p = i.Groups[1].Value;
                    var index = youtube_url.IndexOf('?');
                    if (index != -1 && index < youtube_url.Length)
                    {
                        try
                        {
                            var s = Uri.UnescapeDataString(youtube_url.Substring(0, index)).Split('/');
                            youtube_url = "http://youtube.com/embed/" + s[4];
                        }
                        catch { }
                    }

                    // Finally, we get the Group value and display it.
                    center = Regex.Replace(source, "<iframe.*></iframe>", "<a href=\"" + youtube_url + "\">คลิกชมวิดีโอจาก Youtube</a>");
                    //center = Regex.Replace(center, i.Groups[1].Value + "." + i.Groups[2].Value, resize_text + m.Groups[1].Value + "." + m.Groups[2].Value);
                    Debug.WriteLine("Regex.Matches = " + youtube_url);
                }
                else
                {
                    center = Regex.Replace(source, "<iframe.*></iframe>", string.Empty);
                    Debug.WriteLine("Not youtube!!");
                }


                Debug.WriteLine("center = " + center);
            }


            center = Regex.Replace(center, @"width=""[^\s]*""", "");
            center = Regex.Replace(center, @"height=""[^\s]*""", "");
            //center = Regex.Replace(center, @"height=""[^\s]*""", "height=\"auto\"");

            foreach (Match m in Regex.Matches(center, "src=\"(\\S+?)\\.(jpg|png|bmp)"))
            {
                center = Regex.Replace(center, m.Groups[1].Value + "." + m.Groups[2].Value, resize_text + m.Groups[1].Value + "." + m.Groups[2].Value);
                //Debug.WriteLine("Regex.Matches = " + m.Groups[1].Value + "." + m.Groups[2].Value);
            }
            //Debug.WriteLine("center = " + pre + center + last);
            return pre + center + last;

        }

        /// <summary>
        /// Compiled regular expression for performance.
        /// </summary>
        static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

        /// <summary>
        /// Remove HTML from string with compiled Regex.
        /// </summary>
        public static string StripTagsRegexCompiled(string source)
        {
            return _htmlRegex.Replace(source, string.Empty);
        }

        public static string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

        public static string GetContent(string strRequest)
        {

            // Remove HTML Tag
            Regex regex = new Regex(@"\s]+))?)+\s*|\s*)/?>", RegexOptions.Singleline);
            MatchCollection matches = regex.Matches(strRequest);
            foreach (Match match in matches)
            {
                int begin = strRequest.IndexOf(match.Value);
                strRequest = strRequest.Remove(begin, match.Value.Length);
            }
            return strRequest;
        }

        private void descriptionLabel_Navigating_1(object sender, NavigatingEventArgs e)
        {
            MessageBox.Show(e.Uri.ToString());
        }

    }
}