using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Microsoft.Phone.Tasks;

namespace News
{
    public partial class ShareContentPage : PhoneApplicationPage
    {
        const string URL_SHARE = "http://api.mylife.truelife.com/social/share.xml";
        const string URL_CHECK_SOCIAL = "http://api.mylife.truelife.com/users/profile/profile.xml?response_group=social";
        const string PREFIX_COMMENT = "cms_";
        const string URL_CHECK_IP = "http://mstage.truelife.com/remote_addr.php?format=xml";

        private String IP_Address = "0";
        private int ContentID = 0;
        private String ShareTitle;
        public String Url;
        public String Image;
        public bool facebook_status = false;
        public bool twitter_status = false;
        public TextBox ShareLabelName;
        public String ShareText = "";
        public String Share_Url = "";
        public String Share_Description = "";
        public String Share_Image = "";
        public String AccessToken = "";
        public String SSOID = "";
        public String Username = "";
        public String UserImage = "";

        //Share 
        public String Share_Mylife = "mylife";
        public String Share_Facebook = "";
        public String Share_Twitter = "";
        public String Service_Share = "";

        private String Result_Mylife = "";
        private String Result_Facebook = "";
        private String Result_Twitter = "";

        private bool Status_Mylife = false;
        private bool Status_Facebook = false;
        private bool Status_Twitter = false;

        String ApiKey = "b00bcd679d7b7bb6a3262db12c70ec0f";
        String CommentType = "ext_content";
        String ExtType = "article";
        
        

        public ShareContentPage()
        {
            InitializeComponent();


            //if (Truelife.Instance.GetLoginData().sso_access_token != null && Truelife.Instance.GetLoginData().sso_uid != null)
            //{
            //    AccessToken = Truelife.Instance.GetLoginData().sso_access_token;
            //    SSOID = Truelife.Instance.GetLoginData().sso_uid;
            //    Username = Truelife.Instance.GetLoginData().name;
            //    UserImage = "http://image.platform.truelife.com/" + SSOID + "/avatar?key=1";
            //}

            //CheckPermissionSocial();
            CheckIP();

            this.fb.Checked += new EventHandler<RoutedEventArgs>(fb_Checked);
            this.fb.Unchecked += new EventHandler<RoutedEventArgs>(fb_Unchecked);
            this.fb.IsChecked = true;

            //this.tw.Checked += new EventHandler<RoutedEventArgs>(tw_Checked);
            //this.tw.Unchecked += new EventHandler<RoutedEventArgs>(tw_Unchecked);

            //this.toggle.Click += new EventHandler<RoutedEventArgs>(toggle_Click);
            //this.toggle.Indeterminate += new EventHandler<RoutedEventArgs>(toggle_Indeterminate);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
             base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("content_id") && NavigationContext.QueryString.ContainsKey("title") && NavigationContext.QueryString.ContainsKey("description") && NavigationContext.QueryString.ContainsKey("url") && NavigationContext.QueryString.ContainsKey("img"))
            {
                int.TryParse(NavigationContext.QueryString["content_id"], out ContentID);
                NavigationContext.QueryString.TryGetValue("title", out ShareTitle);
                NavigationContext.QueryString.TryGetValue("description", out Share_Description);
                NavigationContext.QueryString.TryGetValue("url", out Share_Url);
                NavigationContext.QueryString.TryGetValue("img", out Image);
                Share_Image = Image;
                //MessageBox.Show( Title + "\n" + Url + "\n" + Image);
                titleLabel.Text = "Share";
                shareImgLabel.Source = new BitmapImage(new Uri(ResizeImage.Instance.GetResizeAutoWidth(250) + Image, UriKind.Absolute));
                Debug.WriteLine(ResizeImage.Instance.GetResizeAutoWidth(250) + Image);

                shareTextLabel.Text = Share_Description;
            }
        }

        void fb_Unchecked(object sender, RoutedEventArgs e)
        {
            facebook_status = false;
            Share_Facebook = "";
            //this.toggle.Content = "off";
            //this.toggle.SwitchForeground = new SolidColorBrush(Colors.Red);
        }

        void fb_Checked(object sender, RoutedEventArgs e)
        {
            facebook_status = true;
            Share_Facebook = "facebook";
            //this.toggle.Content = "on";
            //this.toggle.SwitchForeground = new SolidColorBrush(Colors.Green);
        }

