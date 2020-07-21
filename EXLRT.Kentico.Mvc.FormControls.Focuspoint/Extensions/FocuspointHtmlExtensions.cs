using CMS.Helpers;
using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace EXLRT.Kentico.Mvc.FormControls.Focuspoint.Extensions
{
    public static class FocuspointHtmlExtensions
    {
        public static MvcHtmlString RenderFocuspointStyles(this HtmlHelper htmlHelper, string path = "~/Content/FormControls/Focuspoint/css/focuspoint.css")
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            return MvcHtmlString.Create($"<link href='{urlHelper.Content(path)}' rel='stylesheet' type='text/css' />");
        }

        public static MvcHtmlString RenderFocuspointSripts(this HtmlHelper htmlHelper, string path = "~/Content/FormControls/Focuspoint/js/jquery.focuspoint.js",
            int throttleDuration = 17, bool reCalcOnWindowResize = true)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            StringBuilder script = new StringBuilder();
            script.Append($"<script src='{urlHelper.Content(path)}'></script>");
            script.Append($@"<script type='text/javascript'>
                                (function ($) {{
                                    $(document).ready(function () {{
                                        $('.js-focuspoint').focusPoint({{
                                            throttleDuration: {throttleDuration},
                                            reCalcOnWindowResize: {reCalcOnWindowResize.ToString().ToLowerInvariant()}
                                        }});
                                    }});
                                }}(jQuery));
                            </script>");

            return MvcHtmlString.Create(script.ToString());
        }

        public static MvcHtmlString FocuspointImage(this HtmlHelper htmlHelper, string imageUrl,
            string cssContainerClass = "",
            string imageAlt = "",
            string imageTitle = "",
            string cssImageClass = "",
            object htmlImageAttributes = null)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return MvcHtmlString.Empty;
            }

            string focusX = string.Empty;
            string focusY = string.Empty;
            string[] separateURL = imageUrl.Split('?');

            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            TagBuilder imageElement = new TagBuilder("img");

            if (separateURL.Length == 2)
            {
                NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(separateURL[1]);

                focusX = ValidationHelper.GetString(queryString.Get("focus-x"), string.Empty);
                focusY = ValidationHelper.GetString(queryString.Get("focus-y"), string.Empty);
                queryString.Remove("focus-x");
                queryString.Remove("focus-y");

                string cleanUrl = separateURL[0];
                if (queryString.Count > 0)
                {
                    cleanUrl = $"{cleanUrl}?{queryString.ToString()}";
                }

                imageElement.SafeMergeAttribute("src", urlHelper.Kentico().ImageUrl(cleanUrl, SizeConstraint.Empty));
                imageElement.SafeMergeAttribute("alt", imageAlt);
                imageElement.SafeMergeAttribute("title", imageTitle);

                if (!String.IsNullOrEmpty(cssImageClass))
                {
                    imageElement.AddCssClass(cssImageClass);
                }

                if (htmlImageAttributes != null)
                {
                    Dictionary<string, object> additionalAttributes = new Dictionary<string, object>();
                    foreach (PropertyInfo attr in htmlImageAttributes.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    {
                        additionalAttributes.AddSafe(attr.Name.Replace("_", "-"), attr.GetValue(htmlImageAttributes, null));
                    }

                    if (additionalAttributes.Any())
                    {
                        imageElement.MergeAttributes(additionalAttributes);
                    }
                }
            }
            else
            {
                imageElement.SafeMergeAttribute("src", urlHelper.Kentico().ImageUrl(imageUrl, SizeConstraint.Empty));
            }

            TagBuilder containerElement = new TagBuilder("div");
            containerElement.AddCssClass("js-focuspoint");
            containerElement.AddCssClass(cssContainerClass);

            containerElement.SafeMergeAttribute("data-focus-x", focusX);
            containerElement.SafeMergeAttribute("data-focus-y", focusY);
            containerElement.InnerHtml += imageElement.ToString(TagRenderMode.SelfClosing);

            return MvcHtmlString.Create(containerElement.ToString(TagRenderMode.Normal));
        }

        private static void SafeMergeAttribute(this TagBuilder tagElement, string attrName, string attrValue)
        {
            if (!String.IsNullOrEmpty(attrValue))
            {
                tagElement.MergeAttribute(attrName, attrValue);
            }
        }

        private static void AddSafe(this IDictionary<string, object> collection, string key, object value)
        {
            if (collection.ContainsKey(key))
            {
                collection[key] = value;
            }
            else
            {
                collection.Add(key, value);
            }
        }
    }
}
