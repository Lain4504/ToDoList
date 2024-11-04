using BusinessObjects;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ToDoListContext _context;
        private User _currentUser;

        // Constructor duy nhất với các phụ thuộc cần thiết
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository), "User repository cannot be null.");
        }

        // Constructor nhận cả IUserRepository và ToDoListContext
        public UserService(IUserRepository userRepository, ToDoListContext context)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository), "User repository cannot be null.");
            _context = context ?? throw new ArgumentNullException(nameof(context), "Database context cannot be null.");
        }
        // Phương thức đăng nhập
        public bool Login(string username, string password)
        {
            if (_context == null)
                throw new InvalidOperationException("Database context is not initialized.");

            string passwordHash = HashPassword(password);
            _currentUser = _context.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == passwordHash);
            return _currentUser != null;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        // UserService.cs
        public User GetCurrentUser()
        {
            if (_currentUser == null)
            {
                throw new InvalidOperationException("No user is currently logged in.");
            }
            return _currentUser;
        }

        public async Task<User> GetUserTeamsAsync(int userId)
        {
            return await _userRepository.GetUserWithTeamsAsync(userId);
        }
        public User GetUser(int userId)
        {
            return _userRepository.GetUser(userId);
        }


        public IEnumerable<Team> GetTeamsByUserId(int userId)
        {
            var user = _context.Users
                .Include(u => u.Teams)
                .FirstOrDefault(u => u.UserId == userId);

            return user?.Teams ?? Enumerable.Empty<Team>();
        }


    }
}
