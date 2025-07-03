using System.ComponentModel.DataAnnotations;

namespace AverBlog.Data.Enitities
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Content { get; set; }
        public string Category { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
