namespace AverBlog.Api.ApiDtos
{
    public class BasePaginationRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int StartIndex => (PageNumber - 1) * PageSize;
       

    }

    public class UserFilter : BasePaginationRequest
    {
        public DateTime JoinedAfter { get; set; }

        public string KeyWord { get; set; }
    }
}


//pageSize 25 
//pageNumber 2