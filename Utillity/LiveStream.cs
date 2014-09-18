using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Xml.Linq;

namespace News
{
    public class LiveStream
    {
        //Variable
        StreamingItem streamingData;
        LoginItem loginData;

        //URL
        const string URL_GET_UNIX_TIME = "http://playlistservices.truelife.com/get_time.php?type=2";
        const string URL_STREAMING = "http://www.truelife.com/proxy_stream/rest/";

        //Delegate
        public delegate void GetLiveStreamUrlCompletedEventHandler(LiveStreamEventArgs e);
        public event GetLiveStreamUrlCompletedEventHandler GetLiveStreamUrlCompleted;

        //Constructor
        public LiveStream()
        {
            //
        }

        public void GetLiveStreamUrlAsyn(StreamingItem streaming)
        {
            streamingData = streaming;
            loginData = null;

            WebClient WebClient_GetUnixTime = new WebClient();
            WebClient_GetUnixTime.DownloadStringCompleted += new DownloadStringCompletedEventHandler(WebClient_GetUnixTime_DownloadStringCompleted);
            WebClient_GetUnixTime.DownloadStringAsync(new Uri(URL_GET_UNIX_TIME, UriKind.Absolute));
        }

        void WebClient_GetUnixTime_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    throw new Exception(e.Error.Message);
                }

                //----------

                string UnixTime = e.Result;
                string preToken = "";
                string keyToken = "abb0256a31";
                preToken += streamingData.Scope;
                preToken += "|" + streamingData.ContentType;
                preToken += "|" + UnixTime;
                preToken += "|" + keyToken;
                preToken += "|" + streamingData.ProjectID;
                preToken += "|" + streamingData.ContentID;

                string url = URL_STREAMING;
                url += "?scope=" + streamingData.Scope;
                url += "&content_type=" + streamingData.ContentType;
                url += "&content_id=" + streamingData.ContentID;
                url += "&project_id=" + streamingData.ProjectID;
                url += "&pl_token=" + LiveStreamSecurity.GenerateToken(preToken, keyToken);
                if (loginData != null)
                {
                    url += "&io=" + LiveStreamSecurity.GenerateToken(loginData.sso_uid);
                }
               
                url += "&ref=" + "HTV";
                url += "&product_id=" + streamingData.ProductID;
                url += "&channel=" + "app";
                url += "&lifestyle=" + streamingData.LifeStyle;
                //url += "&sso_id=" + login.sso_uid;
                //url += "&ip=" + "xxx";
                //url += "&domain=" + "xxx";
                url += "&device=" + "window_phone";//window_phone
                url += "&eplisode_id=" + "";
                url += "&sample_flag=" + "";
                url += "&network=" + "wifi";
                url += "&nosub=1";
                //url += "&mobile_no=" + "xxx";

                WebClient WebClient_GetLiveStreamURL = new WebClient();
                WebClient_GetLiveStreamURL.DownloadStringCompleted += new DownloadStringCompletedEventHandler(WebClient_GetLiveStreamURL_DownloadStringCompleted);
                WebClient_GetLiveStreamURL.DownloadStringAsync(new Uri(url, UriKind.Absolute));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LiveStream : WebClient_GetUnixTime_DownloadStringCompleted ; " + ex.Message);
            }
        }

        void WebClient_GetLiveStreamURL_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    throw new Exception(e.Error.Message);
                }

                //----------

                string url = e.Result;

                WebClient WebClient_GetRealLiveStreamURL = new WebClient();
                WebClient_GetRealLiveStreamURL.DownloadStringCompleted += WebClient_GetRealLiveStreamURL_DownloadStringCompleted;
                WebClient_GetRealLiveStreamURL.DownloadStringAsync(new Uri(url, UriKind.Absolute));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LiveStream : WebClient_GetLiveStreamURL_DownloadStringCompleted ; " + ex.Message);
            }
        }

        void WebClient_GetRealLiveStreamURL_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    throw new Exception(e.Error.Message);
                }

                //----------

                string url = e.Result;

                LiveStreamEventArgs event_arg = new LiveStreamEventArgs();
                event_arg.Result = url;
                GetLiveStreamUrlCompleted(event_arg);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LiveStream : WebClient_GetRealLiveStreamURL_DownloadStringCompleted ; " + ex.Message);
            }
        }

    }

    public class LiveStreamEventArgs
    {
        public string Result { get; set; }
    }
}
