namespace AverBlog.Api.ApiDtos
{
    public class PaginatedResponse<T> where T : class
    {
        public int TotalCount { get; set; }
        public List<T> Data { get; set; }
    }
}
