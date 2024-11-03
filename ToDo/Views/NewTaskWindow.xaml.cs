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
using ToDo.Models;
using Task = ToDo.Models.Task;

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

        public Task Task { get; private set; }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Task = new Task
            {
                Title = TaskTitleTextBox.Text,
                Description = DescriptionTextBox.Text,
                DueDate = DueDatePicker.SelectedDate ?? DateTime.Now,
                StartDate = DateTime.Now,
                TaskState = TaskState.NotStarted
            };

            DialogResult = true;
            Close();
        }

    }
}
