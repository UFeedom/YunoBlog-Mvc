using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YunoBlog.DataBase;

namespace YunoBlog.Controllers
{
    public class PageController : BaseController
    {
        //
        // GET: /Page/
        public ActionResult Show(int id)
        {
            var model = DbContext.Contents.Find(id);
            return View(model);
        }
    }
}