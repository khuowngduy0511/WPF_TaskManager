using System.Collections.ObjectModel;
using System.Windows.Input;
using ToDo.Models;
using ToDo.Services;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo.ViewModels
{
    public class DeleteTaskViewModel : ViewModelBase
    {
        private readonly ITaskService _taskService;
        private ObservableCollection<TaskItemViewModel> _tasks;

        public ObservableCollection<TaskItemViewModel> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged();
            }
        }

        public ICommand DeleteCommand { get; }
        public ICommand CancelCommand { get; }

        public DeleteTaskViewModel(ITaskService taskService)
        {
            _taskService = taskService;
            DeleteCommand = new RelayCommand(ExecuteDelete, CanExecuteDelete);
            CancelCommand = new RelayCommand(ExecuteCancel);
            LoadTasks();
        }

        private async void LoadTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            Tasks = new ObservableCollection<TaskItemViewModel>(
                tasks.Select(t => new TaskItemViewModel
                {
                    Id = t.id,
                    Name = t.Title,
                    IsSelected = false
                })
            );
        }

        private bool CanExecuteDelete() => Tasks?.Any(t => t.IsSelected) == true;

        private async void ExecuteDelete()
        {
            var selectedTasks = Tasks.Where(t => t.IsSelected).ToList();
            foreach (var task in selectedTasks)
            {
                await _taskService.DeleteTaskAsync(task.Id);
                Tasks.Remove(task);
            }
        }

        private void ExecuteCancel()
        {
            // Implement close window logic
        }
    }

    public class TaskItemViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
    }
}