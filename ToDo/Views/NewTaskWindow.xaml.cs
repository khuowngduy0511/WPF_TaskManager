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
    /// Interaction logic for NewTaskWindow.xaml
    /// </summary>
    public partial class NewTaskWindow : Window
    {
        public NewTaskWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Logic to add the task goes here
            string taskTitle = TaskTitleTextBox.Text;
            DateTime? dueDate = DueDatePicker.SelectedDate;
            string description = DescriptionTextBox.Text;

            // Implement your logic for adding the task
            MessageBox.Show($"Task Added: {taskTitle}\nDue Date: {dueDate}\nDescription: {description}");
        }

    }
}
