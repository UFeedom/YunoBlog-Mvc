using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;
using YunoBlog.Models;

namespace YunoBlog.Controllers
{
    public class AdminController : BaseController
    {
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (username == ConfigurationManager.AppSettings["Username"] && password == ConfigurationManager.AppSettings["Password"])
            {
                FormsAuthentication.SetAuthCookie(username, true);
                return RedirectToAction("General", "Admin", null);
            }
            ViewBag.LoginStatus = "登录失败";
            return View();
        }

        public ActionResult Logout(string sid)
        {
            if (sid != Session["Sid"].ToString()) return View();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home", null);
        }

        [Authorize]
        public ActionResult General()
        {
            return View(new Config
            {
                SiteName = ConfigurationManager.AppSettings["SiteName"],
                Disqus = ConfigurationManager.AppSettings["Disqus"],
                Username = ConfigurationManager.AppSettings["Username"],
            });
        }

        [HttpPost]
        [Authorize]
        public ActionResult General(string SiteName, string Disqus, string Username, string Password)
        {
            ConfigurationManager.AppSettings["SiteName"] = SiteName;
            ConfigurationManager.AppSettings["Disqus"] = Disqus;
            ConfigurationManager.AppSettings["Username"] = Username;
            if (!string.IsNullOrEmpty(Password))
                ConfigurationManager.AppSettings["Password"] = Password;
            return View(new Config
            {
                SiteName = ConfigurationManager.AppSettings["SiteName"],
                Disqus = ConfigurationManager.AppSettings["Disqus"],
                Username = ConfigurationManager.AppSettings["Username"],
            });
        }

        [Authorize]
        public ActionResult Articles(int id = 0)
        {
            var query = (from a in DbContext.Contents
                         where a.TypeAsInt == (int)ContentType.Article
                         orderby a.Time descending
                         select a);
            var count = query.Count();
            var page_count = count / 20 + 1;
            var ret = query.Skip(20 * id).Take(20).ToList();
            var Pager = new List<Pager>();
            for (int i = 0; i < page_count; i++)
                Pager.Add(new Pager() { ID = i });
            ViewBag.ArticlesPager = Pager;
            return View(ret);
        }

        [Authorize]
        public ActionResult Pages(int id = 0)
        {
            var query = (from a in DbContext.Contents
                         where a.TypeAsInt == (int)ContentType.Page
                         orderby a.Time descending
                         select a);
            var count = query.Count();
            var page_count = count / 20 + 1;
            var ret = query.Skip(20 * id).Take(20).ToList();
            var Pager = new List<Pager>();
            for (int i = 0; i < page_count; i++)
                Pager.Add(new Pager() { ID = i });
            ViewBag.PagesPager = Pager;
            return View(ret);
        }

        [HttpGet]
        [Authorize]
        public ActionResult CreateContent(int type)
        {
            var content = new Content
            {
                Title = "Untitled content",
                CategoryID = (type == (int)ContentType.Article ? (int?)0 : null),
                TypeAsInt = type,
                Body = "Empty",
                Time = DateTime.Now,
            };
            DbContext.Contents.Add(content);
            DbContext.SaveChanges();
            return RedirectToAction("EditContent", "Admin", new { id = content.ID });
        }

