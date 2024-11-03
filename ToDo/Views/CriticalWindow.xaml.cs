﻿using System;
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
using System.Windows.Shapes;
using ToDo.ViewModels;

namespace ToDo.Views
{
    /// <summary>
    /// Interaction logic for CriticalWindow.xaml
    /// </summary>
    public partial class CriticalWindow : Window
    {
        public CriticalWindow()
        {
            InitializeComponent();
        }

        // Close button click event
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Filter High Priority button click event
        private void FilterHighPriorityButton_Click(object sender, RoutedEventArgs e)
        {

        }

        // Assuming Tasks is a property in the DataContext or ViewModel containing the task list
        public List<Task> Tasks { get; set; }
    }
}

