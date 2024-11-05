using System.Windows;
using System.Windows.Controls;
using ToDo.Models;

namespace ToDo.Views
{
    public partial class TaskStateSelectionDialog : Window
    {
        public TaskState? SelectedTaskState { get; private set; }

        public TaskStateSelectionDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected state from the ComboBox
            if (StateComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                // Parse the selected state based on the Tag
                SelectedTaskState = (TaskState)int.Parse(selectedItem.Tag.ToString());
            }
            DialogResult = true; // Indicate the dialog was accepted
            Close(); // Close the dialog
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Indicate the dialog was canceled
            Close(); // Close the dialog
        }
    }
}
