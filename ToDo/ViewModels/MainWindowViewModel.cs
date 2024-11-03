﻿using System;
using System.Collections.ObjectModel; 
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ToDo.Repositories;
using ToDo.Services;
using ToDo.Views;
using TaskEntity = ToDo.Models.Task;

namespace ToDo.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly ITaskService _taskService;
        private TaskEntity _selectedTask;


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


        // Constructor to initialize the collection and commands
        public MainWindowViewModel(ITaskService taskService)
        {
            _taskService = taskService;
            Tasks = new ObservableCollection<TaskEntity>();
            LoadTasksCommand = new RelayCommand(async () => await LoadTasksAsync());
            AddTaskCommand = new RelayCommand(async () => await AddTaskAsync());
            UpdateTaskCommand = new RelayCommand(async () => await UpdateTaskAsync(), CanUpdateTask);
            DeleteTaskCommand = new RelayCommand(async () => await DeleteTaskAsync(), CanDeleteTask);

            // Load tasks when ViewModel is created
            _ = LoadTasksAsync();
        }
        public ObservableCollection<TaskEntity> Tasks { get; set; }

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
        private async Task LoadTasksAsync()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            Tasks.Clear();
            foreach (var task in tasks)
            {
                Tasks.Add(task);
            }
        }

        private async Task AddTaskAsync()
        {
            var newTaskWindow = new NewTaskWindow();
            if (newTaskWindow.ShowDialog() == true)
            {
                var newTask = newTaskWindow.Task;
                await _taskService.CreateTaskAsync(newTask);
                await LoadTasksAsync();
            }
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
            NewTaskWindow newTaskWindow = new NewTaskWindow();
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
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }

}