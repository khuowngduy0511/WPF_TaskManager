using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using ToDo.Models;
using ToDo.Services;
using TaskEntity = ToDo.Models.Task;


namespace ToDo.ViewModels
{
    public class CriticalTaskViewModel : ViewModelBase
    {
        private readonly ITaskService _taskService;
        private ObservableCollection<TaskEntity> _tasks;
        private ICollectionView _tasksView;

        public ObservableCollection<TaskEntity> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged();
                TasksView = CollectionViewSource.GetDefaultView(_tasks);
            }
        }

        public ICollectionView TasksView
        {
            get => _tasksView;
            set
            {
                _tasksView = value;
                OnPropertyChanged();
            }
        }

        public ICommand FilterHighPriorityCommand { get; }

        public CriticalTaskViewModel(ITaskService taskService)
        {
            _taskService = taskService;
            FilterHighPriorityCommand = new RelayCommand(FilterHighPriorityTasks);
            LoadTasks();
        }

        private async void LoadTasks()
        {
            try
            {
                var allTasks = await _taskService.GetAllTasksAsync();
                // Chỉ lấy những task có TaskImportance là Critical
                var criticalTasks = allTasks.Where(t => t.TaskImportance == TaskImportance.Critical).ToList();
                Tasks = new ObservableCollection<TaskEntity>(criticalTasks);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần
                System.Windows.MessageBox.Show($"Error loading tasks: {ex.Message}");
            }
        }

        private void FilterHighPriorityTasks()
        {
            try
            {
                // Sắp xếp các task theo thời gian hết hạn (DueDate)
                var sortedTasks = Tasks.OrderBy(t => t.DueDate).ToList();
                Tasks = new ObservableCollection<TaskEntity>(sortedTasks);
                System.Windows.MessageBox.Show("Tasks have been sorted by due date.", "Success");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error sorting tasks: {ex.Message}");
            }
        }
    }
}