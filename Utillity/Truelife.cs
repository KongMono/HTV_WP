using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Xml.Linq;

namespace News
{
    public class Truelife
    {
        //Variable
        public static Truelife Instance = new Truelife();
        LoginItem LoginData;
        string username = "";
        string password = "";
        ProfileItem ProfileData;

        //Delegate
        public delegate void LoginCompletedEventHandler(TruelifeEventArgs e);
        //private event LoginCompletedEventHandler LoginWithEmailCompleted;
        public event LoginCompletedEventHandler LoginAllCompleted;

        public delegate void GetProfileCompletedEventHandler(TruelifeEventArgs e);
        public event GetProfileCompletedEventHandler GetProfileCompleted;

        //Constructor
        private Truelife()
        {
            LoadLoginData();
            ProfileData = new ProfileItem();
        }

        //GET & SET
        public LoginItem GetLoginData()
        {
            return LoginData;
        }

        //SAVE & LOAD
        private void SaveLoginData()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("LoginData"))
            {
                IsolatedStorageSettings.ApplicationSettings["LoginData"] = LoginData;
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings.Add("LoginData", LoginData);
            }

            //make sure data is saved immediatelly
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        private void LoadLoginData()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("LoginData"))
            {
                LoginData = (LoginItem)IsolatedStorageSettings.ApplicationSettings["LoginData"];
            }
            else
            {
                LoginData = new LoginItem();
            }
        }

        //Logout
        public void Logout()
        {
            LoginData = null;
            SaveLoginData();
        }

        //Login
        private void LoginWithEmailAsync(string email, string password)
        {
            this.username = email;
            this.password = password;

            string url = "http://new.truelife.com/api/profile/rest?method=loginTruelife";
            url += "&apiKey=" + HttpUtility.UrlEncode("9ae438a7c441a132c698c6389b95ad72");
            url += "&username=" + HttpUtility.UrlEncode(email);
            url += "&password=" + HttpUtility.UrlEncode(password);

            WebClient WebClient_LoginEmail = new WebClient();
            WebClient_LoginEmail.DownloadStringCompleted += new DownloadStringCompletedEventHandler(WebClient_LoginEmail_DownloadStringCompleted);
            WebClient_LoginEmail.DownloadStringAsync(new Uri(url, UriKind.Absolute));
        }

        public void LoginAllAsync(string username, string password)
        {
            this.username = username;
            this.password = password;

            string url = "http://new.truelife.com/api/profile/rest?method=loginAll";
            url += "&app_id=" + "1";
            url += "&secretKey=" + HttpUtility.UrlEncode("a42ca3848f9dd9995fedeb93ebf97805");
            url += "&apiKey=" + HttpUtility.UrlEncode("9ae438a7c441a132c698c6389b95ad72");
            url += "&username=" + HttpUtility.UrlEncode(username);
            url += "&password=" + HttpUtility.UrlEncode(password);
            url += "&language=" + "th";

            WebClient WebClient_LoginAll = new WebClient();
            WebClient_LoginAll.DownloadStringCompleted += new DownloadStringCompletedEventHandler(WebClient_LoginEmail_DownloadStringCompleted);
            WebClient_LoginAll.DownloadStringAsync(new Uri(url, UriKind.Absolute));
        }

        //check token & get profile
        public void GetProfileByAccessTokenAsyn()
        {
            string url = "http://new.truelife.com/api/profile/rest?method=getProfileByAccessToken";
            url += "&apiKey=" + HttpUtility.UrlEncode("9ae438a7c441a132c698c6389b95ad72");
            url += "&access_token=" + LoginData.sso_access_token;

            WebClient WebClient_GetProfile = new WebClient();
            WebClient_GetProfile.DownloadStringCompleted += new DownloadStringCompletedEventHandler(WebClient_GetProfile_DownloadStringCompleted);
            WebClient_GetProfile.DownloadStringAsync(new Uri(url, UriKind.Absolute));
        }

        //Event
        void WebClient_LoginEmail_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            TruelifeEventArgs eventArg = new TruelifeEventArgs();
            eventArg.Code = 0;
            eventArg.Description = "error";
            try
            {
                if (e.Error != null)
                {
                    throw new Exception(e.Error.Message);
                }
                //----------
                XDocument xdoc = XDocument.Parse(e.Result, LoadOptions.None);
                if (xdoc.Root == null)
                {
                    throw new Exception("Root is null");
                }

                if (xdoc.Root.Element("header") == null)
                {
                    throw new Exception("header is null");
                }

                if (xdoc.Root.Element("header").Element("code") == null)
                {
                    throw new Exception("header/code is null");
                }

                if (xdoc.Root.Element("header").Element("description") == null)
                {
                    throw new Exception("header/description is null");
                }
                //----------
                var code = xdoc.Root.Element("header").Element("code");
                eventArg.Code = XmlValueParser.ParseInteger(code);

                var desc = xdoc.Root.Element("header").Element("description");
                eventArg.Description = XmlValueParser.ParseString(desc);

                if (eventArg.Code != 200)
                {
                    eventArg.ErrorMessageForUser = eventArg.Description;
                    eventArg.ErrorMessageForDeveloper = eventArg.Description;
                        eventArg.IsError = true;
                }
                else
                {
                    if (xdoc.Root.Element("profile") == null)
                    {
                        throw new Exception("profile is null");
                    }
                    
                    //----------
                    LoginItem tmp_item = new LoginItem();
                    
                    //parse
                    //tmp_item.blog_id = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("blog_id"));
                    tmp_item.sso_access_token = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("sso_access_token"));
                    tmp_item.sso_expires = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("sso_expires"));
                    //tmp_item.username = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("username"));
                    //tmp_item.password = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("password"));
                    tmp_item.email = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("email"));
                    tmp_item.name = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("name"));
                    tmp_item.birthday = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("birthday"));
                    tmp_item.gender = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("gender"));
                    tmp_item.role_id = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("role_id"));
                    tmp_item.employee_id = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("employee_id"));
                    tmp_item.sso_uid = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("sso_uid"));
                    tmp_item.verify_status = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("verify_status"));
                    
                    //----------
                    LoginData = tmp_item;
                    
                    LoginData.username = username;
                    //LoginData.password = password;
                    LoginData.password = "";//not need to save password
                    
                    eventArg.IsError = false;
                    SaveLoginData();
                }
            }
            catch (Exception ex)
            {
                eventArg.ErrorMessageForUser = "login error , some error occur please try again later.";
                eventArg.ErrorMessageForDeveloper = "Error : Truelife - LoginWithEmail ; " + ex.Message;
                eventArg.IsError = true;
            }
            //-----
            //LoginWithEmailCompleted(eventArg);
            LoginAllCompleted(eventArg);
        }

        void WebClient_GetProfile_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            TruelifeEventArgs eventArg = new TruelifeEventArgs();
            eventArg.Code = 0;
            eventArg.Description = "error";
            try
            {
                if (e.Error != null)
                {
                    throw new Exception(e.Error.Message);
                }
                //----------
                XDocument xdoc = XDocument.Parse(e.Result, LoadOptions.None);
                if (xdoc.Root == null)
                {
                    throw new Exception("Root is null");
                }

                if (xdoc.Root.Element("header") == null)
                {
                    throw new Exception("header is null");
                }

                if (xdoc.Root.Element("header").Element("code") == null)
                {
                    throw new Exception("header/code is null");
                }

                if (xdoc.Root.Element("header").Element("description") == null)
                {
                    throw new Exception("header/description is null");
                }
                //----------
                var code = xdoc.Root.Element("header").Element("code");
                eventArg.Code = XmlValueParser.ParseInteger(code);

                var desc = xdoc.Root.Element("header").Element("description");
                eventArg.Description = XmlValueParser.ParseString(desc);

                if (eventArg.Code == 400)
                {
                    eventArg.ErrorMessageForUser = "กรุณาล็อกอินใหม่";
                    eventArg.ErrorMessageForDeveloper = "error code is " + eventArg.Code + " , " + desc;
                    eventArg.IsError = true;
                    ProfileData = null;
                }
                else if (eventArg.Code != 200)
                {
                    eventArg.ErrorMessageForUser = "error code is " + eventArg.Code + " , " + desc;
                    eventArg.ErrorMessageForDeveloper = "error code is " + eventArg.Code + " , " + desc;
                    eventArg.IsError = true;
                    ProfileData = null;
                }
                else
                {
                    if (xdoc.Root.Element("profile") == null)
                    {
                        throw new Exception("profile is null");
                    }

                    //parse
                    ProfileData.UserID = XmlValueParser.ParseInteger(xdoc.Root.Element("profile").Element("uid"));
                    ProfileData.Email = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("email"));
                    ProfileData.FirstName = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("first_name"));
                    ProfileData.LastName = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("last_name"));
                    ProfileData.Birthday = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("birthday"));
                    ProfileData.Gender = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("gender"));
                    ProfileData.DisplayName = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("display_name"));
                    ProfileData.EmailContact = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("email_contact"));
                    ProfileData.MobileContact = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("mobile_contact"));
                    ProfileData.Mobile = XmlValueParser.ParseString(xdoc.Root.Element("profile").Element("mobile"));

                    //----------
                    eventArg.IsError = false;
                }
            }
            catch (Exception ex)
            {
                eventArg.ErrorMessageForUser = "get profile error , some error occur please try again later.";
                eventArg.ErrorMessageForDeveloper = "Error : Truelife - GetProfile ; " + ex.Message;
                eventArg.IsError = true;
                ProfileData = null;
            }
            //-----
            GetProfileCompleted(eventArg);
        }
    }

    public class TruelifeEventArgs
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public string ErrorMessageForUser { get; set; }
        public string ErrorMessageForDeveloper { get; set; }
        public bool IsError { get; set; }
    }
}
