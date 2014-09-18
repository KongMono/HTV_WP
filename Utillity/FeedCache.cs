using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Net;
using System.Diagnostics;
using System.Security.Cryptography;

namespace MovieTV
{
    public class FeedCache
    {
        //Variable
        public static FeedCache Instance = new FeedCache();
        private ObservableCollection<FeedCacheItem> FeedCacheList;

        //Constructor
        private FeedCache()
        {
            LoadFeedData();
        }

        //GET & SET
        public ObservableCollection<FeedCacheItem> GetFeedCacheData()
        {
            LoadFeedData();
            return FeedCacheList;
        }

        public void AddFeedData(FeedCacheItem Data)
        {
            //filter
            var filtered_list = from item in FeedCacheList
                                where item.url == Data.url
                                orderby item.expireDate descending
                                select item;

            //remove other same url
            foreach (var item in filtered_list)
            {
                if (item.expireDate != filtered_list.First().expireDate)
                {
                    RemoveFeedData(item);
                }
            }

            //add
            FeedCacheList.Add(Data);
            SaveFeedData();
        }

        public void RemoveFeedData(FeedCacheItem Data)
        {
            FeedCacheList.Remove(Data);
            SaveFeedData();
        }

        //SAVE & LOAD
        private void SaveFeedData()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("FeedCacheList"))
            {
                IsolatedStorageSettings.ApplicationSettings["FeedCacheList"] = FeedCacheList;
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings.Add("FeedCacheList", FeedCacheList);
            }

