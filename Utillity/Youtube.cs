﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using MyToolkit.Environment;
using MyToolkit.Multimedia;
using MyToolkit.Networking;

#if METRO
using System.Threading.Tasks;
#elif WINDOWS_PHONE
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using MyToolkit.Paging;
using MyToolkit.UI;
using System.Diagnostics;

#endif

// developed by Rico Suter (http://rsuter.com), http://mytoolkit.codeplex.com
// this code only works with windows phone mango (video urls with query don't work in previous versions)

namespace MovieTV
{
    public static class YouTube
    {

#if METRO
		public static Task<YouTubeUri> GetVideoUriAsync(string youTubeId, YouTubeQuality maxQuality = YouTubeQuality.Quality480P)
		{
			var task = new TaskCompletionSource<YouTubeUri>();
			GetVideoUri(youTubeId, maxQuality, (u, e) =>
			{
				if (u != null)
					task.SetResult(u);
				else if (e == null)
					task.SetCanceled();
				else
					task.SetException(e);
			});
			return task.Task;
		} 
#endif

        public static Uri GetThumbnailUri(string youTubeId, YouTubeThumbnailSize size = YouTubeThumbnailSize.Medium)
        {
            MessageBox.Show("GetThumbnailUri");
            switch (size)
            {
                case YouTubeThumbnailSize.Small:
                    return new Uri("http://img.youtube.com/vi/" + youTubeId + "/default.jpg", UriKind.Absolute);
                case YouTubeThumbnailSize.Medium:
                    return new Uri("http://img.youtube.com/vi/" + youTubeId + "/hqdefault.jpg", UriKind.Absolute);
                case YouTubeThumbnailSize.Large:
                    return new Uri("http://img.youtube.com/vi/" + youTubeId + "/maxresdefault.jpg", UriKind.Absolute);
            }
            throw new Exception();
        }

        private static int GetQualityIdentifier(YouTubeQuality quality)
        {
            switch (quality)
            {
                case YouTubeQuality.Quality480P: return 18;
                case YouTubeQuality.Quality720P: return 22;
                case YouTubeQuality.Quality1080P: return 37;
            }
            throw new ArgumentException("maxQuality");
        }

        public static HttpResponse GetVideoUri(string youTubeId, YouTubeQuality maxQuality, Action<YouTubeUri, Exception> completed)
        {
            Debug.WriteLine("GetVideoUri = " + "http://www.youtube.com/watch?v=" + youTubeId);
            return Http.Get("http://www.youtube.com/watch?v=" + youTubeId, r => OnHtmlDownloaded(r, maxQuality, completed));
        }

