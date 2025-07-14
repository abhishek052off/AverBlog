using AverBlog.Api.ApiDtos;
using AverBlog.Business.IServices;
using AverBlog.Business.ServiceModels;
using AverBlog.Data.Contexts;
using AverBlog.Data.Enitities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AverBlog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnlyPolicy")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly AppDbContext _dbContext;

        public AnalyticsController(IUserService userService , AppDbContext dbContext)
        {
            _userService = userService;
            //DONT DO THIS 
            _dbContext = dbContext;
        }

        [HttpGet("most-active-users")]
        public async Task<ActionResult<List<UserServiceModel>>> GetMostActiveUsers()
        {
            var users = await _userService.GetMostActiveUsers();
            //Map it to A dto 
            return Ok(users);
        }

        //Dirty approach , we are injecting DbContext directly in the controller , NEVER DO THIS 
        //THIS IS JUST FOR DEMO , SO that we can cover more queries, 
        //ALWAYS CREATE A PROPER FLOW IN PROD APPS

        [HttpGet("posts-published-last-month")]
        public async Task<ActionResult> GetPostsPublishedLastWeek()
        {
            var posts = await _dbContext.Posts.Where(post=> post.IsPublished && post.CreatedAt.Date > DateTime.Now.AddDays(-30).Date)
                .OrderByDescending(post => post.CreatedAt)
                .ToListAsync();

            return Ok(posts);
        }

        [HttpGet("posts-published-in")]
        public async Task<ActionResult> GetPostsPublishedIn([FromQuery]YearAndMonthInput input)
        {
            var posts = await _dbContext.Posts.Where(post => post.IsPublished
                                                    && post.CreatedAt.Year == input.Year
                                                    && post.CreatedAt.Month == input.Month)
                .OrderByDescending(post => post.CreatedAt.Date)
                .ThenBy(post=>post.Title)
                .ToListAsync();

            return Ok(posts);
        }


        //Anonymous Objects   
        [HttpGet("number-of-posts-published-grouped-by-month")]
        public async Task<ActionResult> GetPostsPublishedGroupedByMonth()
        {

            //we disregard the month , and get the number of posts published per month 
            //Created anon tytpe so that i dont need to create a type for every example 
            var posts = await _dbContext.Posts.Where(post => post.IsPublished)
                .GroupBy(post => new { post.CreatedAt.Year, post.CreatedAt.Month })
                .Select( grp => new  
                {
                    Year = grp.Key.Year,
                    Month = grp.Key.Month,
                    NumberOfPosts = grp.Count(),
                }
                )
                .OrderBy(grp => grp.Year)
                .ThenByDescending(grp => grp.Month)
                .ToListAsync();

             

            return Ok(posts);
        }

        [HttpGet("number-of-posts-published-grouped-by-month-include-content")]
        public async Task<ActionResult> GetPostsPublishedGroupedByMonthINCCOntent()
        {

            //we disregard the month , and get the number of posts published per month 
            //Created anon tytpe so that i dont need to create a type for every example 
            var posts = await _dbContext.Posts.Where(post => post.IsPublished)
                .GroupBy(post => new { post.CreatedAt.Year, post.CreatedAt.Month })
                .Select(grp => new PostGroup
                {
                    Year = grp.Key.Year,
                    Month = grp.Key.Month,
                    NumberOfPosts = grp.Count(),
                }
                )
                .OrderBy(grp => grp.Year)
                .ThenByDescending(grp => grp.Month)
                .ToListAsync();


            //iterate every item and fetch from the posts table
            foreach (var post in posts)
            {
                post.Content = await _dbContext.Posts.Where(x => x.CreatedAt.Year == post.Year && x.CreatedAt.Month == post.Month && x.IsPublished)
                    .OrderByDescending(post => post.CreatedAt.Date)
                    .ThenBy(post => post.Title)
                    .ToListAsync();
            }


            return Ok(posts);
        }

        [HttpGet("number-of-posts-published-grouped-by-category")]
        public async Task<ActionResult> GetPostsPublishedGroupedByCategory()
        {

            //we disregard the month , and get the number of posts published per month 
            //Created anon tytpe so that i dont need to create a type for every example 
            var posts = await _dbContext.Posts.Where(post => post.IsPublished)
                .GroupBy(post => new { post.Category })
                .Select(grp => new
                {
                    Category = grp.Key.Category,
                    NumberOfPosts = grp.Count(),
                }
                )
                .OrderBy(grp => grp.Category)
                .ToListAsync();



            return Ok(posts);
        }

    }
}


public class PostGroup
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int NumberOfPosts { get; set; }
    public List<Post> Content { get; set; }
}