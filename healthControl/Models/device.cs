using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace healthControl.Models
{
    public class device
    {
        public string id { set; get; }
        public string device_id { set; get; }
        public string device_type { set; get; }
        public string msg_id { set; get; }
        public string msg_type { set; get; }
        public string open_id { set; get; }
        public string session_id { set; get; }
        public string bat { set; get; }
        public string hrs { set; get; }
        public string step { set; get; }
        public string lslt { set; get; }
        public string dslt { set; get; }
        public string btmp { set; get; }
        public string hbld { set; get; }
        public string lbld { set; get; }
        public string oxyg { set; get; }
        public string atmp { set; get; }
        public string create_time { set; get; }
    }
}