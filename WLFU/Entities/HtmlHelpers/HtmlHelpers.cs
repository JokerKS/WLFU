using System;
using System.Text;
using System.Web.Mvc;

namespace JokerKS.WLFU.Entities.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
                                              Pager pager,
                                              string hiddenInputSelector)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pager.PageCount; i++)
            {
                TagBuilder input = new TagBuilder("input");
                input.Attributes.Add("type", "button");
                input.Attributes.Add("value", i.ToString());
                input.Attributes.Add("class", "btn btn-default" + (i == pager.CurrentPage ? " btn-primary" : string.Empty));
                input.Attributes.Add("onclick", "$('"+ hiddenInputSelector + "').val("+ i +");this.form.submit();");

                result.Append(input.ToString());

                //TagBuilder tag = new TagBuilder("a");
                //tag.MergeAttribute("href", pageUrl(i));
                //tag.InnerHtml = i.ToString();
                //if (i == pager.CurrentPage)
                //{
                //    tag.AddCssClass("selected");
                //    tag.AddCssClass("btn-primary");
                //}
                //tag.AddCssClass("btn btn-default");
                //result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}