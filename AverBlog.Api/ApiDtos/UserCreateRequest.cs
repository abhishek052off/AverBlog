using System.ComponentModel.DataAnnotations;

namespace AverBlog.Api.ApiDtos
{
    public class UserCreateRequest
    {
        [Required]
        public string  UserName { get; set; }
        [Required]
        public string  Email { get; set; }

        [Required]
        public string Password { get; set; }
        
 
    }

    public class UserUpdateRequest
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Bio { get; set; }

        [Required]
        [Url]
        public string ProfileImageUrl { get; set; }

 
    }
}
