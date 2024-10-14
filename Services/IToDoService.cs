using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IToDoService
    {
        IEnumerable<ToDo> GetToDosForTeam(int teamId);
        void AddToDoForTeam(int teamId, ToDo todo);
        void UpdateToDoForTeam(int teamId, ToDo todo);
        void DeleteToDoForTeam(int teamId, int todoId);
        ToDo GetToDoDetails(int teamId, int todoId);
    }
}
