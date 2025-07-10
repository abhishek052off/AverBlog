using AverBlog.Api.ApiDtos;
using AverBlog.Business.IServices;
using AverBlog.Business.ServiceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AverBlog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService , IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody]LoginRequest request)
        {
            UserServiceModel user = await _userService.AuthenticateUser(request.Email , request.Password);

            var token = GenerateToken(user);

            var response = new LoginResponseModel
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };
            return Ok(response);
        }


        private string GenerateToken(UserServiceModel user)
        {
            var jwtConfig = _configuration.GetSection("JwtSettings");

            var key = jwtConfig["Key"];
            var issuer = jwtConfig["Issuer"];
            var audience = jwtConfig["Audience"];
            var expirationMinutes = jwtConfig["ExpiryInMinutes"];

            var claims = new[]
            {
                new Claim( ClaimTypes.Name, user.Username), //ClaimTypes.Name
                new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()),//ClaimTypes.NameIdentifier
                new Claim(ClaimTypes.Email , user.Email), //ClaimTypes.Email
                new Claim("full-name", user.FullName),
                new Claim(ClaimTypes.Role , user.Role)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(expirationMinutes)),
                claims: claims,

                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }


    }
}
