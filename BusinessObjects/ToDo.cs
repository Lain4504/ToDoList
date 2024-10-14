using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects
{
    public class ToDo
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartDate {  get; set; }
        public bool IsCompleted {  get; set; }
        public DateTime? DeletedAt { get; set; } // Thời gian xóa task
        public ICollection<User> AssignedUsers { get; set; } = new List<User>();
        public TodoState TodoState { get; set; }
        public TodoImportance TodoImportance { get; set; }
    }
    public enum TodoState
    {
        InProgress,
        Complete,
        NotStarted,
        Late,
        Archived,
        Deleted
    }
    public enum TodoImportance
    {
        Low,
        Medium,
        High
    }
}
