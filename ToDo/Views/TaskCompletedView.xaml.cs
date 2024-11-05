using System.Windows;
using ToDo.Services;
using ToDo.ViewModels;

namespace ToDo.Views
{
    public partial class TaskCompletedView : Window
    {
        public TaskCompletedView(TaskCompletedViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
