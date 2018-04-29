using System.Web.Mvc;
using JokerKS.WLFU.Entities.Menu;
using System.Collections.Generic;

namespace JokerKS.WLFU.Entities.HtmlHelpers
{
    public static class AdminHelpers
    {
        public static MvcHtmlString AdminMenu(this HtmlHelper html, string action, string controller)
        {
            TagBuilder ul = new TagBuilder("ul");
            ul.AddCssClass("nav nav-pills nav-stacked");
            foreach (var item in MenuManager.GetMenuItems(action, controller))
            {
                ul.InnerHtml += $"<li {(item.IsSelected ? "class=active" : string.Empty)}><a href='{new UrlHelper(html.ViewContext.RequestContext).Action(item.Action, item.Controller)}'>{item.Name}</a></li>";
                if(item.HasChildren && item.Children != null)
                {
                    ul.InnerHtml += $"<li class='dropdown'>{AdminHelpers.GenerateMenuStructure(html, item.Children)}</li>";
                }
            }

            return MvcHtmlString.Create(ul.ToString());
        }

        private static string GenerateMenuStructure(HtmlHelper html, IEnumerable<MenuItem> items)
        {
            string result = string.Empty;

            TagBuilder ul = new TagBuilder("ul");
            ul.AddCssClass("nav nav-pills");
            ul.Attributes.Add("style", $"margin-left:10px;");

            foreach (var item in items)
            {
                ul.InnerHtml += $"<li {(item.IsSelected ? "class=active" : string.Empty)}><a href='{new UrlHelper(html.ViewContext.RequestContext).Action(item.Action, item.Controller)}'>{item.Name}</a></li>";
            }

            return ul.ToString();
        }
    }
}