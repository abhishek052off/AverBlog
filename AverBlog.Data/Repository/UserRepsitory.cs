using AverBlog.Data.Contexts;
using AverBlog.Data.Enitities;
using AverBlog.Data.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AverBlog.Data.Repository
{
    public class UserRepsitory : IUserRepsitory
    {
        private readonly AppDbContext _context;

        public UserRepsitory(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            return user;
        }


        

        public async Task< (int count , List<User> )  > GetUsers(string keyWord, DateTime joinedAfter, int startIndex, int pageSize)
        {
            IQueryable<User> usersQuery = _context.Users;


            //Aplying the filters 
            if (joinedAfter != DateTime.MinValue)
            {
                usersQuery = usersQuery.Where(x => x.JoinedOn.Date > joinedAfter.Date);
            }

            if(!string.IsNullOrEmpty(keyWord))
            {
                usersQuery = usersQuery.Where(x => x.FullName.Contains(keyWord) || x.Bio.Contains(keyWord));
            }

            //numebr of records matched count * after filters 
            int totalRecordsMatched = await usersQuery.CountAsync();

            //taking a slice of the records
            usersQuery = usersQuery.Skip(startIndex).Take(pageSize);

            var userMaterialisedList = await usersQuery.ToListAsync();
            return (totalRecordsMatched , userMaterialisedList) ; 
        }

        public async Task<User> UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
