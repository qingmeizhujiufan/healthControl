using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace healthControl.Models
{
    public class food
    {
        public string id { set; get; }
        public string type { set; get; }
        public string sub_type { set; get; }
        public string title { set; get; }
        public double energy_kilocal { set; get; }
        public double ca_mg { set; get; }
        public double protein_g { set; get; }
        public double riboflavin_mg { set; get; }
        public double mg_mg { set; get; }
        public double fat_g { set; get; }
        public double niacin_mg { set; get; }
        public double fe_mg { set; get; }
        public double vc_mg { set; get; }
        public double mn_mg { set; get; }
        public double ve_mg { set; get; }
        public double zn_mg { set; get; }
        public double cu_mg { set; get; }
        public double k_mg { set; get; }
        public double p_mg { set; get; }
        public double re_ug { set; get; }
        public double na_mg { set; get; }
        public double se_ug { set; get; }
    }
}