using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDo.ViewModels;
using ToDo.Views;
namespace ToDo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenSortTask_Click(object sender, RoutedEventArgs e)
        {
            // Tạo cửa sổ SortTask và hiển thị nó
            var sortTaskWindow = new SortTask
            {
                Owner = this, // Thiết lập MainWindow là chủ sở hữu
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            sortTaskWindow.Show();
        }

        private void OpenCriticalWindow_Click(object sender, RoutedEventArgs e)
        {
            // Tạo và mở cửa sổ CriticalWindow
            var criticalWindow = new Views.CriticalWindow
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            criticalWindow.Show();
        }


    }
}
