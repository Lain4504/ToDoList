using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Team
    {
            public int TeamId { get; set; }
            public string Name { get; set; }

            // Admin user relationship
            public int AdminUserId { get; set; }
            public User AdminUser { get; set; } // One-to-many relationship

            public ICollection<User> Members { get; set; } = new List<User>(); // Many-to-many relationship
            public ICollection<ToDo> ToDos { get; set; } = new List<ToDo>();
        }

}
