﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Data;
using ToDo.Models;
using TaskEntity = ToDo.Models.Task;

namespace ToDo.Repositories
{
    // Repositories/TaskRepository.cs
    public class TaskRepository : ITaskRepository
    {
        private readonly TodoDbContext _context;

        public TaskRepository(TodoDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<TaskEntity>> GetAllTasksAsync()
        {
            return await _context.Tasks.Where(t => !t.IsComplete).ToListAsync();
        }

        public async Task<TaskEntity> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<TaskEntity> AddTaskAsync(TaskEntity task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskEntity> UpdateTaskAsync(TaskEntity task)
        {
            try
            {
                _context.Entry(task).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return task;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in TaskRepository.UpdateTaskAsync: {ex.Message}");
                throw;
            }
        }   


        public async System.Threading.Tasks.Task DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TaskEntity>> GetCriticalTasksAsync()
        {
            return await _context.Tasks
                .Where(t => t.TaskImportance == TaskImportance.Critical)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskEntity>> SearchTasksAsync(string searchTerm)
        {
            return await _context.Tasks
                .Where(t => t.Title.Contains(searchTerm) ||
                           t.Description.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetCompletedTasksAsync()
        {
            return await _context.Tasks
                .Where(t => t.IsComplete)
                .ToListAsync();
        }

        public IEnumerable<TaskEntity> GetTasksDueInMonth(int year, int month)
        {
            return _context.Tasks
                .Where(t => t.DueDate.Year == year && t.DueDate.Month == month && !t.IsComplete)
                .ToList();
        }
    }
}
