﻿using System;
using System.Collections.ObjectModel; 
using System.ComponentModel;
using System.Windows.Input;
using ToDo.Views;

namespace ToDo.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

       
        private TaskModel _selectedTask;
        public TaskModel SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }


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

        // ObservableCollection to hold the list of tasks
        public ObservableCollection<TaskModel> Tasks { get; set; }

        // Constructor to initialize the collection and commands
        public MainWindowViewModel()
        {
            Tasks = new ObservableCollection<TaskModel>();
            LoadTasksCommand = new RelayCommand(LoadTasks);  // Command to populate the task list
        }

        // Method to load tasks into the ObservableCollection
        private void LoadTasks()
        {
            // Simulate loading tasks
            Tasks.Clear(); // Clear the existing tasks before loading new ones
            Tasks.Add(new TaskModel { Title = "Task 1", Description = "Task 1 description", DueDate = DateTime.Now.AddDays(1), Importance = "High" });
            Tasks.Add(new TaskModel { Title = "Task 2", Description = "Task 2 description", DueDate = DateTime.Now.AddDays(3), Importance = "Medium" });
            Tasks.Add(new TaskModel { Title = "Task 3", Description = "Task 3 description", DueDate = DateTime.Now.AddDays(5), Importance = "Low" });

            // Notify the UI that the tasks collection has been updated
            OnPropertyChanged(nameof(Tasks));
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

    // Task model to represent individual tasks
    public class TaskModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Importance { get; set; }
    }
}