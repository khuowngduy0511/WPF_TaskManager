using System.Windows;
using ToDo.ViewModels;

namespace ToDo.Views
{
    public partial class SortTask : Window
    {
        private readonly SortTaskViewModel _viewModel;

        public SortTask(SortTaskViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SortByDueDate(); // or SortByStatus() or SortByDueDate()
        }
    }
}