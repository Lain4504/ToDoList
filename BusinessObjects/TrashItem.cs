using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject
{
    public class TrashItem
    {
        [Key] // Đánh dấu DeletedId là khóa chính
        public int DeletedId { get; set; }

        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}
