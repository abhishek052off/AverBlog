using AverBlog.Business.ServiceModels;

namespace AverBlog.Business.IServices
{
    public interface IUserService
    {
        Task<UserServiceModel> CreateUser(string userName, string email);
        Task<(int count, List<UserServiceModel>)> GetUsers(string keyWord, DateTime joinedAfter, int startIndex, int pageSize);
        Task<UserServiceModel> UpdateUser(int id, string bio, string fullName, string profileImageUrl);
    }
}