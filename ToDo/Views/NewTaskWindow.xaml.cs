﻿using System;
using System.Windows;
using ToDo.Models; 
using ToDo.Services;
using ToDo.ViewModels;
using TaskEntity = ToDo.Models.Task; // Đặt alias cho Task trong model
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace ToDo.Views
{
    public partial class NewTaskWindow : Window
    {
        private readonly ITaskService _taskService;
        private readonly MainWindowViewModel _mainViewModel;
        private ITaskService taskService;

        public event EventHandler<TaskEntity> TaskCreated;

        public NewTaskWindow(ITaskService taskService, MainWindowViewModel mainViewModel)
        {
            InitializeComponent();
            _taskService = taskService;
            _mainViewModel = mainViewModel;
            InitializeComboBoxes();
        }

        public NewTaskWindow(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        private void InitializeComboBoxes()
        {
            TaskStateComboBox.ItemsSource = Enum.GetValues(typeof(TaskState));
            TaskCategoryComboBox.ItemsSource = Enum.GetValues(typeof(TaskCategory));
            TaskImportanceComboBox.ItemsSource = Enum.GetValues(typeof(TaskImportance));
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newTask = new TaskEntity
            {
                Title = TaskTitleTextBox.Text,
                Description = DescriptionTextBox.Text,
                StartDate = StartDatePicker.SelectedDate ?? DateTime.Now,
                DueDate = DueDatePicker.SelectedDate ?? DateTime.Now,
                TaskState = (TaskState)(TaskStateComboBox.SelectedItem ?? TaskState.NotStarted),
                TaskCategory = (TaskCategory)(TaskCategoryComboBox.SelectedItem ?? TaskCategory.Personal),
                TaskImportance = (TaskImportance)(TaskImportanceComboBox.SelectedItem ?? TaskImportance.Medium)
            };

            try
            {
                var addedTask = await _taskService.CreateTaskAsync(newTask);
                if (addedTask != null)
                {
                    OnTaskCreated(addedTask);
                    ClearInputFields();

                    // Gọi trực tiếp phương thức LoadTasksAsync của MainWindowViewModel
                    await _mainViewModel.LoadTasksAsync();

                    MessageBox.Show("Task added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else
                {
                    MessageBox.Show("Failed to add task.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected virtual void OnTaskCreated(TaskEntity task)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                TaskCreated?.Invoke(this, task);
            });
        }
        private void ClearInputFields()
        {
            TaskTitleTextBox.Clear();
            DescriptionTextBox.Clear();
            StartDatePicker.SelectedDate = null;
            DueDatePicker.SelectedDate = null;
            TaskStateComboBox.SelectedItem = null;
            TaskCategoryComboBox.SelectedItem = null;
            TaskImportanceComboBox.SelectedItem = null;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
       
            Close();
        }

        public TaskEntity Task { get; private set; }
    }
}