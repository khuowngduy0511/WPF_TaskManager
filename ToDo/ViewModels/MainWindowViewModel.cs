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


        public MainWindowViewModel()
        {
            // Khởi tạo danh sách task mẫu
            Tasks = new ObservableCollection<Task>
            {
                new Task { id = 1, Title = "Task 1", Description = "Description 1", DueDate = DateTime.Now.AddDays(1), IsComplete = false },
                new Task { id = 2, Title = "Task 2", Description = "Description 2", DueDate = DateTime.Now.AddDays(2), IsComplete = true }
            };
        }

        private void OpenNewWindow()
        {
            NewTaskWindow newTaskWindow = new NewTaskWindow();
            newTaskWindow.Show();
        }

        private void OpenEditTaskWindow()
        {
            EditWindow editTaskWindow = new EditWindow();
            editTaskWindow.Show();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}