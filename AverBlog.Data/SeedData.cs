using AverBlog.Data.Contexts;
using AverBlog.Data.Enitities;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AverBlog.Data
{
    public static class SeedData
    {
        public static void SeedUsers(AppDbContext context , int count )
        {
            if (count <= 0) return;

            var userFaker = new Faker<User>()
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.FullName, f => f.Name.FullName())
                .RuleFor(u => u.Bio, f => f.Lorem.Sentences(2))
                .RuleFor(u => u.ProfileImageUrl, f => f.Internet.Avatar())
                .RuleFor(u => u.JoinedOn, f => f.Date.Past(3));

            var users = userFaker.Generate(count);
            context.Users.AddRange(users);
            context.SaveChanges();

             
        }

        public static void SeedPosts(AppDbContext context , int count)
        {
            if (count <= 0) return;

            var categories = new[] { "Tech", "Fitness", "Food", "Travel", "Gaming" };

            var userIds = context.Users.Select(u => u.Id).ToList();

            var postFaker = new Faker<Post>()
                .RuleFor(p => p.Title, f => f.Lorem.Sentence(5))
                .RuleFor(p => p.Content, f => f.Lorem.Paragraphs(2))
                .RuleFor(p => p.Category, f => f.PickRandom(categories))
                .RuleFor(p => p.IsPublished, f => f.Random.Bool(0.8f))
                .RuleFor(p => p.CreatedAt, f => f.Date.Past(2))
                .RuleFor(p => p.UserId, f => f.PickRandom(userIds));

            var posts = postFaker.Generate(count);
            context.Posts.AddRange(posts);
            context.SaveChanges();
        }
    }
}
