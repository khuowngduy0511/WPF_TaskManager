using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ToDo.Services; // Update with your actual namespace for ITaskService
using ToDo.Views;
using TaskEntity = ToDo.Models.Task;

namespace ToDo.ViewModels
{
    public class CalendarDayModel
    {
        public string DisplayDay { get; set; }
        public DateTime? Date { get; set; }
        public bool HasTasks => TaskCount > 0;
        public int TaskCount { get; set; }
        public IEnumerable<int> TaskDots => Enumerable.Range(0, TaskCount);
        public ICommand ShowTasksCommand { get; set; }
    }

    public class CalendarViewModel : INotifyPropertyChanged
    {
        private DateTime _currentMonthYear;
        private readonly ITaskService _taskService;

        public DateTime CurrentMonthYear
        {
            get => _currentMonthYear;
            set
            {
                _currentMonthYear = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentMonthYearDisplay));
                UpdateCalendarDates();
            }
        }

        public string CurrentMonthYearDisplay => CurrentMonthYear.ToString("MMMM yyyy");

        public ObservableCollection<CalendarDayModel> CalendarDates { get; set; }

        public ICommand PreviousMonthCommand { get; }
        public ICommand NextMonthCommand { get; }
        public ICommand TodayCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CalendarViewModel(ITaskService taskService)
        {
            _taskService = taskService;
            _currentMonthYear = DateTime.Now;
            CalendarDates = new ObservableCollection<CalendarDayModel>();
            PreviousMonthCommand = new RelayCommand(GoToPreviousMonth);
            NextMonthCommand = new RelayCommand(GoToNextMonth);
            TodayCommand = new RelayCommand(GoToToday);

            UpdateCalendarDates();
        }

        private void GoToPreviousMonth()
        {
            CurrentMonthYear = CurrentMonthYear.AddMonths(-1);
        }

        private void GoToNextMonth()
        {
            CurrentMonthYear = CurrentMonthYear.AddMonths(1);
        }

        private void GoToToday()
        {
            CurrentMonthYear = DateTime.Now;
        }

        private void UpdateCalendarDates()
        {
            CalendarDates.Clear();

            DateTime firstDayOfMonth = new DateTime(CurrentMonthYear.Year, CurrentMonthYear.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(CurrentMonthYear.Year, CurrentMonthYear.Month);
            int startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

            var tasksForCurrentMonth = _taskService.GetTasksDueInMonth(CurrentMonthYear.Year, CurrentMonthYear.Month);
            var taskCounts = tasksForCurrentMonth.GroupBy(t => t.DueDate.Date)
                .ToDictionary(g => g.Key, g => g.Count());

            for (int i = 0; i < startDayOfWeek; i++)
            {
                CalendarDates.Add(new CalendarDayModel { DisplayDay = string.Empty, Date = null });
            }

            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(CurrentMonthYear.Year, CurrentMonthYear.Month, day);
                var dayModel = new CalendarDayModel
                {
                    DisplayDay = day.ToString(),
                    Date = date,
                    TaskCount = taskCounts.ContainsKey(date) ? taskCounts[date] : 0,
                    ShowTasksCommand = new RelayCommand(() => ShowTasksForDate(date, tasksForCurrentMonth))
                };
                CalendarDates.Add(dayModel);
            }

            while (CalendarDates.Count < 35)
            {
                CalendarDates.Add(new CalendarDayModel { DisplayDay = string.Empty, Date = null });
            }
        }

        private void ShowTasksForDate(DateTime date, IEnumerable<TaskEntity> tasksForCurrentMonth)
        {
            var tasksForDate = tasksForCurrentMonth.Where(t => t.DueDate.Date == date).ToList();

            if (tasksForDate.Any())
            {
                // Create the TaskDetailView
                var taskDetailView = new TaskDetailsView(); // Make sure this is the correct type for your Task detail view
                var taskDetailViewModel = new TaskDetailViewModel(tasksForDate); // Pass tasks for that date

                // Set the DataContext to the view model
                taskDetailView.DataContext = taskDetailViewModel;

                // Show the TaskDetailView
                taskDetailView.ShowDialog(); // Use ShowDialog to display as a modal dialog
            }
            else
            {
                MessageBox.Show("No tasks for this date.", "Tasks");
            }
        }



        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
