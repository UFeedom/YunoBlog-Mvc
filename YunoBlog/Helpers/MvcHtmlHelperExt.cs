using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YunoBlog.Helpers
{
    public static class MvcHtmlHelperExt
    {
        public static MvcHtmlString ToCreationTime<TModel>(this HtmlHelper<TModel> self, DateTime time)
        {
            return new MvcHtmlString(StringHelper.ToCreationTime(time));
        }
        public static MvcHtmlString Markdown<TModel>(this HtmlHelper<TModel> self, string mdtxt)
        {
            return new MvcHtmlString(MarkdownHelper.Markdown(mdtxt));
        }
    }
}