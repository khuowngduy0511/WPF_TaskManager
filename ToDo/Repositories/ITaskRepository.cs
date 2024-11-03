using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Models;
using TaskEntity = ToDo.Models.Task;

namespace ToDo.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskEntity>> GetAllTasksAsync();
        Task<TaskEntity> GetTaskByIdAsync(int id);
        Task<TaskEntity> AddTaskAsync(TaskEntity task);
        Task<TaskEntity> UpdateTaskAsync(TaskEntity task);
        System.Threading.Tasks.Task DeleteTaskAsync(int id);
        Task<IEnumerable<TaskEntity>> GetCriticalTasksAsync();
        Task<IEnumerable<TaskEntity>> SearchTasksAsync(string searchTerm);
    }
}
