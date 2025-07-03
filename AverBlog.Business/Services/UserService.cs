using AverBlog.Business.IServices;
using AverBlog.Business.ServiceModels;
using AverBlog.Data.Enitities;
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
        public async Task<UserServiceModel> CreateUser(string userName, string email)
        {
            var user = new User();
            user.Username = userName;
            user.Email = email;
            user.JoinedOn = DateTime.Now;

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
           (int count, List<User> users) = await _userRepository.GetUsers(keyWord , joinedAfter, startIndex, pageSize);

            var userServiceModels = users.Select(x => new UserServiceModel
            {
                Id = x.Id,
                Username = x.Username,
                Email = x.Email,
                FullName = x.FullName,
                Bio = x.Bio,
                JoinedOn = x.JoinedOn,
                ProfileImageUrl = x.ProfileImageUrl
            }).ToList();
            return (count, userServiceModels) ;
        }

        public async Task<UserServiceModel> UpdateUser(int id, string bio, string fullName, string profileImageUrl)
        {
            var user = await _userRepository.GetUserById(id);

            if(user == null)
            {
                throw new Exception($"User with id {id} not found.");
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
                ProfileImageUrl = user.ProfileImageUrl
            };
        }
    }
}
