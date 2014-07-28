using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YunoBlog.Models
{
    public class Calendar
    {
        public string Display 
        {
            get { return Year + "年" + Month + "月"; }
        }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Count { get; set; }
    }
}