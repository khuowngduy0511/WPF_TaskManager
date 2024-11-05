using System.Collections.ObjectModel;
using ToDo.Models;
using TaskEntity = ToDo.Models.Task;
namespace ToDo.ViewModels
{
    public class TaskDetailViewModel
    {
        public ObservableCollection<TaskEntity> Tasks { get; set; }

        public TaskDetailViewModel(IEnumerable<TaskEntity> tasks)
        {
            Tasks = new ObservableCollection<TaskEntity>(tasks);
        }
    }
}
