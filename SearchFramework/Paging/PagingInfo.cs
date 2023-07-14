namespace SearchFramework.Paging;

public class PagingInfo
{
    public int Skip { get; set; }
    public int Take { get; set; }

    public sealed class DefaultPagingInfo : PagingInfo
    {
        public DefaultPagingInfo()
        {
            this.Skip = 0;
            this.Take = 50;
        }
    }

    public static PagingInfo Default => new DefaultPagingInfo();
}