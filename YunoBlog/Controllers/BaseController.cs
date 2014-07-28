using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using YunoBlog.Models;
using YunoBlog.DataBase;

namespace YunoBlog.Controllers
{
    public abstract class BaseController : Controller
    {
        private DB _DbContext = new DB();
        public DB DbContext { get { return _DbContext; } }
        public BaseController()
        {
            ViewBag.Navs = (from p in DbContext.Contents
                            where p.TypeAsInt == 1
                            orderby p.ID ascending
                            select new NavBar
                            {
                                ID = p.ID,
                                Title = p.Title
                            }).ToList();
            ViewBag.Links = (from l in DbContext.Links
                             orderby l.Priority descending
                             select l).ToList();
            ViewBag.Categories = (from c in DbContext.Categories
                                  orderby c.Priority descending
                                  select c).ToList();
            ViewBag.SitePostfix = ConfigurationManager.AppSettings["SiteName"];
            ViewBag.Disqus = ConfigurationManager.AppSettings["Disqus"];
            var Calendars = new List<Calendar>();
            var tmp = from a in DbContext.Contents where a.TypeAsInt == 0 orderby a.Time descending select a;
            if (tmp.Count() > 0)
            {
                var EndYear = tmp.Take(1).Single().Time.Year;
                var EndMonth = tmp.Take(1).Single().Time.Month;
                tmp = tmp.OrderBy(a => a.Time);
                var BeginYear = tmp.Take(1).Single().Time.Year;
                var BeginMonth = tmp.Take(1).Single().Time.Month;
                for (DateTime i = new DateTime(EndYear, EndMonth, 1); i.Date >= new DateTime(BeginYear, BeginMonth, 1); i = i.AddMonths(-1))
                {
                    Calendars.Add(new Calendar 
                    { 
                        Year = i.Year,
                        Month = i.Month,
                        Count = (from a in DbContext.Contents
                                 let t = a.Time
                                 where a.TypeAsInt == 0
                                 && t.Year == i.Year
                                 && t.Month == i.Month
                                 select a.ID).Count()
                    });
                }
            }
            ViewBag.Calendars = Calendars;
        }
    }
}