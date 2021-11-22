using Microsoft.EntityFrameworkCore;
using MyPomodoro.Core.Entities;
using SQLite.CodeFirst;

namespace MyPomodoro.Dal
{
    public class AppContext : DbContext
    {
        public DbSet<Pomodoro> Pomodoros { get; set; }

        public AppContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var contextInitializer = new SqliteCreateDatabaseIfNotExists<AppContext>(modelBuilder);
            optionsBuilder.UseSqlite("Filename=Database.db");
        }
    }
}