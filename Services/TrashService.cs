using System.Collections.Generic;
using BusinessObject; 
using Repositories;

namespace Services
{
    public class TrashService
    {
        private readonly ITrashRepository _trashRepository;

        public TrashService(ITrashRepository trashRepository)
        {
            _trashRepository = trashRepository;
        }

        // Lấy tất cả các task đã xóa
        public IEnumerable<TrashItem> GetAllDeletedTasks()
        {
            return _trashRepository.GetAllDeletedTasks();
        }

        // Lấy task đã xóa theo ID
        public TrashItem GetDeletedTaskById(int id)
        {
            return _trashRepository.GetDeletedTaskById(id);
        }

        // Khôi phục task đã xóa
        public void RestoreTask(int id)
        {
            _trashRepository.RestoreTask(id);
        }

        // Xóa vĩnh viễn task đã xóa
        public void DeleteTaskPermanently(int id)
        {
            _trashRepository.DeleteTaskPermanently(id);
        }
    }
}