            //make sure data is saved immediatelly
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        private void LoadFeedData()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("FeedCacheList"))
            {
                FeedCacheList = (ObservableCollection<FeedCacheItem>)IsolatedStorageSettings.ApplicationSettings["FeedCacheList"];
            }
            else
            {
                FeedCacheList = new ObservableCollection<FeedCacheItem>();
            }
        }
    }

    public class FeedCacheRequestEventArgs
    {
        public FeedCacheItem Detail { get; set; }
        public Exception Error { get; set; }
        public XDocument XmlData { get; set; }
    }

    public class FeedCacheRequest
    {
        //Protect Key
        static byte[] XmlOptionalEntropy = { 2, 7, 0, 9, 0, 6, 0, 5, 3 };

        //Delegate
        public delegate void FeedCacheRequestCompletedEventHandler(FeedCacheRequestEventArgs e);
        public event FeedCacheRequestCompletedEventHandler FeedCacheRequestCompleted;

        //Constructor
        public FeedCacheRequest()
        {
            //
        }

        /// <summary>
        /// Request to specify absolute url like a WebClient with caching system.
        /// A return value indicates whether the caching system is requesting new data.
        /// </summary>
        public bool RequestFeedXml(string url, out XDocument XmlCacheData)
        {
            bool isRequestNewItem = false;
            XmlCacheData = null;
            //-----
            var filtered_list = from item in FeedCache.Instance.GetFeedCacheData()
                                where item.url == EncryptUrl(url)
                                orderby item.expireDate descending
                                select item;
            //-----
            if (filtered_list.Count() > 1)
            {
                //existing many just focus on lastest one
                XmlCacheData = ReadXmlFile(EncryptUrl(url));

                //remove other same url
                foreach (var item in filtered_list)
                {
                    if (item.expireDate != filtered_list.First().expireDate)
                    {
                        FeedCache.Instance.RemoveFeedData(item);
                    }
                }

                //request the new one if the old one expired
                if (DateTime.Compare(DateTime.Now, filtered_list.First().expireDate) >= 0)
                {
                    //expired
                    isRequestNewItem = true;
                    RequestNewFeedXml(url);
                }
                else
                {
                    //not expired yet
                    isRequestNewItem = false;
                }
            }
            else if (filtered_list.Count() == 1)
            {
                //existing only one
                XmlCacheData = ReadXmlFile(EncryptUrl(url));

                //request the new one if the old one expired
                if (DateTime.Compare(DateTime.Now, filtered_list.First().expireDate) >= 0)
                {
                    //expired
                    isRequestNewItem = true;
                    RequestNewFeedXml(url);
                }
                else
                {
                    //not expired yet
                    isRequestNewItem = false;
                }
            }
            else
            {
                //new url
                isRequestNewItem = true;
                RequestNewFeedXml(url);
            }
            //-----
            return isRequestNewItem;
        }

        private void RequestNewFeedXml(string url)
        {
            WebClient WebClient_RequestNewFeedXml = new WebClient();
            WebClient_RequestNewFeedXml.Headers["url"] = url;
            WebClient_RequestNewFeedXml.DownloadStringCompleted += WebClient_RequestNewFeedXml_DownloadStringCompleted;
            WebClient_RequestNewFeedXml.DownloadStringAsync(new Uri(url, UriKind.Absolute));
        }

        void WebClient_RequestNewFeedXml_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            FeedCacheRequestEventArgs eventArg = new FeedCacheRequestEventArgs();

            if (e.Error != null)
            {
                eventArg.Error = e.Error;
            }
            else
            {
                XDocument xdoc = XDocument.Parse(e.Result, LoadOptions.None);

                //save
                FeedCacheItem item = new FeedCacheItem()
                {
                    expireDate = DateTime.Now.Add(TimeSpan.FromSeconds(1)),
                    url = EncryptUrl((sender as WebClient).Headers["url"])
                };
                FeedCache.Instance.AddFeedData(item);
                WriteXmlFile(item.url, xdoc);

                //event_arg
                eventArg.Detail = item;
                eventArg.XmlData = xdoc;
            }

            FeedCacheRequestCompleted(eventArg);
        }

        private void WriteXmlFile(string fileName, XDocument xdoc)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                //init folder
                if (!store.DirectoryExists(@"cache"))
                {
                    store.CreateDirectory(@"cache");
                }

                if (!store.DirectoryExists(@"cache/xml"))
                {
                    store.CreateDirectory(@"cache/xml");
                }

                //delete existing file
                if (store.FileExists(@"cache/xml/" + fileName + ".xml"))
                {
                    store.DeleteFile(@"cache/xml/" + fileName + ".xml");
                }

                using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(@"cache/xml/" + fileName + ".xml", System.IO.FileMode.Create, store))
                {
                    //xdoc.Save(stream);

                    //convert xdoc to byte array
                    byte[] bytes = Encoding.UTF8.GetBytes(xdoc.ToString());

                    //encrypt byte array
                    byte[] encryptedBytes = ProtectedData.Protect(bytes, XmlOptionalEntropy);

                    //write encrypted byte array to file
                    stream.Write(encryptedBytes, 0, encryptedBytes.Length);
                    stream.Flush();
                }
            }
        }

        private XDocument ReadXmlFile(string fileName)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                //init folder
                if (!store.DirectoryExists(@"cache"))
                {
                    store.CreateDirectory(@"cache");
                }

                if (!store.DirectoryExists(@"cache/xml"))
                {
                    store.CreateDirectory(@"cache/xml");
                }

                //read existing file
                if (store.FileExists(@"cache/xml/" + fileName + ".xml"))
                {
                    using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(@"cache/xml/" + fileName + ".xml", System.IO.FileMode.Open, store))
                    {
                        //return XDocument.Load(stream);

                        //read file to byte array
                        byte[] bytesInStream = new byte[stream.Length];
                        stream.Read(bytesInStream, 0, bytesInStream.Length);

                        //decrypt
                        byte[] decryptedBytes = ProtectedData.Unprotect(bytesInStream, XmlOptionalEntropy);

                        //convert byte array to string
                        string xml_text = Encoding.UTF8.GetString(decryptedBytes, 0, decryptedBytes.Length);

                        return XDocument.Parse(xml_text, LoadOptions.None);
                    }
                }

                return null;
            }
        }

        private string EncryptUrl(string url)
        {
            string EncryptedString = "";

            //byte[] bytes = Encoding.UTF8.GetBytes(url);
            //EncryptedString = Convert.ToBase64String(bytes);

            EncryptedString = AES.EncryptString(url);

            return EncryptedString;
        }

        private string DecryptUrl(string encryptURL)
        {
            string DecryptedString = "";

            //byte[] bytes = Convert.FromBase64String(encryptURL);
            //DecryptedString = System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            DecryptedString = AES.DecryptString(encryptURL);

            return DecryptedString;
        }
    }
}
