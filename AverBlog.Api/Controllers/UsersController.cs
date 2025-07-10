using AverBlog.Api.ApiDtos;
using AverBlog.Business.IServices;
using AverBlog.Business.ServiceModels;
using AverBlog.Data;
using AverBlog.Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AverBlog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
    

        public UsersController(IUserService userService  )
        {
            _userService = userService;
        }

        [HttpPost("create-user")]
        [AllowAnonymous]
        public async Task<ActionResult<UserResponse>> CreateUser([FromBody]UserCreateRequest request)
        {
            var userServiceModel =  await _userService.CreateUser(request.UserName , request.Email ,request.Password);

            var apiResponse = new UserResponse
            {
                Id = userServiceModel.Id,
                Username = userServiceModel.Username,
                Email = userServiceModel.Email,
                Bio = userServiceModel.Bio,
                FullName = userServiceModel.FullName,
                JoinedOn = userServiceModel.JoinedOn
            };

            return Ok(apiResponse);
        }

        [HttpPut("update-user/{id}")]
        public async Task<ActionResult<UserResponse>> UpdateUser(int id, [FromBody]UserUpdateRequest request)
        {
            UserServiceModel userServiceModel = await _userService.UpdateUser(id, request.Bio, request.FullName , request.ProfileImageUrl);

            var apiResponse = new UserResponse
            {
                Id = userServiceModel.Id,
                Username = userServiceModel.Username,
                Email = userServiceModel.Email,
                Bio = userServiceModel.Bio,
                FullName = userServiceModel.FullName,
                JoinedOn = userServiceModel.JoinedOn,
                ProfileImageUrl = userServiceModel.ProfileImageUrl
            };

            return Ok(apiResponse);
        }


        [HttpGet("users")]
        [Authorize(Policy = "AdminOnlyPolicy")]
        public async Task< ActionResult< PaginatedResponse<UserResponse>>> GetUsers([FromQuery] UserFilter request)
        {
            (int count , List<UserServiceModel>? userServiceList) = await _userService.GetUsers(request.KeyWord,request.JoinedAfter, request.StartIndex , request.PageSize);

            var apiResponse = new List<UserResponse>();

            foreach (var userServiceModel in userServiceList)
            {
                var userResponse = new UserResponse
                {
                    Id = userServiceModel.Id,
                    Username = userServiceModel.Username,
                    Email = userServiceModel.Email,
                    Bio = userServiceModel.Bio,
                    FullName = userServiceModel.FullName,
                    JoinedOn = userServiceModel.JoinedOn,
                    ProfileImageUrl = userServiceModel.ProfileImageUrl,
                    TotalPosts = userServiceModel.PostCount
                };

                apiResponse.Add(userResponse);
            }


            var response = new PaginatedResponse<UserResponse>
            {
                TotalCount = count,
                Data = apiResponse
            };


            return Ok(response);
        }


        


    }
}
