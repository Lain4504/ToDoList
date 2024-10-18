using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ToDoListContext _context;

        public UserRepository(ToDoListContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserWithTeamsAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Teams)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }
}
