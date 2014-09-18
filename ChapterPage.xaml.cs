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
    public partial class ChapterPage : PhoneApplicationPage
    {
        
        public int totalPage = 0;
        public int content_id = 0;
        Boolean check = true;
        ObservableCollection<ChaptersItem> EpisodeItemList = new ObservableCollection<ChaptersItem>();
        ProgressIndicator progressIndicator = new ProgressIndicator();
        const string UrlTotalPageAPI = "http://mstage.truelife.com/api_movietv/novel/novelinfo?method=getinfo&page=1&content_id=";
        const string UrlPageAPI = "http://mstage.truelife.com/api_movietv/novel/novelinfo?method=getinfo";

        public ChapterPage()
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

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
            {
                if (int.TryParse(this.NavigationContext.QueryString["ContentID"], out content_id))
                {
                    if (content_id != 0)
                    {
                        this.read_api(content_id);
                    }
                }

                GoogleAnalytics.EasyTracker.GetTracker().SendView((Application.Current as App).pathAnalytic.Replace("title", this.NavigationContext.QueryString["Title"]).Replace("level", "D"));
            }
        }

        public void read_api(int content_id)
        {
            WebClient myService = new WebClient();

            try
            {
                if (content_id != 0)
                {
                    ShowProgressIndicator("Loading...");

                    Debug.WriteLine(UrlTotalPageAPI + content_id);
                    myService.DownloadStringAsync(new Uri(UrlTotalPageAPI + content_id));
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
                ChaptersItem item = new ChaptersItem();

                if (o.Root.Element("status_code").Value == "200")
                {
                    item.total_page = XmlValueParser.ParseInteger(o.Root.Element("data").Element("total_page"));
                    item.now_page = XmlValueParser.ParseInteger(o.Root.Element("data").Element("now_page"));
                    item.section = XmlValueParser.ParseString(o.Root.Element("data").Element("section"));
                    foreach (var v in o.Descendants("items"))
                    {

                        item.description = v.Element("description").Value;
                        item.view = v.Element("views").Value;

                        if (check)
                        {
                            List<string> dataSource = new List<string>();

                            for (int i = 1; i <= item.total_page; i++)
                            {
                                dataSource.Add("Page " + Convert.ToString(i) + "/" +  Convert.ToString(item.total_page));                               
                            }

                            lstPicker.ItemsSource = dataSource;
                         
                            check = false;
                        }
                        
                        descriptionLabel.NavigateToString(StripTagsRegex(item.description));
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
                this.NavigationService.GoBack();
                NavigationService.Navigate(new Uri("/MainPage.xaml?Refresh=true", UriKind.Relative));
            }

            HideProgressIndicator();

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
                    center = Regex.Replace(source, "<iframe.*></iframe>", "<a href=\"" + youtube_url + "\">??????????????? Youtube</a>");
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

        private void sightingTypesPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListPicker).SelectedIndex >= 0)
            {
                var picker = sender as ListPicker;
                WebClient myService = new WebClient();
                string url = null;
                try
                {
                    if (content_id != 0)
                    {
                        ShowProgressIndicator("Loading...");
                        url = (UrlPageAPI + "&content_id=" + content_id + "&page=" + picker.SelectedItem.ToString().Substring(5, picker.SelectedItem.ToString().IndexOf("/") - 5));
                        Debug.WriteLine(url);
                        myService.DownloadStringAsync(new Uri(url));
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
        }


    }
}