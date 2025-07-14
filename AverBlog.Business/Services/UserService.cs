using AverBlog.Business.IServices;
using AverBlog.Business.ServiceModels;
using AverBlog.Data.DADtos;
using AverBlog.Data.Enitities;
using AverBlog.Data.Exceptions;
using AverBlog.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AverBlog.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepsitory _userRepository;

        public UserService(IUserRepsitory userRepsitory)
        {
            _userRepository = userRepsitory;
        }
        public async Task<UserServiceModel> CreateUser(string userName, string email, string password)
        {
            var user = new User();
            user.Username = userName;
            user.Email = email;
            user.JoinedOn = DateTime.Now;
            user.Password = password;

            user.FullName = string.Empty;
            user.Bio = string.Empty;
            user.ProfileImageUrl = string.Empty;

             await _userRepository.CreateUser(user);

            return new UserServiceModel
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Bio = user.Bio,
                JoinedOn = user.JoinedOn,
                ProfileImageUrl = user.ProfileImageUrl
            };
        }

        public async Task<(int count, List<UserServiceModel>)> GetUsers(string keyWord, DateTime joinedAfter, int startIndex, int pageSize)
        {
           (int count, List<UsersFilterDataResponse> users) = await _userRepository.GetUsers(keyWord , joinedAfter, startIndex, pageSize);

            var userServiceModels = users.Select(x => new UserServiceModel
            {
                Id = x.Id,
                Username = x.Username,
                Email = x.Email,
                FullName = x.FullName,
                Bio = x.Bio,
                JoinedOn = x.JoinedOn,
                ProfileImageUrl = x.ProfileImageUrl,

                PostCount = x.PostCount,
            }).ToList();
            return (count, userServiceModels) ;
        }

        public async Task<UserServiceModel> UpdateUser(int id, string bio, string fullName, string profileImageUrl)
        {
            var user = await _userRepository.GetUserById(id);

            if(user == null)
            {
                throw new NotFoundException($"User with id {id} not found.");
            }

            user.Bio = bio;
            user.FullName = fullName;
            user.ProfileImageUrl = profileImageUrl;

            await _userRepository.UpdateUser(user);

            return new UserServiceModel
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Bio = user.Bio,
                JoinedOn = user.JoinedOn,
                Role = user.Role,
                ProfileImageUrl = user.ProfileImageUrl
                
            };
        }

        public async Task<UserServiceModel> AuthenticateUser(string email , string password)
        {
            var user = await _userRepository.GetUserByEmail(email);

            if(user == null)
            {
                throw new NotFoundException($"User with email {email} not found.");
            }

            if(user.Password != password)
            {
                throw new Exception("Invalid password.");
            }


            return new UserServiceModel
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Bio = user.Bio,
                JoinedOn = user.JoinedOn,
                Role = user.Role,
                ProfileImageUrl = user.ProfileImageUrl
            };

        }

        public async Task<UserServiceModel> GetUser(int id)
        {
            User user = await _userRepository.GetUserById(id);

            return new UserServiceModel
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Bio = user.Bio,
                JoinedOn = user.JoinedOn,
                ProfileImageUrl = user.ProfileImageUrl
            };
        }

        public async Task<IEnumerable<PostServiceModel>> GetUserTimeline(int id)
        {
            var posts = await _userRepository.GetUserTimeline(id);

            if(posts == null)
                throw new NotFoundException($"User with id {id} not found.");

            return posts.Select(x => new PostServiceModel
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                CreatedAt = x.CreatedAt,
                Category = x.Category,
                IsPublished = x.IsPublished,
                UserId=x.UserId
            });
        }

        public async Task<List<UserPostAnalysticsServiceDto>> GetMostActiveUsers()
        {
            var users = await _userRepository.GetMostActiveUsers();

            var serviceModelList = users.Select(x => new UserPostAnalysticsServiceDto
            {
                UserId = x.UserId,
                UserName = x.UserName,
                PostCount = x.PostCount,
            }).ToList() ;

            return serviceModelList;
        }
    }
}
