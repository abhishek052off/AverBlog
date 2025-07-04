namespace AverBlog.Api.ApiDtos
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime JoinedOn { get; set; }

        public int TotalPosts { get; set; }
    }
}
