using System;
using System.Windows;
using System.Windows.Controls;

namespace ToDo.Views
{
    public partial class CalendarView : Window
    {
        public CalendarView()
        {
            InitializeComponent();
            ToggleCalendarButton.Checked += ToggleCalendarButton_Checked;
            ToggleCalendarButton.Unchecked += ToggleCalendarButton_Unchecked;
            MainCalendar.SelectedDatesChanged += OnCalendarDateSelected;

            // Set the initial month and year
            UpdateMonthYearText();
        }

        private void ToggleCalendarButton_Checked(object sender, RoutedEventArgs e)
        {
            MainCalendar.Visibility = Visibility.Visible; // Show calendar
            ToggleCalendarButton.Content = "▲"; // Change button to up arrow
        }

        private void ToggleCalendarButton_Unchecked(object sender, RoutedEventArgs e)
        {
            MainCalendar.Visibility = Visibility.Collapsed; // Hide calendar
            ToggleCalendarButton.Content = "▼"; // Change button to down arrow
            WeekDisplay.Visibility = Visibility.Collapsed; // Hide week display when calendar is hidden
        }

        private void UpdateMonthYearText()
        {
            var now = DateTime.Now;
            MonthYearTextBlock.Text = now.ToString("MMMM yyyy"); // Format: October 2024
        }

        private void OnCalendarDateSelected(object sender, SelectionChangedEventArgs e)
        {
            if (MainCalendar.SelectedDate.HasValue)
            {
                DateTime selectedDate = MainCalendar.SelectedDate.Value;

                // Update the MonthYearTextBlock with the selected month and year
                MonthYearTextBlock.Text = selectedDate.ToString("MMMM yyyy");

                // Display the week based on the selected date
                DisplayWeek(selectedDate);

                // Hide the calendar and change the toggle button back to down arrow
                MainCalendar.Visibility = Visibility.Collapsed;
                ToggleCalendarButton.IsChecked = false;
                ToggleCalendarButton.Content = "▼";
            }
        }

        private void DisplayWeek(DateTime selectedDate)
        {
            WeekDisplay.Children.Clear(); // Clear previous week display

            // Calculate the start of the week (Sunday)
            DateTime startOfWeek = selectedDate.Date.AddDays(-(int)selectedDate.DayOfWeek);

            string[] dayNames = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

            // Display each day of the week
            for (int i = 0; i < 7; i++)
            {
                DateTime date = startOfWeek.AddDays(i);

                // Day header (e.g., Monday)
                TextBlock dayHeader = new TextBlock
                {
                    Text = dayNames[i],
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetRow(dayHeader, 0); // Set to the first row (for day names)
                Grid.SetColumn(dayHeader, i);
                WeekDisplay.Children.Add(dayHeader);

                // Date display (e.g., Oct 01)
                TextBlock dateTextBlock = new TextBlock
                {
                    Text = date.ToString("MMM dd"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetRow(dateTextBlock, 1); // Set to the second row (for dates)
                Grid.SetColumn(dateTextBlock, i);
                WeekDisplay.Children.Add(dateTextBlock);
            }

            WeekDisplay.Visibility = Visibility.Visible; // Show the week display
        }
    }
}
