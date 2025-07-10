using AverBlog.Data.DADtos;
using AverBlog.Data.Enitities;

namespace AverBlog.Data.IRepository
{
    public interface IUserRepsitory
    {
        Task<User> CreateUser(User user);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(int id);
        Task<(int count, List<UsersFilterDataResponse>)> GetUsers(string keyWord, DateTime joinedAfter, int startIndex, int pageSize);
        Task<List<Post>> GetUserTimeline(int id);
        Task<User> UpdateUser(User user);
    }
}