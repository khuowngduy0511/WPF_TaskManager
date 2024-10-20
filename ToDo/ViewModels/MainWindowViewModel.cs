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
        public ICommand IOpenDeleteWindowCommand => new RelayCommand(OpenDeleteTaskWindow);
        public ICommand IOpenEditTaskWindowCommand => new RelayCommand(OpenEditTaskWindow);
        private void OpenNewWindow()
        {
            NewTaskWindow  newTaskWindow= new NewTaskWindow();
            newTaskWindow.Show();
        }

        private void OpenSearchWindow()
        {
            SearchWindow searchWindow = new SearchWindow();
            searchWindow.ShowDialog(); 
        }
        private void OpenDeleteTaskWindow()
        {
            DeleteTaskWindow deleteTaskWindow = new DeleteTaskWindow();
            deleteTaskWindow.ShowDialog(); 
        }
        private void OpenEditTaskWindow()
        {
            EditTaskWindow editTaskWindow = new EditTaskWindow();
            editTaskWindow.ShowDialog(); 
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }   
}
