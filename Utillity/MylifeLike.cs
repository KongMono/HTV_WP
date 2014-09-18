using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Linq;

namespace News
{
    public class MylifeLike
    {
        //Set Variable
        public static MylifeLike Instance = new MylifeLike();
        private PhoneApplicationPage Page;
        public int ContentID = 0;
        public String Title = "";
        public String Share_Url = "";
        public String Share_Image = "";
        public String AccessToken = "";
        public String SSOID = "";
        public String Username = "";
        public String UserImage = "";
        public bool Status_Like;


        String ApiKey = "b00bcd679d7b7bb6a3262db12c70ec0f";
        String CommentType = "ext_content";
        String ExtType = "article";

        //Constant
        const string URL_CHECK_LIKE = "http://api.mylife.truelife.com/content/content.xml?";
        const string URL_ACTIVE_LIKE = "http://api.mylife.truelife.com/like/like.xml";
        const string PREFIX_CONTENT = "cms_";

        public MylifeLike()
        {

            if (Truelife.Instance.GetLoginData().sso_access_token != null && Truelife.Instance.GetLoginData().sso_uid != null)
            {
                AccessToken = Truelife.Instance.GetLoginData().sso_access_token;
                SSOID = Truelife.Instance.GetLoginData().sso_uid;
                Username = Truelife.Instance.GetLoginData().name;
                UserImage = "http://image.platform.truelife.com/" + SSOID + "/avatar?key=1";
            }

        }

        public void GetCheckLike(int content_id, PhoneApplicationPage page)
        {
            //Set Variable
            ContentID = content_id;
            Page = page;

            WebClient myService = new WebClient();
            String url = "";
            try
            {
                if (AccessToken != "" && ApiKey != "" && ContentID != 0 && Page != null)
                {

                    url = URL_CHECK_LIKE + "apiKey=" + ApiKey + "&access_token=" + AccessToken + "&type=" + CommentType + "&ext_key=" + PREFIX_CONTENT + ContentID + "&time=" + DateTime.Now;
                    Debug.WriteLine("URL_CHECK_LIKE = " + url);
                    Debug.WriteLine("Before Status Like" + Status_Like);
                    
                    //myService.DownloadStringCompleted += new DownloadStringCompletedEventHandler(GetCheckLike_DownloadStringCompleted);
                    myService.DownloadStringCompleted += (sender, e) =>
                    {
                        try
                        {

                            if (e.Error != null)
                            {
                                throw new Exception(e.Error.Message);
                            }
                            var o = XDocument.Parse(e.Result);

                            if (o.Root.Element("is_like") != null)
                            {
                                //Debug.WriteLine("CheckLike = " + o.Root.Element("is_like").Value.ToString());
                                Debug.WriteLine("After Status Like = " + o.Root.Element("is_like").Value.ToString());

                                bool.TryParse(o.Root.Element("is_like").Value, out Status_Like);
                                Debug.WriteLine("CheckLike API = " + Status_Like);
                                if (Status_Like)
                                {
                                    //Status_Like = true;
                                    ((ApplicationBarIconButton)Page.ApplicationBar.Buttons[0]).IconUri = new Uri("/Assets/AppBar/icn_like.png", UriKind.Relative);
                                    ((ApplicationBarIconButton)Page.ApplicationBar.Buttons[0]).Text = "like";
                                }
                                else
                                {

                                    //Status_Like = false;
                                    ((ApplicationBarIconButton)Page.ApplicationBar.Buttons[0]).IconUri = new Uri("/Assets/AppBar/icn_like_no.png", UriKind.Relative);
                                    ((ApplicationBarIconButton)Page.ApplicationBar.Buttons[0]).Text = "unlike";
                                }

                                

                            }
                            else
                            {
                                throw new Exception("GetCheckLike_DownloadStringCompleted Fail!");
                            }

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("InCinemaDetailPage.xaml.cs : GetCommentWebClient_DownloadStringCompleted ; " + ex.Message);
                        }
                    };

                    myService.DownloadStringAsync(new Uri(url));

                }
                else
                {
                    throw new Exception("URL_CHECK_LIKE : Content id ,AccessToken,ApiKey is NULL !");
                }

            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }

            Debug.WriteLine("GetCheckLike Return = " + Status_Like);
            //return Status_Like;

        }