        void tw_Unchecked(object sender, RoutedEventArgs e)
        {
            twitter_status = false;
            Share_Twitter = "";
            //this.toggle.Content = "off";
            //this.toggle.SwitchForeground = new SolidColorBrush(Colors.Red);
        }

        void tw_Checked(object sender, RoutedEventArgs e)
        {
            twitter_status = true;
            Share_Twitter = "twitter";
            //this.toggle.Content = "on";
            //this.toggle.SwitchForeground = new SolidColorBrush(Colors.Green);
        }

        public void Share_Click(object sender, EventArgs e)
        {

            MessageBoxResult m = MessageBox.Show("คุณต้องการแชร์คอนเทนท์ตอนนี้ใช่หรือไม่ ?", "", MessageBoxButton.OKCancel);
            if (m == MessageBoxResult.OK)
            {
                if (shareTextLabel.Text.Trim() != "")
                {
                    if (shareTextLabel.Text.Trim().Count() <= 140)
                    {
                        ShareText = shareTextLabel.Text;

                        if (Share_Facebook != "")
                        {
                            postShareFacebook(ContentID, ShareTitle, Share_Description, ShareText, Share_Url, Share_Image);
                        }
                        else
                        {
                            Status_Facebook = true;
                        }

                        if (Share_Twitter != "")
                        {
                            postShareTwitter(ContentID, ShareTitle, Share_Description, ShareText, Share_Url, Share_Image);
                        }
                        else
                        {
                            Status_Twitter = true;
                        }

                        //Clear TextBox Comment & Result
                        shareTextLabel.Text = "";

                    }
                    else
                    {

                        MessageBox.Show("คุณกรอกข้อความยาวเกินที่กำหนดคะ");
                    }

                }
                else
                {
                    MessageBox.Show("กรุณากรอกข้อความด้วยคะ");
                }

                shareTextLabel.Focus();
            }
            else
            {
                shareTextLabel.Focus();
                //MessageBox.Show("cancelled!");
            }
        }

        public void MessageShowResultShare()
        {
            if (Status_Facebook)
            {
                MessageBox.Show(Result_Mylife + "\n" + Result_Facebook + "\n" + Result_Twitter);
                NavigationService.GoBack();
            }
        }

        public void postShareMylife(int content_id,String title = "",String description = "", String Share_text = "", String shareUrl = "", String shareImg = "", String share_service = "")
        {
            //Set Variable
            ContentID = content_id;
            Share_Description = description;
            ShareTitle = title;
            ShareText = Share_text;
            Share_Url = shareUrl;
            Share_Image = shareImg;

            System.Uri myUri = new System.Uri(URL_SHARE);
            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.BeginGetRequestStream(new AsyncCallback(GetRequestMyifeStreamCallback), myRequest);
        }

        public void GetRequestMyifeStreamCallback(IAsyncResult callbackResult)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
                // End the stream request operation
                Stream postStream = myRequest.EndGetRequestStream(callbackResult);
                
                Debug.WriteLine("You Debug Entered: " + ShareText);

