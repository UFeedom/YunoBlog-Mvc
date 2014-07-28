using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YunoBlog.DataBase;
using YunoBlog.Models;

namespace YunoBlog.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string s)
        {
            return RedirectToAction("Index", "Home", new { s = HttpUtility.UrlEncode(s) });
        }

        public ActionResult GetArticles(int page, int? category, string title, int? year, int? month)
        {
            var articles = (from a in DbContext.Contents
                            where a.TypeAsInt == 0
                            && a.Title.Contains(title)
                            select a);
            if (category != null) articles = articles.Where(a => a.CategoryID == category);
            if (year != null && month != null)
                articles = articles.Where(a => a.Time.Year == year && a.Time.Month == month);
            var _articles = articles.OrderByDescending(a => a.Time).Skip(10 * page).Take(10).ToList();
            var ret = new List<Article>();
            foreach (var a in _articles)
            {
                var tmp = new Article();
                tmp.Title = a.Title;
                tmp.ID = a.ID;
                tmp.Category = a.Category.Title;
                tmp.CategoryID = a.CategoryID.Value;
                tmp.Line = a.Body.Split('\n').Length;
                if (tmp.Line <= 20) tmp.Body = a.Body;
                else tmp.Body = a.Body.Substring(0, Helpers.StringHelper.IndexOf(a.Body, "\n", 20));
                tmp.Body = Helpers.MarkdownHelper.Markdown(tmp.Body);
                tmp.CreationTime = Helpers.StringHelper.ToCreationTime(a.Time);
                ret.Add(tmp);
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
    }
}