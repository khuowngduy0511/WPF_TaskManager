using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToDo.Services; // Adjust the namespace as necessary
using ToDo.ViewModels;

namespace ToDo.Views
{
    public partial class CalendarView : Window
    {
        public CalendarView(ITaskService taskService)
        {
            InitializeComponent();
            DataContext = new CalendarViewModel(taskService);
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is CalendarDayModel dayModel)
            {
                // Execute the command to show tasks for the date clicked
                dayModel.ShowTasksCommand.Execute(null);
            }
        }
    }
}
