using System.Windows;
using ToDo.ViewModels;

namespace ToDo.Views
{
    public partial class CriticalWindow : Window
    {
        private readonly CriticalTaskViewModel _viewModel;

        public CriticalWindow(CriticalTaskViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}