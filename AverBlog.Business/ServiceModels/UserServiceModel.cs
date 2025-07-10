using AverBlog.Data.Enitities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AverBlog.Business.ServiceModels
{
    public  class UserServiceModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime JoinedOn { get; set; }
        public string Role { get; set; }
        public int PostCount { get; set; }
    }
}
