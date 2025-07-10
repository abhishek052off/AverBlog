using AverBlog.Api.ApiDtos;
using AverBlog.Api.SessionModels;
using AverBlog.Business.IServices;
using AverBlog.Business.ServiceModels;
using AverBlog.Data.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AverBlog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HomeController : ControllerBase
    {
        private readonly IUserService _userService;
         
        private UserSession _session;

        public HomeController(UserSession session ,IUserService userService)
        {
            _session = session;
            _userService = userService;
            
        }

        [HttpGet("whoami")]
        public async Task<ActionResult<UserResponse>> GetSessionUser()
        {
            //var contextUser = _httpContext?.User;

            //var id =  contextUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "0";

            UserServiceModel userServiceModel = await _userService.GetUser(_session.Id);

            return Ok(new UserResponse
            {
                Id = userServiceModel.Id,
                Username = userServiceModel.Username,
                Email = userServiceModel.Email,
                Bio = userServiceModel.Bio,
                FullName = userServiceModel.FullName,
                JoinedOn = userServiceModel.JoinedOn
            });

        }

        [HttpGet("user-timeline")]
        public async Task<ActionResult<List<PostServiceModel>>> GetUserTimeline()
        {
            var posts = await _userService.GetUserTimeline(_session.Id);
            return Ok(posts);
        }



    }
}
