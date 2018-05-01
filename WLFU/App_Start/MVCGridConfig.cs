[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(JokerKS.WLFU.MVCGridConfig), "RegisterGrids")]

namespace JokerKS.WLFU
{
    using MVCGrid.Models;
    using MVCGrid.Web;
    using Entities.Product;
    using System;

    public static class MVCGridConfig 
    {
        public static void RegisterGrids()
        {
            MVCGridDefinitionTable.Add("AdminListOfProducts", new MVCGridBuilder<Product>()
                .WithAuthorizationType(AuthorizationType.Authorized)
                .WithPageParameterNames("Active")
                .AddColumns(cols =>
                {
                    cols.Add().WithColumnName("Name")
                        .WithHeaderText("Name")
                        .WithValueExpression(i => i.Name)
                        .WithSorting(true);
                    cols.Add().WithColumnName("Price")
                        .WithHeaderText("Price")
                        .WithValueExpression(i => i.Price.ToString())
                        .WithSorting(true);
                    cols.Add().WithColumnName("Category")
                        .WithHeaderText("Category")
                        .WithValueExpression(i => i.Category.Name)
                        .WithSorting(true);
                    cols.Add().WithColumnName("IsPublished")
                        .WithHeaderText("Is Published")
                        .WithValueExpression(i => i.IsPublished.ToString())
                        .WithSorting(true);
                    cols.Add().WithColumnName("IsActive")
                        .WithHeaderText("Is Active")
                        .WithValueExpression(i => i.IsActive.ToString())
                        .WithSorting(true)
                        .WithFiltering(true);
                    cols.Add().WithColumnName("Designer")
                        .WithHeaderText("Designer")
                        .WithValueExpression(i => i.Designer.UserName)
                        .WithSorting(true);
                    cols.Add().WithColumnName("AvailableAmount")
                        .WithHeaderText("Available Amount")
                        .WithValueExpression(i => i.AvailableAmount.ToString())
                        .WithSorting(true);
                    cols.Add("Actions")
                        .WithHeaderText(string.Empty)
                        .WithHtmlEncoding(false)
                        .WithValueExpression((p, c) => c.UrlHelper.Action("ProductDetails", "Admin", new { productId = p.Id }))
                        .WithValueTemplate("<a href='{Value}' title='Show details'><i class='fa fa-info-circle fa-2x'></i></a>");
                })
                .WithSorting(true, "Name")
                .WithPaging(true, 10)
                .WithFiltering(true)
                .WithAdditionalQueryOptionNames("Search")
                .WithRowCssClassExpression(p => p.IsActive ? "success" : "")
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;

                    var pager = new Pager();
                    pager.SortExpression = options.SortColumnName;
                    pager.SortDirection = options.SortDirection == SortDirection.Asc ? "ASC" : "DESC";
                    pager.ItemsPerPage = options.ItemsPerPage.HasValue ? options.ItemsPerPage.Value : 10;
                    pager.CurrentPage = options.PageIndex.HasValue ? options.PageIndex.Value : 1;

                    switch (pager.SortExpression)
                    {
                        case "Category":
                            pager.SortExpression = "Category.Name";
                            break;
                        case "Designer":
                            pager.SortExpression = "Designer.UserName";
                            break;
                        default:
                            break;
                    }

                    pager.SearchExpression = options.GetAdditionalQueryOptionString("Search");

                    // ”становка параметр≥в по замовчуванню дл€ ф≥льтр≥в
                    var isActiveParam = options.GetPageParameterString("active");
                    var isActiveFilter = options.GetFilterString("IsActive");
                    // якщо додатковий параметер дл€ гр≥да не пустий ≥ пустий параметер ф≥льтра активност≥
                    if (!string.IsNullOrEmpty(isActiveParam) && string.IsNullOrEmpty(isActiveFilter))
                    {
                        // ”становка параметра ф≥льтра активност≥
                        options.Filters["IsActive"] = isActiveParam;
                        isActiveFilter = isActiveParam;
                    }

                    bool? isActive = null;
                    if(!string.IsNullOrEmpty(isActiveFilter))
                    {
                        if (isActiveFilter != "-")
                        {
                            isActive = Convert.ToBoolean(isActiveFilter);
                        }
                    }

                    return new QueryResult<Product>
                    {
                        Items = ProductManager.GetList(pager, isActive),
                        TotalRecords = pager.TotalCount
                    };
                })
            );
        }
    }
}