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
        void RestoreToDo(int todoId, int teamId);
        IEnumerable<ToDo> GetDeletedTodos(int teamId); 
    }
}
