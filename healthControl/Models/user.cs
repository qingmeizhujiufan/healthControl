using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace healthControl.Models
{
    public class user
    {
        public string id { set; get; }
        public string open_id { set; get; }
        public string name { set; get; }
        public int age { set; get; }
        public string sex { set; get; }
        public string telephone { set; get; }
        public string period { set; get; }
        public float height { set; get; }
        public float weight { set; get; }
        public float waistline { set; get; }
        public string workingType { set; get; }
        public string create_time { set; get; }
    }
}