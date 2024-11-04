using System.Windows;
using ToDo.ViewModels;

namespace ToDo.Views
{
    public partial class DeleteTaskWindow : Window
    {
        public DeleteTaskWindow(DeleteTaskViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is DeleteTaskViewModel viewModel)
            {
                viewModel.DeleteCommand.Execute(null);
                Close();
            }
        }
    }
}