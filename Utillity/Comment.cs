using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace News
{
    class Comment
    {
        private ObservableCollection<CommentItem> commentList = new ObservableCollection<CommentItem>();

        public static Comment Instance = new Comment();
        public int ContentID = 0;
        public String Title = "";
        public TextBox TextBoxName;
        public String CommentText = "";
        public TextBlock CommentCount;
        public int CommentAmount;
        public String Share_Url = "";
        public String Share_Image = "";
        public String AccessToken = "";
        public String SSOID = "";
        public String Username = "";
        public String UserImage = "";


        String ApiKey = "b00bcd679d7b7bb6a3262db12c70ec0f";
        String CommentType = "ext_content";
        String ExtType = "article";
        int Start = 0;
        int Limit = 10;

        //URL
        const string URL_GET_COMMENT = "http://api.mylife.truelife.com/comment/comment.xml?";
        const string URL_ADD_COMMENT = "http://api.mylife.truelife.com/comment/comment.xml";
        const string PREFIX_COMMENT = "cms_";

        public Comment()
        {
            
            if (Truelife.Instance.GetLoginData().sso_access_token != null && Truelife.Instance.GetLoginData().sso_uid != null)
            {
                AccessToken = Truelife.Instance.GetLoginData().sso_access_token;
                SSOID = Truelife.Instance.GetLoginData().sso_uid;
                Username = Truelife.Instance.GetLoginData().name;
                UserImage = "http://image.platform.truelife.com/" + SSOID + "/avatar?key=1";
            }
            
        }

        public void GetComment(int content_id = 0, ListBox ListBox = null,TextBlock CommentCountLabel = null)
        {
            //Set Variable
            CommentAmount = 0;
            ContentID = content_id;
            ListBox.ItemsSource = commentList;
            CommentCount = CommentCountLabel;

            WebClient myService = new WebClient();
            String url = "";
            try
            {
                if (AccessToken != "" && ApiKey != "" && ContentID != 0 && CommentCount != null)
                {

                    url = URL_GET_COMMENT + "apiKey=" + ApiKey + "&access_token=" + AccessToken + "&type=" + CommentType + "&ext_key=" + PREFIX_COMMENT + ContentID + "&start=" + Start + "&limit=" + Limit + "&time=" + DateTime.Now;
                    Debug.WriteLine("URL GetComment() = " + url);

                    ContentID = 0;

                    myService.DownloadStringAsync(new Uri(url));
                    myService.DownloadStringCompleted += new DownloadStringCompletedEventHandler(GetCommentClient_DownloadStringCompleted);

                }
                else
                {
                    throw new Exception("Content id ,AccessToken,ApiKey is NULL !");
                }

            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetCommentClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {

                if (e.Error != null)
                {
                    throw new Exception(e.Error.Message);
                }

                var o = XDocument.Parse(e.Result);

                
                if (o.Root.Element("data") != null)
                {
                    commentList.Clear();

                    Debug.WriteLine("Comment = " + o.Root.Element("data").Value.ToString());

                    foreach (var v in o.Root.Element("data").Elements("items"))
                    {
                        CommentAmount++;
                        CommentItem item = new CommentItem();

                        item.Username = v.Element("display_name").Value;
                        item.ImagePath = v.Element("avatar").Value;
                        item.Comment = v.Element("message").Value;

                        CommentCount.Text = "จำนวนคอมเม้นท์ " + CommentAmount + " คอมเม้นท์";
                        commentList.Add(item);

                        //postData();
                    }

                    Debug.WriteLine("Comment List Count : " + commentList.Count());
                }
                else
                {
                    Debug.WriteLine("GetComment : No Data Comment !");
                    CommentCount.Text = "จำนวนคอมเม้นท์ 0 คอมเม้นท์";
                    commentList.Clear();
                    //this.commentList.Clear();
                    //commentListBox.ItemsSource = null;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("InCinemaDetailPage.xaml.cs : GetCommentWebClient_DownloadStringCompleted ; " + ex.Message);
            }

            //SetLoadingPanelVisibility(false);
        }

        public void postData(int content_id = 0, ListBox ListBox = null, String title = "", TextBox textBoxComment = null, String Comment_text = "", String shareUrl = "", String shareImg = "")
        {
            //Set Variable
            ContentID = content_id;
            ListBox.ItemsSource = commentList;
            Title = title;
            TextBoxName = textBoxComment;
            CommentText = Comment_text;
            Share_Url = shareUrl;
            Share_Image = shareImg;

            Debug.WriteLine("postData Comment List Count : " + commentList.Count());

            //MessageBox.Show("You Entered: " + AddComment.Text);
            System.Uri myUri = new System.Uri(URL_ADD_COMMENT);
            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), myRequest);
        }

        public void GetRequestStreamCallback(IAsyncResult callbackResult)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
                // End the stream request operation
                Stream postStream = myRequest.EndGetRequestStream(callbackResult);


                Debug.WriteLine("You Debug Entered: " + CommentText);

                // Create the post data

                if (AccessToken != "" && this.ContentID != 0)
                {
                    if (CommentText.Trim() != "")
                    {
                        string postData = "access_token=" + AccessToken + "&apiKey=" + ApiKey + "&type=" + CommentType + "&ext_key=" + PREFIX_COMMENT + this.ContentID + "&message=" + CommentText +
                            "&ext_type=" + ExtType
                            + "&ext_field={\"url\": \"" + Share_Url + "\", \"thumbnail\": \"" + Share_Image + "\"}" + "&title=" + HttpUtility.UrlEncode(Title) + "&description=" + this.ContentID;
                        //string postData = "access_token=" + AccessToken + "&apiKey=" + ApiKey + "&type=" + CommentType + "&ext_key=" + this.content_id + "&message=" + AddComment.Text + "&ext_type=" + ExtType + "&ext_field=" + ExtField + "&title=title&description=description";
                        Debug.WriteLine("postData: " + postData);

                        byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                        // Add the post data to the web request
                        postStream.Write(byteArray, 0, byteArray.Length);
                        postStream.Close();

                        // Start the web request
                        myRequest.BeginGetResponse(new AsyncCallback(GetResponsetStreamCallback), myRequest);

                        //SetLoadingPanelVisibility(true);
                    }
                    else
                    {
                        MessageBox.Show("คุณกรอกข้อความไม่ถูกต้องคะ");
                        //Clear TextBox Comment
                        TextBoxName.Text = "";
                        TextBoxName.Focus();
                    }
                }
            });
        }

        public void GetResponsetStreamCallback(IAsyncResult callbackResult)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {

                HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
                using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = httpWebStreamReader.ReadToEnd();
                    //For debug: show results
                    Debug.WriteLine("Add comment result" + result);

                    if (result != null)
                    {

                        CommentItem item = new CommentItem();

                        item.Username = Username;
                        item.ImagePath = UserImage;
                        item.Comment = CommentText;

                        //Clear TextBox Comment
                        TextBoxName.Text = "";

                        MessageBox.Show("ขอบคุณสำหรับข้อความคะ");
                        CommentAmount += 1;
                        CommentCount.Text = "จำนวนคอมเม้นท์ " + (CommentAmount) + " คอมเม้นท์";
                        this.commentList.Insert(0, item);

                    }
                }

                //SetLoadingPanelVisibility(false);

            });
        }
    }//End Class
}
