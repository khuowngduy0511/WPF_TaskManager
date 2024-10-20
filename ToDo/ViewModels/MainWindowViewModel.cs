using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDo.Views;

namespace ToDo.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand IOpenNewWindowCommand => new RelayCommand(OpenNewWindow);
        public ICommand IOpenSearchWindowCommand => new RelayCommand(OpenSearchWindow);

        private void OpenNewWindow()
        {
            NewTaskWindow  newTaskWindow= new NewTaskWindow();
            newTaskWindow.Show();
        }

        private void OpenSearchWindow()
        {
            // Mở cửa sổ tìm kiếm
            SearchWindow searchWindow = new SearchWindow();
            searchWindow.ShowDialog(); // Hoặc Show() nếu bạn muốn cửa sổ không chặn
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }   
}
