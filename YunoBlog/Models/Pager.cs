using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YunoBlog.Models
{
    public class Pager
    {
        public int ID { get; set; }
        public string Display { get { return (ID + 1).ToString(); } }
    }
}