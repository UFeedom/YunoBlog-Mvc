using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YunoBlog.DataBase;

namespace YunoBlog.Controllers
{
    public class ArticleController : BaseController
    {
        //
        // GET: /Article/
        public ActionResult Index(int id)
        {
            using (DB db = new DB())
            {
                var model = db.Contents.Find(id);
                return View(model);
            }
        }
	}
}