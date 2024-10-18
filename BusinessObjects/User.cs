using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Để lưu hash mật khẩu
        public ICollection<Team> Teams { get; set; } = new List<Team>();
        public ICollection<ToDo> ToDos { get; set; } = new List<ToDo>();
    }
}
