using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ToDo.Data;
using ToDo.Repositories;
using ToDo.Services;
using ToDo.ViewModels;
using ToDo.Views;

namespace ToDo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
            Resources.Add("BoolToYesNoConverter", new BoolToYesNoConverter());
            Resources.Add("NullToBooleanConverter", new NullToBooleanConverter());
        }

        private void ConfigureServices(ServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Đăng ký DbContext với cấu hình kết nối
            services.AddDbContext<TodoDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Scoped);

            services.AddTransient<SortTaskViewModel>();
            services.AddTransient<SortTask>();


            services.AddTransient<CriticalTaskViewModel>();


            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<MainWindowViewModel>();
            services.AddSingleton<MainWindowViewModel>(sp =>
            {
                var taskService = sp.GetRequiredService<ITaskService>();
                var sortTaskViewModelFactory = new Func<SortTaskViewModel>(() => sp.GetRequiredService<SortTaskViewModel>());
                var criticalWindowFactory = new Func<CriticalWindow>(() => sp.GetRequiredService<CriticalWindow>());
                return new MainWindowViewModel(taskService, sortTaskViewModelFactory, criticalWindowFactory);
            });
            services.AddSingleton<MainWindow>();
            services.AddTransient<NewTaskWindow>(provider =>
    new NewTaskWindow(
        provider.GetRequiredService<ITaskService>(),
        provider.GetRequiredService<MainWindowViewModel>()
                )
            );
            services.AddTransient<CriticalWindow>();

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            var viewModel = serviceProvider.GetRequiredService<MainWindowViewModel>();
            System.Diagnostics.Debug.WriteLine($"MainWindow DataContext: {mainWindow.DataContext}");
            System.Diagnostics.Debug.WriteLine($"ViewModel: {viewModel}");
            mainWindow.Show();
        }
    }
}