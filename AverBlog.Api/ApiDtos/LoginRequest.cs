using System.ComponentModel.DataAnnotations;

namespace AverBlog.Api.ApiDtos
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
