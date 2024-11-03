using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaskEntity = ToDo.Models.Task;

namespace ToDo.Views
{
    /// <summary>
    /// Interaction logic for EditTaskWindow.xaml
    /// </summary>
    public partial class EditTaskWindow : Window
    {
        private TaskEntity _task;
        private Task selectedTask;

        public EditTaskWindow()
        {
            InitializeComponent();
        }

        public EditTaskWindow(TaskEntity task)
        {
            InitializeComponent();
            _task = task;
            LoadTaskData();
        }

        public Task Task { get; private set; }

        private void LoadTaskData()
        {
            if (_task != null)
            {
                TaskTitleTextBox.Text = _task.Title;
                DescriptionTextBox.Text = _task.Description;
                DueDatePicker.SelectedDate = _task.DueDate;
                // Cập nhật các thêm control khác
            }

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_task != null)
            {
                _task.Title = TaskTitleTextBox.Text;
                _task.Description = DescriptionTextBox.Text;
                _task.DueDate = DueDatePicker.SelectedDate ?? DateTime.Now;
                // Cập nhật các thuộc tính khác

                DialogResult = true;
            }
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

    }
}
