using System;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using TaskEntity = ToDo.Models.Task;
using ToDo.Services;
using ToDo.Views;
using System.Windows;
using ToDo.Models;
using Task = System.Threading.Tasks.Task;

namespace ToDo.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly ITaskService _taskService;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private ObservableCollection<TaskEntity> _tasks;
        private TaskEntity _selectedTask;
        private string _searchText;
        private ICollectionView _tasksView;

        // Commands
        public ICommand IOpenNewWindowCommand => new RelayCommand(OpenNewWindow);
        public ICommand IOpenDeleteWindowCommand => new RelayCommand(OpenDeleteTaskWindow);
        public ICommand IOpenEditTaskWindowCommand => new RelayCommand(OpenEditTaskWindow);
        public ICommand iOpenCalendarCommand => new RelayCommand(OpenCalendarView);
        public ICommand iOpenTaskCompletedViewCommand => new RelayCommand(OpenTaskCompletedView);
        public ICommand IOpenCriticalWindowCommand => new RelayCommand(OpenCriticalWindow);
        public ICommand IOpenSortlWindowCommand => new RelayCommand(OpenSortWindow);
        public ICommand LoadTasksCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand CompleteTaskCommand { get; private set; }




        // Properties
        public ObservableCollection<TaskEntity> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                OnPropertyChanged(nameof(Tasks));
                TasksView = CollectionViewSource.GetDefaultView(_tasks);
                if (TasksView != null)
                {
                    TasksView.Filter = TaskFilter;
                }
            }
        }

        public ICollectionView TasksView
        {
            get => _tasksView;
            set
            {
                _tasksView = value;
                OnPropertyChanged(nameof(TasksView));
            }
        }

        public TaskEntity SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                TasksView?.Refresh();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Constructor
        public MainWindowViewModel(ITaskService taskService)
        {
            _taskService = taskService;
            _tasks = new ObservableCollection<TaskEntity>();
            TasksView = CollectionViewSource.GetDefaultView(_tasks);
            if (TasksView != null)
            {
                TasksView.Filter = TaskFilter;
            }

            LoadTasksCommand = new RelayCommand(async () => await LoadTasksAsync());
            SearchCommand = new RelayCommand(ExecuteSearch);
            CompleteTaskCommand = new RelayCommand(ExecuteCompleteTask, CanExecuteCompleteTask);


            // Initial load of tasks
            _ = LoadTasksAsync();
        }


  

        public async Task RefreshTasksAsync()
        {
            await LoadTasksAsync();
        }
        // Methods
        private bool TaskFilter(object item)
        {
            if (string.IsNullOrEmpty(SearchText))
                return true;
            if (item is TaskEntity task)
            {
                return task.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private void ExecuteSearch()
        {
            TasksView?.Refresh();
        }

        public async Task LoadTasksAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                var tasks = await _taskService.GetAllTasksAsync();
                Tasks = new ObservableCollection<TaskEntity>(tasks);
            }
            catch (Exception ex)
            {
                // Handle error...
                MessageBox.Show($"Error loading tasks: {ex.Message}");
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private bool CanExecuteCompleteTask()
        {
            return SelectedTask != null && !SelectedTask.IsComplete;
        }

        private async void ExecuteCompleteTask()
        {
            System.Diagnostics.Debug.WriteLine("ExecuteCompleteTask called");

            try
            {
                if (SelectedTask == null)
                {
                    MessageBox.Show("No task selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var result = MessageBox.Show(
                    $"Are you sure you want to complete the task '{SelectedTask.Title}'?",
                    "Confirm Complete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    SelectedTask.IsComplete = true;
                    SelectedTask.TaskState = TaskState.Complete;

                    await _taskService.UpdateTaskAsync(SelectedTask);

                    // Remove from list and update UI
                    Tasks.Remove(SelectedTask);
                    MessageBox.Show("Task completed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Clear selection
                    SelectedTask = null;

                    // Refresh the view
                    OnPropertyChanged(nameof(Tasks));
                    TasksView?.Refresh();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ExecuteCompleteTask: {ex.Message}");
                MessageBox.Show($"Error completing task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        public void AddNewTask(TaskEntity newTask)
        {
            Tasks.Add(newTask);
            TasksView?.Refresh();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Existing window opening methods
        private void OpenNewWindow()
        {
            NewTaskWindow newTaskWindow = new NewTaskWindow(_taskService, this);
            newTaskWindow.Show();
        }



        private void OpenDeleteTaskWindow()
        {
            var deleteTaskViewModel = new DeleteTaskViewModel(_taskService);
            var deleteTaskWindow = new DeleteTaskWindow(deleteTaskViewModel);
            deleteTaskWindow.ShowDialog();

            // Refresh tasks after deletion
            _ = LoadTasksAsync();
        }

        private void OpenEditTaskWindow()
        {
            EditTaskWindow editTaskWindow = new EditTaskWindow();
            editTaskWindow.ShowDialog();
        }

        private void OpenCalendarView()
        {
            CalendarView calendarView = new CalendarView();
            calendarView.Show();
        }

        private void OpenTaskCompletedView()
        {
            TaskCompletedView taskCompletedView = new TaskCompletedView();
            taskCompletedView.Show();
        }

        private void OpenCriticalWindow()
        {
            CriticalWindow criticalWindow = new CriticalWindow();
            criticalWindow.Show();
        }

        private void OpenSortWindow()
        {
            SortTask sortWindow = new SortTask();
            sortWindow.Show();
        }
    }
}