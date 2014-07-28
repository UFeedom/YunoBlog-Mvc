using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YunoBlog.Models
{
    public abstract class HomeBase
    {
        public List<NavBar> NavBar { get; set; }
        public List<Link> Links { get; set; }
    }
    public class Home : HomeBase
    {
    }
    public class NavBar
    {
        public int ID { get; set; }
        public string Title { get; set; }
    }
}