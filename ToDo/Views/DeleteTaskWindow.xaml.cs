using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ToDo.Models;

namespace ToDo.Views
{
    /// <summary>
    /// Interaction logic for DeleteTaskWindow.xaml
    /// </summary>
    public partial class DeleteTaskWindow : Window
    {
        public ObservableCollection<TaskItem> Tasks { get; set; }
        public DeleteTaskWindow() 
        {
            InitializeComponent();
        }
        public DeleteTaskWindow(ObservableCollection<TaskItem> tasks)
        {
            Tasks = tasks;
            TaskListBox.ItemsSource = Tasks;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Collect tasks that are selected (IsSelected == true)
            var tasksToDelete = new ObservableCollection<TaskItem>();
            foreach (var task in Tasks)
            {
                if (task.IsSelected)
                {
                    tasksToDelete.Add(task);
                }
            }

            // Remove selected tasks from the list
            foreach (var task in tasksToDelete)
            {
                Tasks.Remove(task);
            }

            // Close the window
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the window without any changes
            this.DialogResult = false;
            this.Close();
        }
    }

    // TaskItem class represents each task with a name and selection status
    public class TaskItem
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}