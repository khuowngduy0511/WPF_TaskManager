using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using ToDo.Views;
using ToDo.Models;
using System;
namespace ToDo.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Task> _tasks;
        public ObservableCollection<Task> Tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                OnPropertyChanged(nameof(Tasks));
            }
        }
        private Task selectedTask;
        public Task SelectedTask
        {
            get { return selectedTask; }
            set { selectedTask = value; OnPropertyChanged(nameof(SelectedTask)); }
        }
        public ICommand iOpenNewWindowCommand => new RelayCommand(OpenNewWindow);

        public ICommand OpenEditTaskWindowCommand => new RelayCommand(OpenEditTaskWindow);


        public ICommand IOpenNewWindowCommand => new RelayCommand(OpenNewWindow);
        public ICommand IOpenSearchWindowCommand => new RelayCommand(OpenSearchWindow);
        public ICommand IOpenDeleteWindowCommand => new RelayCommand(OpenDeleteTaskWindow);
        public ICommand IOpenEditTaskWindowCommand => new RelayCommand(OpenEditTaskWindow);
        private void OpenNewWindow()
        {
            NewTaskWindow newTaskWindow = new NewTaskWindow();
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