        private static void OnHtmlDownloaded(HttpResponse response, YouTubeQuality quality, Action<YouTubeUri, Exception> completed)
        {
            if (response.Successful)
            {
                var urls = new List<YouTubeUri>();
                try
                {
                    //Debug.WriteLine("match = " + response.Response.ToString());
                    //var match = Regex.Match(response.Response, "url_encoded_fmt_stream_map=(.*?)(&|\")");
                    //var data = Uri.UnescapeDataString(match.Groups[1].Value);

                    var match = Regex.Match(response.Response, @"url_encoded_fmt_stream_map\"":(.*)\""");

                    String a = match.Groups[1].Value;
                    var data = a.Replace("\\u0026", "&");

                    var arr = data.Split(',');
                    foreach (var d in arr)
                    {
                        var url = "";
                        var signature = "";
                        var tuple = new YouTubeUri();
                        foreach (var p in d.Split('&'))
                        {
                            var index = p.IndexOf('=');
                            if (index != -1 && index < p.Length)
                            {
                                try
                                {
                                    var key = p.Substring(0, index);
                                    var value = Uri.UnescapeDataString(p.Substring(index + 1));
                                    if (key == "url")
                                        url = value;
                                    else if (key == "itag")
                                        tuple.Itag = int.Parse(value);
                                    else if (key == "type" && value.Contains("video/mp4"))
                                        tuple.Type = value;
                                    else if (key == "sig")
                                        signature = value;
                                }
                                catch { }
                            }
                        }

                        tuple.url = url + "&signature=" + signature;
                        if (tuple.IsValid)
                            urls.Add(tuple);
                    }

                    var itag = GetQualityIdentifier(quality);
                    foreach (var u in urls.Where(u => u.Itag > itag).ToArray())
                        urls.Remove(u);
                }
                catch (Exception ex)
                {
                    if (completed != null)
                        completed(null, ex);
                    return;
                }

                Debug.WriteLine("entry = " + urls.OrderByDescending(u => u.Itag).FirstOrDefault().ToString());
                var entry = urls.OrderByDescending(u => u.Itag).FirstOrDefault();
                if (entry != null)
                {
                    if (completed != null)
                        completed(entry, null);
                }
                else if (completed != null)
                    completed(null, new Exception("no_video_urls_found"));
            }
            else if (completed != null)
                completed(null, response.Exception);
        }

        public class YouTubeUri
        {
            internal string url;

            public Uri Uri { get { return new Uri(url, UriKind.Absolute); } }
            public int Itag { get; set; }
            public string Type { get; set; }

            public bool IsValid
            {
                get { return url != null && Itag > 0 && Type != null; }
            }
        }

        #region Phone

#if WINDOWS_PHONE

        public static HttpResponse Play(string youTubeId, YouTubeQuality maxQuality = YouTubeQuality.Quality480P, Action<Exception> completed = null)
        {
            //Debug.WriteLine("GetVideoUri");
            return GetVideoUri(youTubeId, maxQuality, (entry, e) =>
            {
                if (e != null)
                {
                    if (completed != null)
                        completed(e);
                }
                else
                {
                    if (completed != null)
                        completed(null);

                    if (entry != null)
                    {
                        var launcher = new MediaPlayerLauncher
                        {
                            Controls = MediaPlaybackControls.All,
                            Media = entry.Uri
                        };
                        Debug.WriteLine("Media URL = " + entry.Uri);
                        launcher.Show();
                    }
                }
            });
        }


        private static HttpResponse httpResponse;
        private static PageDeactivator oldState;

        /// <summary>
        /// This method disables the current page and shows a progress indicator until the youtube movie url has been loaded and starts
        /// </summary>
        /// <param name="youTubeId"></param>
        /// <param name="manualActivatePage">if true add YouTube.CancelPlay() in OnNavigatedTo() of the page (better)</param>
        /// <param name="maxQuality"></param>
        /// <param name="completed"></param>
        public static void Play(string youTubeId, bool manualActivatePage, YouTubeQuality maxQuality = YouTubeQuality.Quality480P, Action<Exception> completed = null)
        {
            //Debug.WriteLine("Play(string youTubeId, bool manualActivatePage, YouTubeQuality maxQuality = YouTubeQuality.Quality480P, Action<Exception> completed = null)");
            lock (typeof(YouTube))
            {
                if (oldState != null)
                    return;

                if (SystemTray.ProgressIndicator == null)
                    SystemTray.ProgressIndicator = new ProgressIndicator();

                SystemTray.ProgressIndicator.IsVisible = true;
                SystemTray.ProgressIndicator.IsIndeterminate = true;

                var page = PhonePage.CurrentPage;
                oldState = PageDeactivator.Inactivate();
                httpResponse = Play(youTubeId, YouTubeQuality.Quality480P, ex => Deployment.Current.Dispatcher.BeginInvoke(
                    delegate
                    {
                        if (page == PhonePage.CurrentPage) // !user navigated away
                            CancelPlay(manualActivatePage);

                        if (completed != null)
                            completed(ex);
                    }));
            }
        }

        /// <summary>
        /// call this in OnBackKeyPress() of the page: 
        /// e.Cancel = YouTube.CancelPlay();
        /// or in OnNavigatedTo() and use manualActivatePage = true
        /// </summary>
        /// <returns></returns>
        public static bool CancelPlay()
        {
            return CancelPlay(false);
        }

        private static bool CancelPlay(bool manualActivate)
        {
            lock (typeof(YouTube))
            {
                if (oldState == null && httpResponse == null)
                    return false;

                if (httpResponse != null)
                {
                    httpResponse.Abort();
                    httpResponse = null;
                }

                if (!manualActivate && oldState != null)
                {
                    oldState.Revert();
                    SystemTray.ProgressIndicator.IsVisible = false;
                    oldState = null;
                }

                return true;
            }
        }

#endif
        #endregion
    }
}