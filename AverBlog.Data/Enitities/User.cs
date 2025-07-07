using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AverBlog.Data.Enitities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string FullName { get; set; }
        public string Bio { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime JoinedOn { get; set; }

        [Required]
        public string Password { get; set; }

        public List<Post> Posts { get; set; }
    }
}
