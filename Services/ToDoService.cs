using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoRepository _toDoRepository;

        public ToDoService(ToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        public IEnumerable<ToDo> GetToDosForTeam(int teamId)
        {
            var team = _toDoRepository.GetTeamById(teamId);
            if (team == null)
            {
                throw new Exception("Team not found");
            }
            return _toDoRepository.GetToDosByTeam(teamId);
        }

        public void AddToDoForTeam(int teamId, ToDo todo)
        {
            var team = _toDoRepository.GetTeamById(teamId);
            if (team == null)
            {
                throw new Exception("Team not found");
            }
            todo.TeamId = teamId;
            todo.Team = team;
            _toDoRepository.AddToDo(todo);
        }

        public void UpdateToDoForTeam(int teamId, ToDo todo)
        {
            var existingTodo = _toDoRepository.GetToDoById(teamId, todo.Id);
            if (existingTodo == null)
            {
                throw new Exception("ToDo not found");
            }
            existingTodo.Title = todo.Title;
            existingTodo.Description = todo.Description;
            existingTodo.DueDate = todo.DueDate;
            existingTodo.IsCompleted = todo.IsCompleted;
            _toDoRepository.UpdateToDo(existingTodo);
        }

        public void DeleteToDoForTeam(int teamId, int todoId)
        {
            var todo = _toDoRepository.GetToDoById(teamId, todoId);
            if (todo == null)
            {
                throw new Exception("ToDo not found");
            }
            _toDoRepository.DeleteToDo(todoId);
        }

        public ToDo GetToDoDetails(int teamId, int todoId)
        {
            var todo = _toDoRepository.GetToDoById(teamId, todoId);
            if (todo == null)
            {
                throw new Exception("ToDo not found");
            }
            return todo;
        }
    }
}
