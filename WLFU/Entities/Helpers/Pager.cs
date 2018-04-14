namespace JokerKS.WLFU
{
    public class Pager
    {
        #region Pager()
        public Pager() : this(1, 10)
        {
        }
        public Pager(int page) : this(page, 10)
        {
        }
        public Pager(int page, int itemsPerPage)
        {
            CurrentPage = page < 1 ? 1 : page;
            ItemsPerPage = itemsPerPage < 1 ? 10 : itemsPerPage;
            SortDirection = Entities.Helpers.SortDirection.ASC();
        } 
        #endregion

        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalCount { get; set; }

        public string SearchExpression { get; set; }
        public string SortExpression { get; set; }
        public string SortDirection { get; set; }

        public int PageCount
        {
            get
            {
                return (TotalCount + ItemsPerPage - 1) / ItemsPerPage;
            }
        }

        public int ItemsSkip
        {
            get
            {
                return (CurrentPage - 1) * ItemsPerPage;
            }
        }
    }
}