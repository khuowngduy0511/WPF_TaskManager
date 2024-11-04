﻿using System;
using System.Collections.ObjectModel; 
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using ToDo.Repositories;
using ToDo.Services;
using ToDo.Views;
using TaskEntity = ToDo.Models.Task;

namespace ToDo.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        
        private readonly ITaskService _taskService;
        private TaskEntity _selectedTask;
        private ObservableCollection<TaskEntity> _tasks;
        private DispatcherTimer _timer;


        public event PropertyChangedEventHandler PropertyChanged;

        // Commands for opening different windows
        public ICommand IOpenNewWindowCommand => new RelayCommand(OpenNewWindow);
        public ICommand IOpenSearchWindowCommand => new RelayCommand(OpenSearchWindow);
        public ICommand IOpenDeleteWindowCommand => new RelayCommand(OpenDeleteTaskWindow);
        public ICommand IOpenEditTaskWindowCommand => new RelayCommand(OpenEditTaskWindow);
        public ICommand iOpenCalendarCommand => new RelayCommand(OpenCalendarView);
        public ICommand iOpenTaskCompletedViewCommand => new RelayCommand(OpenTaskCompletedView);
        public ICommand IOpenCriticalWindowCommand => new RelayCommand(OpenCriticalWindow);
        public ICommand IOpenSortlWindowCommand => new RelayCommand(OpenSortWindow);

        public ICommand FilterHighPriorityCommand { get; set; }

        // Command to load tasks when the List Icon is clicked
        public ICommand LoadTasksCommand { get; set; }
        public ICommand AddTaskCommand { get; set; }
        public ICommand UpdateTaskCommand { get; set; }
        public ICommand DeleteTaskCommand { get; set; }
        public ICommand RefreshCommand { get; }
        public ICommand CompleteTaskCommand { get; }

        // Constructor to initialize the collection and commands
        public MainWindowViewModel(ITaskService taskService)
        {
            _taskService = taskService;
            Tasks = new ObservableCollection<TaskEntity>();
            BindingOperations.EnableCollectionSynchronization(Tasks, new object());
            RefreshCommand = new RelayCommand(async () => await LoadTasksAsync());
            LoadTasksCommand = new RelayCommand(async () => await LoadTasksAsync());
            AddTaskCommand = new RelayCommand(async () => await AddTaskAsync());
            UpdateTaskCommand = new RelayCommand(async () => await UpdateTaskAsync(), CanUpdateTask);
            DeleteTaskCommand = new RelayCommand(async () => await DeleteTaskAsync(), CanDeleteTask);
            CompleteTaskCommand = new RelayCommand<TaskEntity>(CompleteTask);

/*            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2) // Thay đổi thời gian nếu cần
            };
            _timer.Tick += async (sender, args) => await LoadTasksAsync();
            _timer.Start();*/

            // Load tasks khi ViewModel được tạo
            _ = LoadTasksAsync();
        }


        public ObservableCollection<TaskEntity> Tasks
        {
            get => _tasks;
            set
            {
                if (_tasks != value)
                {
                    _tasks = value;
                    OnPropertyChanged(nameof(Tasks));
                }
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

        // Method to load tasks into the ObservableCollection
        public async Task LoadTasksAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                var tasks = await _taskService.GetAllTasksAsync();
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    Tasks.Clear();
                    foreach (var task in tasks)
                    {
                        Tasks.Add(task);
                    }
                });
                Debug.WriteLine($"Loaded {tasks.Count()} tasks");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading tasks: {ex.Message}");
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    MessageBox.Show($"Error loading tasks: {ex.Message}");
                });
            }
            finally
            {
                _semaphore.Release();
            }
        }




        private async Task AddTaskAsync()
            {
                var newTaskWindow = new NewTaskWindow(_taskService);
                newTaskWindow.TaskCreated += async (sender, task) =>
                {
                    await LoadTasksAsync(); // Tự động refresh danh sách task
                
                    System.Diagnostics.Debug.WriteLine($"Task added: {task.Title}");
                };
                newTaskWindow.ShowDialog();
            }

        private async Task UpdateTaskAsync()
        {
            if (SelectedTask != null)
            {
                var editWindow = new EditTaskWindow(SelectedTask);
                if (editWindow.ShowDialog() == true)
                {
                    await _taskService.UpdateTaskAsync(SelectedTask);
                    await LoadTasksAsync();
                }
            }
        }

        private async void CompleteTask(TaskEntity task)
        {
            if (task != null)
            {
                task.IsComplete = true;
                await _taskService.UpdateTaskAsync(task);
                await LoadTasksAsync(); // Refresh the task list
            }
        }

        private async Task DeleteTaskAsync()
        {
            if (SelectedTask != null)
            {
                var result = MessageBox.Show("Are you sure you want to delete this task?",
                    "Confirm Delete", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    await _taskService.DeleteTaskAsync(SelectedTask.id);
                    await LoadTasksAsync();
                }
            }
        }

        private bool CanUpdateTask()
        {
            return SelectedTask != null;
        }

        private bool CanDeleteTask()
        {
            return SelectedTask != null;
        }

        // Methods for opening different windows
        private void OpenNewWindow()
        {
            NewTaskWindow newTaskWindow = new NewTaskWindow(_taskService, this);
            newTaskWindow.TaskCreated += async (sender, task) =>
            {
                await LoadTasksAsync();
            };
            newTaskWindow.Show();
        }

        private void OpenSearchWindow()
        {
            SearchWindow searchWindow = new SearchWindow();
            searchWindow.ShowDialog();
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

        // Helper method to notify when properties change (for data binding)
        protected virtual void  OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task RefreshTasksAsync()
        {
            await LoadTasksAsync();
        }
    }

}