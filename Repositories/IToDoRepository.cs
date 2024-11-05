using BusinessObjects;

namespace Repositories
{
    public interface IToDoRepository
    {
        IEnumerable<ToDo> GetDeletedTodos(int teamId);
        void RestoreToDo(int todoId, int teamId);
        ToDo GetToDoDeletedById(int teamId, int todoId);
    }
}
