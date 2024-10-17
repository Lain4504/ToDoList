using System;
using System.Collections.Generic;
using System.Linq;
using BusinessObject; 
using BusinessObjects;
using DataAccessLayer;
using Repositories;

namespace YourNamespace.Repositories
{
    public class TrashRepository : ITrashRepository
    {
        private readonly ToDoListContext _context;

        public TrashRepository(ToDoListContext context)
        {
            _context = context;
        }

        public IEnumerable<TrashItem> GetAllDeletedTasks()
        {
            return _context.DeletedTasks.ToList();
        }

        public TrashItem GetDeletedTaskById(int id)
        {
            return _context.DeletedTasks.FirstOrDefault(t => t.DeletedId == id);
        }

        public void RestoreTask(int id)
        {
            var deletedTask = _context.DeletedTasks.Find(id);
            if (deletedTask != null)
            {
                _context.DeletedTasks.Remove(deletedTask);
                _context.ToDos.Add(new ToDo 
                {
                    Id = deletedTask.TaskId, 
                    Title = deletedTask.TaskName,
                   
                });
                _context.SaveChanges();
            }
        }

        public void DeleteTaskPermanently(int id)
        {
            var deletedTask = _context.DeletedTasks.Find(id);
            if (deletedTask != null)
            {
                _context.DeletedTasks.Remove(deletedTask);
                _context.SaveChanges();
            }
        }
    }
}
