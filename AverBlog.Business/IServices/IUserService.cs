using AverBlog.Business.ServiceModels;

namespace AverBlog.Business.IServices
{
    public interface IUserService
    {
        Task<UserServiceModel> AuthenticateUser(string email, string password);
        Task<UserServiceModel> CreateUser(string userName, string email, string password);
        Task<UserServiceModel> GetUser(int id);
        Task<(int count, List<UserServiceModel>)> GetUsers(string keyWord, DateTime joinedAfter, int startIndex, int pageSize);
        Task<IEnumerable<PostServiceModel>> GetUserTimeline(int id);
        Task<UserServiceModel> UpdateUser(int id, string bio, string fullName, string profileImageUrl);
    }
}