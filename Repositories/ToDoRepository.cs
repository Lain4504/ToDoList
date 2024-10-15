using BusinessObjects;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ToDoListContext _context;

        public ToDoRepository(ToDoListContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ToDoRepository()
        {
        }

        public Team GetTeamById(int teamId)
        {
            return _context.Teams.FirstOrDefault(t => t.TeamId == teamId);
        }
        public IEnumerable<ToDo> GetToDosByTeam(int teamId)
        {
            return _context.ToDos.Where(t => t.TeamId == teamId && t.DeletedAt == null).ToList();
        }
        public void AddToDo(ToDo todo)
        {
            _context.ToDos.Add(todo);
            _context.SaveChanges();
        }
        public void UpdateToDo(ToDo todo)
        {
            _context.ToDos.Update(todo);
            _context.SaveChanges();
        }
        public void DeleteToDo(int todoId)
        {
            var todo = _context.ToDos.FirstOrDefault(t => t.Id == todoId);
            if (todo != null)
            {
                todo.DeletedAt = DateTime.Now;
                _context.ToDos.Update(todo);
                _context.SaveChanges();
            }
        }

        public ToDo GetToDoById(int teamId, int todoId)
        {
            return _context.ToDos.FirstOrDefault(t => t.TeamId == teamId && t.Id == todoId && t.DeletedAt == null);
        }
        public async Task<IEnumerable<ToDo>> GetToDoByTitleAsync(string title, int teamId)
        {
            return await _context.ToDos
                .Where(t => t.Title.Contains(title) && t.TeamId == teamId)
                .ToListAsync();
        }
    }
}