        [Authorize]
        public ActionResult EditContent(int id)
        {
            var content = DbContext.Contents.Find(id);
            return View(content);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditContent(int ID, string Title, int? CategoryID, string Body, string Upload)
        {
            var ret = new Content();
            if (!string.IsNullOrEmpty(Upload))
            {
                var File = Request.Files["File"];
                ret = DbContext.Contents.Find(ID);
                ret.Title = Title;
                ret.CategoryID = CategoryID;
                ret.Body = Body;
                string[] ImageExtension = { ".gif", ".jpg", ".jpeg", ".png", ".bmp" };
                string[] MusicExtension = { ".mp3", ".wav", ".aiff", ".m4a", ".wma", ".amr", ".mid", ".midi" };
                var filename = Helpers.StringHelper.ToTimeStamp(DateTime.Now) + System.IO.Path.GetExtension(File.FileName);
                File.SaveAs(Server.MapPath(@"~\Upload\" + filename));

                if (ImageExtension.Contains(System.IO.Path.GetExtension(File.FileName).ToLower()))
                {
                    ret.Body += "\r\n![Picture](/Upload/" + filename + ")";
                }
                else if (MusicExtension.Contains(System.IO.Path.GetExtension(File.FileName).ToLower()))
                {
                    ret.Body += "\r\n![@Music](/Upload/" + filename + ")";
                }
                else
                {
                    ret.Body += "\r\n[" + File.FileName + "](/Upload/" + filename + ")";
                }
                return View(ret);
            }
            else
            {
                GC.Collect();
                ret = DbContext.Contents.Find(ID);
                ret.Title = Title;
                ret.CategoryID = CategoryID;
                ret.Body = Body;
                DbContext.SaveChanges();
                if (ret.Type == ContentType.Article)
                    return RedirectToAction("Articles", "Admin", null);
                else
                    return RedirectToAction("Pages", "Admin", null);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult DeleteContent(int id)
        {
            var content = DbContext.Contents.Find(id);
            var type = content.Type;
            DbContext.Contents.Remove(content);
            DbContext.SaveChanges();
            if (type == ContentType.Article)
                return RedirectToAction("Articles", "Admin", null);
            else
                return RedirectToAction("Pages", "Admin", null);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Markdown(string content)
        {
            return Content(Helpers.MarkdownHelper.Markdown(content));
        }

        [Authorize]
        public ActionResult Categories()
        {
            var ret = (from c in DbContext.Categories
                       orderby c.Priority descending
                       select c).ToList();
            return View(ret);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Categories(int ID, int Priority, string Title)
        {
            var category = DbContext.Categories.Find(ID);
            category.Priority = Priority;
            category.Title = Title;
            DbContext.SaveChanges();
            return Content("");
        }

        [HttpGet]
        [Authorize]
        public ActionResult DeleteCategory(int id)
        {
            var category = DbContext.Categories.Find(id);
            DbContext.Categories.Remove(category);
            DbContext.SaveChanges();
            return RedirectToAction("Categories", "Admin", null);
        }

        [HttpGet]
        [Authorize]
        public ActionResult CreateCategory()
        {
            DbContext.Categories.Add(new Category() { Title = "新分类", Priority = 0 });
            DbContext.SaveChanges();
            return RedirectToAction("Categories", "Admin", null);
        }
        [Authorize]
        public ActionResult Links()
        {
            var ret = (from c in DbContext.Links
                       orderby c.Priority descending
                       select c).ToList();
            return View(ret);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Links(int ID, int Priority, string Title, string URL)
        {
            var Link = DbContext.Links.Find(ID);
            Link.Priority = Priority;
            Link.Title = Title;
            Link.URL = URL;
            DbContext.SaveChanges();
            return Content("");
        }

        [HttpGet]
        [Authorize]
        public ActionResult DeleteLink(int id)
        {
            var Link = DbContext.Links.Find(id);
            DbContext.Links.Remove(Link);
            DbContext.SaveChanges();
            return RedirectToAction("Links", "Admin", null);
        }

        [HttpGet]
        [Authorize]
        public ActionResult CreateLink()
        {
            DbContext.Links.Add(new Link() { Title = "新链接", Priority = 0, URL = "http:///" });
            DbContext.SaveChanges();
            return RedirectToAction("Links", "Admin", null);
        }

        [HttpGet]
        [Authorize]
        public ActionResult ConvertPages()
        {
            var files = System.IO.Directory.GetFiles(System.Web.HttpContext.Current.Server.MapPath("~") + "\\Pages", "*.md");
            foreach (var file in files)
            {
                DbContext.Contents.Add(new Content 
                { 
                    Title = System.IO.Path.GetFileNameWithoutExtension(file),
                    CategoryID = null,
                    Time = System.IO.File.GetCreationTime(file),
                    Body = System.IO.File.ReadAllText(file).Replace("\r\n\r\n","\r\n"),
                    TypeAsInt = 1
                });
                DbContext.SaveChanges();
            }
            return Content("Finished");
        }
        [HttpGet]
        [Authorize]
        public ActionResult ConvertArticles()
        {
            var files = System.IO.Directory.GetFiles(System.Web.HttpContext.Current.Server.MapPath("~") + "\\Articles", "*.md");
            foreach (var file in files)
            {
                DbContext.Contents.Add(new Content
                {
                    Title = System.IO.Path.GetFileNameWithoutExtension(file),
                    CategoryID = 1,
                    Time = System.IO.File.GetCreationTime(file),
                    Body = System.IO.File.ReadAllText(file),
                    TypeAsInt = 0
                });
                DbContext.SaveChanges();
            }
            return Content("Finished");
        }
    }
}