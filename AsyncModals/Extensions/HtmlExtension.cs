using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace AsyncModals.Extensions
{
    public static class HtmlExtension
    {
        public static IHtmlContent AjaxModal(this IHtmlHelper html,
            string innerHtml,
            string url = null,
            string htmlTag = "button",
            object htmlAttributes = null,
            string method = "get",
            string updateId = null,
            object model = null,
            string dateFormat = null,
            ModalType modalType = ModalType.Normal)
        {
            if (url == null)
                url = html.ViewContext.HttpContext.Request.Path;
            if (model != null)
                method = "post";
            MyTagBuilder myTagBuilder = new MyTagBuilder(htmlTag);
            if (htmlAttributes != null)
                myTagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            myTagBuilder.MergeAttribute("data-toggle", "modal");
            myTagBuilder.MergeAttribute("data-target", $"{modalType}modal");
            myTagBuilder.MergeAttribute("data-ajax-method", method);
            myTagBuilder.MergeAttribute("data-url", url);
            myTagBuilder.MergeAttribute("onclick", "ajaxHelper.onclick(event, this)");
            if (updateId != null)
                myTagBuilder.MergeAttribute("data-update", updateId);
            if (model != null)
                myTagBuilder.MergeAttribute("data-model", JsonConvert.SerializeObject(model));
            myTagBuilder.InnerHtml.Append(innerHtml);
            return myTagBuilder.RenderBody();
        }
    }
    public enum ModalType
    {
        Normal,
        Tiny,
        Medium,
        Centered,
        Big,
        Biggest
    }
}
