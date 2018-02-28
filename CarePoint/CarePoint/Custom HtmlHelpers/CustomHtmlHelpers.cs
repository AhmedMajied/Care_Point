using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CarePoint.Custom_HtmlHelpers
{
    public static class CustomHtmlHelpers
    {
        public static MvcHtmlString Image(this HtmlHelper htmlHelper,byte[] imgBytes)
        {
            if (imgBytes != null)
            {
                var base64 = Convert.ToBase64String(imgBytes);
                var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                return BuildImageTag(imgSrc, null);
            }
            else
            {
                return BuildImageTag("../../img/notfound.png", null);
            }
        } 
        public static MvcHtmlString Image(this HtmlHelper htmlHelper,byte[] imgBytes,object htmlAttributes)
        {
            if (imgBytes != null)
            {
                var base64 = Convert.ToBase64String(imgBytes);
                var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                return BuildImageTag(imgSrc, htmlAttributes);
            }
            else
            {
                return BuildImageTag("../../img/notfound.png", htmlAttributes);
            }
        }
 
        private static MvcHtmlString BuildImageTag(string imgUrl, object htmlAttributes)
        {
            TagBuilder tag = new TagBuilder("img");
 
            tag.Attributes.Add("src", imgUrl);
            if (htmlAttributes != null)
                tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
 
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }
    }
}