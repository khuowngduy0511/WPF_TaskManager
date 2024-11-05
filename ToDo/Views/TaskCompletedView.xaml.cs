using System.Windows;
using ToDo.Services;
using ToDo.ViewModels;

namespace ToDo.Views
{
    public partial class TaskCompletedView : Window
    {
        public TaskCompletedView(ITaskService taskService)
        {
            InitializeComponent();
            DataContext = new TaskCompletedViewModel(taskService);
        }
    }
}
