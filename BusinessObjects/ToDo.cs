using System.ComponentModel.DataAnnotations;

namespace BusinessObjects
{
    public class ToDo
    {
        [Key]
        public int ToDoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? DeletedAt { get; set; } // Thời gian xóa task
        public ICollection<User> AssignedUsers { get; set; } = new List<User>();
    }
}
