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
        public ICommand CompleteTaskCommand { get; }

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

        private bool CanExecuteCompleteTask()
        {
            return SelectedTask != null && !SelectedTask.IsComplete;
        }

        private async void ExecuteCompleteTask()
        {
            if (SelectedTask != null)
            {
                try
                {
                    SelectedTask.IsComplete = true;
                    await _taskService.UpdateTaskAsync(SelectedTask);
                    Tasks.Remove(SelectedTask);
                    SelectedTask = null;
                    OnPropertyChanged(nameof(SelectedTask));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error completing task: {ex.Message}");
                }
            }
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
            DeleteTaskWindow deleteTaskWindow = new DeleteTaskWindow();
            deleteTaskWindow.ShowDialog();
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