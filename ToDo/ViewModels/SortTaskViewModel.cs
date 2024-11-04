using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using ToDo.Models;
using ToDo.Services;
using TaskEntity = ToDo.Models.Task;


namespace ToDo.ViewModels
{
    public class SortTaskViewModel : ViewModelBase
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

        public SortTaskViewModel(ITaskService taskService)
        {
            _taskService = taskService;
            LoadTasks();
        }

        private async void LoadTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            Tasks = new ObservableCollection<TaskEntity>(tasks);
        }

        public void SortByTitle()
        {
            Tasks = new ObservableCollection<TaskEntity>(Tasks.OrderBy(t => t.Title));
        }

        public void SortByStatus()
        {
            Tasks = new ObservableCollection<TaskEntity>(Tasks.OrderBy(t => t.TaskState));
        }

        public void SortByDueDate()
        {
            Tasks = new ObservableCollection<TaskEntity>(Tasks.OrderBy(t => t.DueDate));
        }
    }
}