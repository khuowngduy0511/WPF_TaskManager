using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Models
{
    public class Task
    {
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsComplete { get; set; }
        public TimeSpan Timer { get; set; }
        public TaskState TaskState { get; set; }
        public TaskCategory TaskCategory { get; set; }
        public TaskImportance TaskImportance { get; set; }
    } 

    public enum TaskState
    {
        InProgress,
        Complete,
        NotStarted,
        Late,
        Archived,
        Deleted 
    }

    public enum TaskCategory
    { 
        Work,
        Personal,
        Home,
        HealthWelbeing,
        Finance,
        Shopping,
        SocialFamily, 
        Education,
        Travel,
        Errands,
        HobbiesLeisure,
        VolunteeringCommunity,
        BirthdaysAnniversaries,
        Projects,
        LongTermGoals
    }

    public enum TaskImportance
    {
        Low,
        Medium,
        High,
        Critical
    }

}
    