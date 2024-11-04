using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ToDo.Models;
using ToDo.Services;
using TaskEntity = ToDo.Models.Task;

namespace ToDo.ViewModels
{
    public class EditTaskViewModel : ViewModelBase
    {
        private readonly ITaskService _taskService;
        private TaskEntity _selectedTask;

        public ObservableCollection<TaskEntity> Tasks { get; private set; }
        public Array TaskStates => Enum.GetValues(typeof(TaskState));
        public Array TaskCategories => Enum.GetValues(typeof(TaskCategory));
        public Array TaskImportances => Enum.GetValues(typeof(TaskImportance));

        public TaskEntity SelectedTask
        {
            get => _selectedTask;
            set => SetProperty(ref _selectedTask, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CloseCommand { get; }
        public Action CloseAction { get; set; }

        public EditTaskViewModel(ITaskService taskService)
        {
            _taskService = taskService;
            SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
            CloseCommand = new RelayCommand(ExecuteCancel);
            LoadTasks();
        }

        private async void LoadTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            Tasks = new ObservableCollection<TaskEntity>(tasks);
            OnPropertyChanged(nameof(Tasks));
        }

        private bool CanExecuteSave() => SelectedTask != null;

        private async void ExecuteSave()
        {
            if (SelectedTask != null)
            {
                await _taskService.UpdateTaskAsync(SelectedTask);
                // Optionally, you can show a message box to inform the user that the task has been updated
            }
        }

        private void ExecuteCancel()
        {
            CloseAction?.Invoke();
        }
    }
}