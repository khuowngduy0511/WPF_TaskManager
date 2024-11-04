using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskEntity = ToDo.Models.Task;

namespace ToDo.Data
{
    public class TodoDbContext : DbContext
    {
        public DbSet<TaskEntity> Tasks { get; set; }
        public TodoDbContext() { }

        public TodoDbContext(DbContextOptions<TodoDbContext> options)
    : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskEntity>(entity =>
            {
                entity.HasKey(e => e.id);
            });
        }

    }
}
