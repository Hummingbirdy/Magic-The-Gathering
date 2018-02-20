using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MTG.Utilities;

namespace MTG.Helpers
{
    public static class HTMLHelperExtensions
    {
        public static IHtmlString JsonFor<T>(this HtmlHelper helper, T obj)
        {
            return helper.Raw(obj.ToJson());
        }
    }
}