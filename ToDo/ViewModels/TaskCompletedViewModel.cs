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

        public ObservableCollection<TaskEntity> CompletedTasks { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public TaskCompletedViewModel(ITaskService taskService)
        {
            _taskService = taskService;
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
                // Show confirmation dialog
                var result = MessageBox.Show("Are you sure you want to restore this task?",
                                              "Confirm Restore",
                                              MessageBoxButton.YesNo,
                                              MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Show state selection dialog
                    var selectedState = ShowStateSelectionDialog(); // Call to the method that shows the dialog
                    if (selectedState.HasValue)
                    {
                        task.IsComplete = false;  // Mark the task as not completed
                        task.TaskState = selectedState.Value; // Set the task state to user-selected state
                        await _taskService.UpdateTaskAsync(task); // Update the task in the database
                        CompletedTasks.Remove(task);  // Remove the task from the completed list

                        // Display success message
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
