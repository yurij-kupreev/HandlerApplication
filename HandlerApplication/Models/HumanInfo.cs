using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HandlerApplication.Models
{
    public class HumanInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
    }

    public class PageData
    {
        public List<HumanInfo> humansInfo { get; set; }
        public int numOfPage { get; set; }
        public int numOfPages { get; set; }
        public List<String> comments { get; set; }

    }

}