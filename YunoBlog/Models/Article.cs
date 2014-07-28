using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YunoBlog.Models
{
    public class Article
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public int Line { get; set; }
        public string CreationTime { get; set; }
    }
}