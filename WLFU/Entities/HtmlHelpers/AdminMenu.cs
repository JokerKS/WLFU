using System.Web.Mvc;
using JokerKS.WLFU.Entities.Menu;

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
            }

            return MvcHtmlString.Create(ul.ToString());
        }
    }
}