                if (AccessToken != "" && ShareText != "" && ContentID != 0 && Share_Mylife != "")
                {

                        string postData = "access_token=" + AccessToken + "&apiKey=" + ApiKey + "&type=" + CommentType + "&ext_key=" + PREFIX_COMMENT + ContentID + "&message=" + ShareText +
                            "&ext_type=" + ExtType
                            + "&ext_field={\"url\": \"" + Share_Url + "\", \"thumbnail\": \"" + Share_Image + "\"}" + "&title=" + HttpUtility.UrlEncode(ShareTitle) + "&description=" + HttpUtility.UrlEncode(Share_Description) + "&service=" + Share_Mylife + "&client_ip=" + IP_Address;

                        Debug.WriteLine("Service Mylife : " + postData);

                        byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                        // Add the post data to the web request
                        postStream.Write(byteArray, 0, byteArray.Length);
                        postStream.Close();

                        // Start the web request
                        myRequest.BeginGetResponse(new AsyncCallback(GetResponsetMylifeStreamCallback), myRequest);
                }
            });
        }

        public void postShareFacebook(int content_id, String title = "", String description = "", String Share_text = "", String shareUrl = "", String shareImg = "")
        {
            //Set Variable
            ContentID = content_id;
            Share_Description = description;
            ShareTitle = title;
            ShareText = Share_text;
            Share_Url = shareUrl;
            Share_Image = shareImg;

            ShareLinkTask shareLinkTask = new ShareLinkTask();

            shareLinkTask.Title = description;
            shareLinkTask.LinkUri = new Uri(shareUrl, UriKind.Absolute);
            shareLinkTask.Message = description;
            shareLinkTask.Show();
        }

        public void GetRequestFacebookStreamCallback(IAsyncResult callbackResult)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
                // End the stream request operation
                Stream postStream = myRequest.EndGetRequestStream(callbackResult);

                Debug.WriteLine("You Debug Entered: " + ShareText);

                if (AccessToken != "" && ShareText != "" && ContentID != 0 && Share_Facebook != "")
                {

                    string postData = "access_token=" + AccessToken + "&apiKey=" + ApiKey + "&type=" + CommentType + "&ext_key=" + PREFIX_COMMENT + ContentID + "&message=" + ShareText +
                        "&ext_type=" + ExtType
                        + "&ext_field={\"url\": \"" + Share_Url + "\", \"thumbnail\": \"" + Share_Image + "\"}" + "&title=" + HttpUtility.UrlEncode(ShareTitle) + "&description=" + HttpUtility.UrlEncode(Share_Description) + "&service=" + Share_Facebook + "&client_ip=" + IP_Address;

                    Debug.WriteLine("Service Facebook : " + postData);

                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                    // Add the post data to the web request
                    postStream.Write(byteArray, 0, byteArray.Length);
                    postStream.Close();

                    // Start the web request
                    myRequest.BeginGetResponse(new AsyncCallback(GetResponsetFacebookStreamCallback), myRequest);
                }
            });
        }

        public void postShareTwitter(int content_id, String title = "", String description = "", String Share_text = "", String shareUrl = "", String shareImg = "")
        {
            //Set Variable
            ContentID = content_id;
            Share_Description = description;
            ShareTitle = title;
            ShareText = Share_text;
            Share_Url = shareUrl;
            Share_Image = shareImg;

            System.Uri myUri = new System.Uri(URL_SHARE);
            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.BeginGetRequestStream(new AsyncCallback(GetRequestTwitterStreamCallback), myRequest);
        }

        public void GetRequestTwitterStreamCallback(IAsyncResult callbackResult)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
                // End the stream request operation
                Stream postStream = myRequest.EndGetRequestStream(callbackResult);

                Debug.WriteLine("You Debug Entered: " + ShareText);

                if (AccessToken != "" && ShareText != "" && ContentID != 0 && Share_Twitter != "")
                {

                    string postData = "access_token=" + AccessToken + "&apiKey=" + ApiKey + "&type=" + CommentType + "&ext_key=" + PREFIX_COMMENT + ContentID + "&message=" + ShareText +
                        "&ext_type=" + ExtType
                        + "&ext_field={\"url\": \"" + Share_Url + "\", \"thumbnail\": \"" + Share_Image + "\"}" + "&title=" + HttpUtility.UrlEncode(ShareTitle) + "&description=" + HttpUtility.UrlEncode(Share_Description) + "&service=" + Share_Twitter + "&client_ip=" + IP_Address;

                    Debug.WriteLine("Service Facebook : " + postData);

                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                    // Add the post data to the web request
                    postStream.Write(byteArray, 0, byteArray.Length);
                    postStream.Close();

                    // Start the web request
                    myRequest.BeginGetResponse(new AsyncCallback(GetResponsetTwitterStreamCallback), myRequest);
                }
            });
        }

        //Get Response Share Content
        public void GetResponsetMylifeStreamCallback(IAsyncResult callbackResult)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {

                HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
                using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = httpWebStreamReader.ReadToEnd();
                    //For debug: show results
                    Debug.WriteLine("share Mylife result = " + result);

                    if (result != null)
                    {
                        var o = XDocument.Parse(result);
                        if (o.Root.Element("error") == null)
                        {
                            Result_Mylife = "Mylife : แชร์คอนเทนท์เสร็จแล้วคะ";
                            //Debug.WriteLine("GetResponsetMylifeStreamCallback : Success");
                        }
                        else if (o.Root.Element("error").Value.ToLower() == "too much share")
                        {
                            Result_Mylife = "Mylife : เคยแชร์คอนเทนท์นี้แล้วคะ";
                            Debug.WriteLine("GetResponsetMylifeStreamCallback :  too much share");

                        }
                        else
                        {
                            Result_Mylife = "Mylife : กรุณาลองใหม่อีกครั้งคะ";
                            Debug.WriteLine("GetResponsetMylifeStreamCallback : ไม่สามารถแชร์ได้ กรุณาลองใหม่อีกครั้งคะ");
                        }

                    }
                    else
                    {
                        Result_Mylife = "Mylife : กรุณาลองใหม่อีกครั้งคะ";
                        Debug.WriteLine("GetResponsetMylifeStreamCallback : ไม่สามารถแชร์ได้ กรุณาลองใหม่อีกครั้งคะ");
                        //MessageBox.Show("ไม่สามารถแชร์ได้ กรุณาลองใหม่อีกครั้งคะ");
                    }
                }

                //MessageBox.Show(Result_Mylife);
                //SetLoadingPanelVisibility(false);
                Status_Mylife = true;
                MessageShowResultShare();
            });
        }

        public void GetResponsetFacebookStreamCallback(IAsyncResult callbackResult)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {

                HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
                using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = httpWebStreamReader.ReadToEnd();
                    //For debug: show results
                    Debug.WriteLine("share Facebook result = " + result);

                    if (result != null)
                    {
                        var o = XDocument.Parse(result);
                        if (o.Root.Element("error") == null)
                        {
                            Result_Facebook = "Facebook : แชร์คอนเทนท์เสร็จแล้วคะ";
                            //Debug.WriteLine("GetResponsetFacebookStreamCallback : Success");
                        }
                        else if (o.Root.Element("error").Value.ToLower() == "too much share")
                        {
                            Result_Facebook = "Facebook : เคยแชร์คอนเทนท์นี้แล้วคะ";
                            //Debug.WriteLine("GetResponsetFacebookStreamCallback :  too much share");

                        }
                        else
                        {
                            Result_Facebook = "Facebook : กรุณาลองใหม่อีกครั้งคะ";
                            //Debug.WriteLine("GetResponsetFacebookStreamCallback : ไม่สามารถแชร์ได้ กรุณาลองใหม่อีกครั้งคะ");
                        }

                    }
                    else
                    {
                        Result_Facebook = "Facebook : กรุณาลองใหม่อีกครั้งคะ";
                        //Debug.WriteLine("GetResponsetFacebookStreamCallback : ไม่สามารถแชร์ได้ กรุณาลองใหม่อีกครั้งคะ");

                    }
                }

                Status_Facebook = true;
                MessageShowResultShare();
                //MessageBox.Show(Result_Facebook);
                //SetLoadingPanelVisibility(false);

            });
        }

        public void GetResponsetTwitterStreamCallback(IAsyncResult callbackResult)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {

                HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
                using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = httpWebStreamReader.ReadToEnd();
                    //For debug: show results
                    Debug.WriteLine("share Twitter result = " + result);

                    if (result != null)
                    {
                        var o = XDocument.Parse(result);
                        if (o.Root.Element("error").Value.ToLower() == "success")
                        {
                            Result_Twitter = "Facebook : แชร์คอนเทนท์เสร็จแล้วคะ";
                            Debug.WriteLine("GetResponsetTwitterStreamCallback :  Success");

                        }else if(o.Root.Element("error").Value.ToLower() == "too much share")
                        {
                            Result_Twitter = "Twitter : เคยแชร์คอนเทนท์นี้แล้วคะ";
                            Debug.WriteLine("GetResponsetTwitterStreamCallback :  too much share");

                        }else
                        {
                            Result_Twitter = "Twitter : กรุณาลองใหม่อีกครั้งคะ";
                            Debug.WriteLine("GetResponsetTwitterStreamCallback : ไม่สามารถแชร์ได้ กรุณาลองใหม่อีกครั้งคะ");
                        }

                    }
                    else
                    {
                        Result_Twitter = "Twitter : กรุณาลองใหม่อีกครั้งคะ";
                        Debug.WriteLine("GetResponsetTwitterStreamCallback : ไม่สามารถแชร์ได้ กรุณาลองใหม่อีกครั้งคะ");
                        //MessageBox.Show("ไม่สามารถแชร์ได้ กรุณาลองใหม่อีกครั้งคะ");
                    }
                }

                Status_Twitter = true;
                MessageShowResultShare();
                //MessageBox.Show(Result_Twitter);
                //SetLoadingPanelVisibility(false);

            });
        }

        //Check Permission Share social
        public void CheckPermissionSocial()
        {
            WebClient myService = new WebClient();
            String url = "";
            try
            {
                if (AccessToken != "" && SSOID != "")
                {

                    url = URL_CHECK_SOCIAL + "&access_token=" + AccessToken + "&user_id=" + SSOID + "&time=" + DateTime.Now;
                    Debug.WriteLine("URL URL_CHECK_SOCIAL = " + url);

                    ContentID = 0;

                    myService.DownloadStringAsync(new Uri(url));
                    myService.DownloadStringCompleted += new DownloadStringCompletedEventHandler(CheckPermissionSocial_DownloadStringCompleted);

                }
                else
                {
                    throw new Exception("AccessToken,SSOID is NULL !");
                }

            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void CheckPermissionSocial_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {

                if (e.Error != null)
                {
                    throw new Exception(e.Error.Message);
                }

                var o = XDocument.Parse(e.Result);

                //Check Facebook
                if (o.Root.Element("facebook") != null)
                {
                    fb.IsChecked = true;
                    Debug.WriteLine("Facebook = " + o.Root.Element("facebook").Element("username").Value.ToString());
                    
                }
                else
                {
                    FacebookLabel.Foreground = new SolidColorBrush(Colors.Gray);
                    fb.IsEnabled = false;
                    Debug.WriteLine("Permission Facebook Social : No have Permission !");
                }

                //Check Twitter
                //if (o.Root.Element("twitter") != null)
                //{
                //    tw.IsChecked = true;
                //    Debug.WriteLine("Twiiter = " + o.Root.Element("twitter").Element("username").Value.ToString());

                //}
                //else
                //{
                //    TwitterLabel.Foreground = new SolidColorBrush(Colors.Gray);
                //    tw.IsEnabled = false;
                //    Debug.WriteLine("Permission Twiiter Social : No have Permission !");
                //}

            }
            catch (Exception ex)
            {
                Debug.WriteLine("ShareContentPage.xaml.cs : CheckPermissionSocial_DownloadStringCompleted ; " + ex.Message);
            }

            //SetLoadingPanelVisibility(false);
        }

        //Check IP Address
        private void CheckIP()
        {
            WebClient ipService = new WebClient();
            String url = "";
            try
            {
                    url = URL_CHECK_IP + "&time=" + DateTime.Now;
                    Debug.WriteLine("URL_CHECK_IP = " + url);

                    ipService.DownloadStringAsync(new Uri(url));
                    ipService.DownloadStringCompleted += new DownloadStringCompletedEventHandler(CheckIP_DownloadStringCompleted);

            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void CheckIP_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {

                if (e.Error != null)
                {
                    throw new Exception(e.Error.Message);
                }

                var o = XDocument.Parse(e.Result);

                if (o.Root.Element("code") != null)
                {
                    if (Convert.ToInt16(o.Root.Element("code").Value) == 200)
                    {

                        IP_Address = o.Root.Element("remote_addr").Value;
                    
                    }

                    Debug.WriteLine("IP Address = " + IP_Address);

                }
                else
                {
                    Debug.WriteLine("Get IP Address Error !!!");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("ShareContentPage.xaml.cs : CheckIP_DownloadStringCompleted ; " + ex.Message);
            }

        }



        /*
        void toggle_Indeterminate(object sender, RoutedEventArgs e)
        {
            //add some content here
            this.toggle.Content = "toggle_Indeterminate";
        }

        void toggle_Click(object sender, RoutedEventArgs e)
        {
            //this.toggle.Content = "Click";
            //add some content here
        }
        */



        private void Button_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Share_Click(null, null);
        }

        private void Button_Tap_2(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.NavigationService.GoBack();
        }


    }
}