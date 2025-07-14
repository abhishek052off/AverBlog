using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AverBlog.Data.DADtos
{
    public class UserPostAnalysticsDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int PostCount { get; set; }
    }
}