        private void GetCheckLike_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {

                if (e.Error != null)
                {
                    throw new Exception(e.Error.Message);
                }
                var o = XDocument.Parse(e.Result);

                if (o.Root.Element("is_like") != null)
                {
                    //Debug.WriteLine("CheckLike = " + o.Root.Element("is_like").Value.ToString());

                    if (XmlValueParser.ParseBool(o.Root.Element("is_like")) == true)
                    {
                        Status_Like = true;
                        ((ApplicationBarIconButton)Page.ApplicationBar.Buttons[0]).IconUri = new Uri("/Assets/AppBar/icn_like.png", UriKind.Relative);

                    }
                    else
                    {

                        Status_Like = false;
                        ((ApplicationBarIconButton)Page.ApplicationBar.Buttons[0]).IconUri = new Uri("/Assets/AppBar/icn_like_no.png", UriKind.Relative);
                    }

                    Debug.WriteLine("CheckLike = " + Status_Like);

                }
                else
                {
                    throw new Exception("GetCheckLike_DownloadStringCompleted Fail!");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("MylifeLike.cs : GetCheckLike_DownloadStringCompleted ; " + ex.Message);
            }

            //SetLoadingPanelVisibility(false);
        }

        //Active Like
        public void ActiveLikeContent(int content_id = 0, String title = "", String share_url = "", String share_img = "", PhoneApplicationPage page = null)
        {
            //Set Variable
            ContentID = content_id;
            Title = title;
            Share_Url = share_url;
            Share_Image = share_img;
            Page = page;
            if (ContentID != 0 && Page != null)
            {
                //ProgressBar
                ProgressIndicator indicator = new ProgressIndicator();
                indicator.IsVisible = true;
                indicator.IsIndeterminate = true;
                indicator.Text = "Loading...";
                SystemTray.SetProgressIndicator(Page, indicator);

                //AppBar
                //ApplicationBar.Mode = ApplicationBarMode.Minimized;
                Page.ApplicationBar.IsVisible = false;

                Debug.WriteLine("ActiveLikeContent : Doing");

                System.Uri myUri = new System.Uri(URL_ACTIVE_LIKE);
                HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);

                if (Status_Like == false)
                {
                    myRequest.Method = "POST";
                }
                else
                {
                    myRequest.Method = "DELETE";
                }

                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.BeginGetRequestStream(new AsyncCallback(GetRequestActiveLikeContentCallback), myRequest);
            }
        }

        private void GetRequestActiveLikeContentCallback(IAsyncResult callbackResult)
        {
            //Deployment.Current.Dispatcher.BeginInvoke(() =>
            //{
                HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
                // End the stream request operation
                Stream postStream = myRequest.EndGetRequestStream(callbackResult);

                // Create the post data

                if (AccessToken != "" && ContentID != 0)
                {
                    string postData = "";
                    if (Status_Like == false)
                    {
                        postData = "access_token=" + AccessToken + "&apiKey=" + ApiKey + "&type=" + CommentType + "&ext_key=" + PREFIX_CONTENT + ContentID +
                            "&ext_type=" + ExtType
                            + "&ext_field={\"url\": \"" + Share_Url + "\", \"thumbnail\": \"" + Share_Image + "\"}" + "&title=" + HttpUtility.UrlEncode(Title) + "&description=" + ContentID;

                        Debug.WriteLine("Set Active Like");
                    }
                    else
                    {
                        postData = "access_token=" + AccessToken + "&apiKey=" + ApiKey + "&type=" + CommentType + "&ext_key=" + PREFIX_CONTENT + ContentID;

                        Debug.WriteLine("Delete Like!");
                    }

                    Debug.WriteLine("postData_ActiveLike: " + postData);

                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                    // Add the post data to the web request
                    postStream.Write(byteArray, 0, byteArray.Length);
                    postStream.Close();

                    // Start the web request
                    myRequest.BeginGetResponse(new AsyncCallback(GetResponsetActiveLikeContentCallback), myRequest);

                    //SetLoadingPanelVisibility(true);
                }
            //});
        }

        private void GetResponsetActiveLikeContentCallback(IAsyncResult callbackResult)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {

                HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
                using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = httpWebStreamReader.ReadToEnd();
                    //For debug: show results
                    Debug.WriteLine(result);

                    if (result != null)
                    {
                        if (Status_Like == false)
                        {
                            Status_Like = true;
                            ((ApplicationBarIconButton)Page.ApplicationBar.Buttons[0]).IconUri = new Uri("/Assets/AppBar/icn_like.png", UriKind.Relative);
                            ((ApplicationBarIconButton)Page.ApplicationBar.Buttons[0]).Text = "like";
                            Page.ApplicationBar.IsVisible = true;
                            Page.ApplicationBar.Mode = ApplicationBarMode.Default;
                            SystemTray.SetProgressIndicator(Page, null);

                        }
                        else
                        {

                            Status_Like = false;
                            ((ApplicationBarIconButton)Page.ApplicationBar.Buttons[0]).IconUri = new Uri("/Assets/AppBar/icn_like_no.png", UriKind.Relative);
                            ((ApplicationBarIconButton)Page.ApplicationBar.Buttons[0]).Text = "unlike";
                            Page.ApplicationBar.IsVisible = true;
                            Page.ApplicationBar.Mode = ApplicationBarMode.Default;
                            SystemTray.SetProgressIndicator(Page, null);

                        }
                    }
                }

                //SetLoadingPanelVisibility(false);

            });
        }
    }
}
