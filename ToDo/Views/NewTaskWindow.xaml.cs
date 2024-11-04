using System;
using System.Windows;
using ToDo.Models; // Đảm bảo rằng bạn đã thêm namespace này
using ToDo.Services;
using TaskEntity = ToDo.Models.Task; // Đặt alias cho Task trong model

namespace ToDo.Views
{
    public partial class NewTaskWindow : Window
    {
        private readonly ITaskService _taskService;

        public NewTaskWindow(ITaskService taskService)
        {
            InitializeComponent();
            _taskService = taskService;
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
                IsComplete = IsCompleteCheckBox.IsChecked ?? false,
               
                TaskState = (TaskState)(TaskStateComboBox.SelectedItem ?? TaskState.NotStarted),
                TaskCategory = (TaskCategory)(TaskCategoryComboBox.SelectedItem ?? TaskCategory.Personal),
                TaskImportance = (TaskImportance)(TaskImportanceComboBox.SelectedItem ?? TaskImportance.Medium)
            };

            try
            {
                var addedTask = await _taskService.CreateTaskAsync(newTask);
                if (addedTask != null)
                {
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
       
            Close();
        }

        public TaskEntity Task { get; private set; }
    }
}