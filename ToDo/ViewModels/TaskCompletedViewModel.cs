using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ToDo.Models;
using ToDo.Services;
using ToDo.Views;
using TaskEntity = ToDo.Models.Task;

namespace ToDo.ViewModels
{
    public class TaskCompletedViewModel : INotifyPropertyChanged
    {
        private readonly ITaskService _taskService;
        private readonly MainWindowViewModel _mainViewModel;


        public ObservableCollection<TaskEntity> CompletedTasks { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public TaskCompletedViewModel(ITaskService taskService, MainWindowViewModel mainViewModel)
        {
            _taskService = taskService;
            _mainViewModel = mainViewModel;
            CompletedTasks = new ObservableCollection<TaskEntity>();
            LoadCompletedTasksAsync();
            RestoreTaskCommand = new RelayCommand<TaskEntity>(RestoreTask);
        }

        public ICommand RestoreTaskCommand { get; }

        private async void LoadCompletedTasksAsync()
        {
            var tasks = await _taskService.GetCompletedTasksAsync();
            CompletedTasks.Clear();
            foreach (var task in tasks)
            {
                CompletedTasks.Add(task);
            }
        }

        private async void RestoreTask(TaskEntity task)
        {
            if (task != null)
            {
                var result = MessageBox.Show("Are you sure you want to restore this task?",
                                              "Confirm Restore",
                                              MessageBoxButton.YesNo,
                                              MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var selectedState = ShowStateSelectionDialog();
                    if (selectedState.HasValue)
                    {
                        task.IsComplete = false;
                        task.TaskState = selectedState.Value;
                        await _taskService.UpdateTaskAsync(task);
                        CompletedTasks.Remove(task);

                        // Cập nhật danh sách task trong MainWindowViewModel
                        await _mainViewModel.RefreshTasksAsync();

                        MessageBox.Show("Task restored successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private TaskState? ShowStateSelectionDialog()
        {
            // Create a simple dialog window or use a MessageBox for simplicity
            var dialog = new TaskStateSelectionDialog();

            // Show the dialog and wait for user input
            if (dialog.ShowDialog() == true)
            {
                return dialog.SelectedTaskState;
            }

            return null; // User canceled the dialog
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
