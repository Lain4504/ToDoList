using BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DataAccessLayer;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ToDoListContext _context;

        public ToDoRepository(ToDoListContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<ToDo> GetDeletedTodos(int teamId)
        {
            return _context.ToDos
                .Where(t => t.IsDeleted && t.DeletedAt >= DateTime.Now.AddDays(-7)
                              && t.TeamId == teamId)
                .ToList();
        }

        public ToDo GetToDoById(int id)
        {
            return _context.ToDos.Find(id);
        }

        public void RestoreToDo(int todoId, int teamId)
        {
            var todo = GetToDoDeletedById(teamId, todoId);

            if (todo != null)
            {
                todo.IsDeleted = false;
                todo.DeletedAt = null;
                _context.SaveChanges();
            }
        }
        public ToDo GetToDoDeletedById(int teamId, int todoId)
        {
            return _context.ToDos
                .FirstOrDefault(t => t.TeamId == teamId && t.Id == todoId && t.DeletedAt != null);
        }

    }
}
