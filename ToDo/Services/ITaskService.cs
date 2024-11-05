    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TaskEntity = ToDo.Models.Task;

    namespace ToDo.Services
    {
        public interface ITaskService
        {
            Task<IEnumerable<TaskEntity>> GetAllTasksAsync();
            Task<TaskEntity> GetTaskByIdAsync(int id);
            Task<TaskEntity> CreateTaskAsync(TaskEntity task);
            Task<TaskEntity> UpdateTaskAsync(TaskEntity task);
            Task DeleteTaskAsync(int id);
            Task<IEnumerable<TaskEntity>> GetCriticalTasksAsync();
            Task<IEnumerable<TaskEntity>> SearchTasksAsync(string searchTerm);
            Task<IEnumerable<TaskEntity>> GetCompletedTasksAsync();
            IEnumerable<TaskEntity> GetTasksDueInMonth(int year, int month);
        }
    }
