using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
namespace Repositories
{
    public interface ITrashRepository
    {
        IEnumerable<TrashItem> GetAllDeletedTasks();
        TrashItem GetDeletedTaskById(int id);
        void RestoreTask(int id);
        void DeleteTaskPermanently(int id);
    }
}
