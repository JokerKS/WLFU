namespace JokerKS.WLFU
{
    public class Pager
    {
        public Pager(int page, int pageSize)
        {
            PageIndex = page;
            PageSize = pageSize;
        }
        public Pager()
        {
            PageIndex = 1;
            PageSize = 10;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public int PageCount
        {
            get
            {
                int count = TotalCount / PageSize;

                return TotalCount - count * PageSize == 0 && count > 0 ? count : count + 1;
            }
        }

        public int ItemsSkip
        {
            get
            {
                return (PageIndex - 1) * PageSize;
            }
        }
    }
}