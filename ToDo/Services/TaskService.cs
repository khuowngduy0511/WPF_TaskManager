using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Models;
using ToDo.Repositories;
using TaskEntity = ToDo.Models.Task;

namespace ToDo.Services
{

    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskEntity>> GetAllTasksAsync()
        {
            try
            {
                return await _taskRepository.GetAllTasksAsync();
            }
            catch (Exception ex)
            {
                // Log error
                throw;
            }
        }

        public async Task<TaskEntity> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetTaskByIdAsync(id);
        }

        public async Task<TaskEntity> CreateTaskAsync(TaskEntity task)
        {
            return await _taskRepository.AddTaskAsync(task);
        }

        public async Task<TaskEntity> UpdateTaskAsync(TaskEntity task)
        {
            return await _taskRepository.UpdateTaskAsync(task);
        }


        public async Task<IEnumerable<TaskEntity>> GetCriticalTasksAsync()
        {
            return await _taskRepository.GetCriticalTasksAsync();
        }

        public async Task<IEnumerable<TaskEntity>> SearchTasksAsync(string searchTerm)
        {
            return await _taskRepository.SearchTasksAsync(searchTerm);
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(int id)
        {
            await _taskRepository.DeleteTaskAsync(id);
        }
    }
}
