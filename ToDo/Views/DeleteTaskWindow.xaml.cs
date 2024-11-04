using System.Windows;
using ToDo.ViewModels;

namespace ToDo.Views
{
    public partial class DeleteTaskWindow : Window
    {
        private readonly DeleteTaskViewModel _viewModel;

        public DeleteTaskWindow(DeleteTaskViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = viewModel;
            _viewModel.CloseAction = () => Close();
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