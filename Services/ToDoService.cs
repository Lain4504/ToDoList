using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoRepository _toDoRepository;

        public ToDoService(IToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository ?? throw new ArgumentNullException(nameof(toDoRepository));
        }

        public IEnumerable<ToDo> GetDeletedTodos(int teamId)
        {
            return _toDoRepository.GetDeletedTodos(teamId);
        }

        public void RestoreToDo(int todoId, int teamId)
        {
            var todo = _toDoRepository.GetToDoDeletedById(teamId, todoId);
            if (todo == null)
            {
                throw new Exception("ToDo not found");
            }
            _toDoRepository.RestoreToDo(todoId, teamId);
        }
    }
}
