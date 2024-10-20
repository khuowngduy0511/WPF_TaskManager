using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ToDo.Views
{
    /// <summary>
    /// Interaction logic for EditTaskWindow.xaml
    /// </summary>
    public partial class EditTaskWindow : Window
    {
        public EditTaskWindow()
        {
            InitializeComponent();
        }
        private List<Task> tasks; // Assuming you have a Task class defined
        private Task selectedTask;

        public EditTaskWindow(List<Task> tasks)
        {
            this.tasks = tasks;
            LoadTasks();
        }
        public class Task
        {
            public string Title { get; set; }
            public DateTime? DueDate { get; set; } // Nullable DateTime
            public string Description { get; set; }

            public override string ToString()
            {
                return Title; // Return the Title for ComboBox display
            }
        }
        private void LoadTasks()
        {
            TaskComboBox.ItemsSource = tasks; // Assuming Task has a meaningful ToString() method
        }

        private void TaskComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TaskComboBox.SelectedItem is Task task)
            {
                selectedTask = task;
                TaskTitleTextBox.Text = selectedTask.Title;
                DueDatePicker.SelectedDate = selectedTask.DueDate;
                DescriptionTextBox.Text = selectedTask.Description;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Save changes to the selected task
            if (selectedTask != null)
            {
                selectedTask.Title = TaskTitleTextBox.Text;
                selectedTask.DueDate = DueDatePicker.SelectedDate;
                selectedTask.Description = DescriptionTextBox.Text;

                // Add logic to save updated task to your data store
            }

            this.Close();
        }
    }
}
