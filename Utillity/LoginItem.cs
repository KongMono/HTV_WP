using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace News
{
    public class LoginItem
    {
        public string blog_id { get; set; }
        public string sso_access_token { get; set; }
        public string sso_expires { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string birthday { get; set; }
        public string gender { get; set; }
        public string role_id { get; set; }
        public string employee_id { get; set; }
        public string sso_uid { get; set; }
        public string verify_status { get; set; }
    }
}
