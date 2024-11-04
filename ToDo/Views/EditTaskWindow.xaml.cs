using System.Windows;
using ToDo.ViewModels;

namespace ToDo.Views
{
    public partial class EditTaskWindow : Window
    {
        private readonly EditTaskViewModel _viewModel;

        public EditTaskWindow(EditTaskViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            _viewModel.CloseAction = () => Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}