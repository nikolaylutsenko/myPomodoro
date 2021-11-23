using System;
using Microsoft.EntityFrameworkCore;
using MyPomodoro.Core.Entities;
using SQLite.CodeFirst;

namespace MyPomodoro.Dal
{
    public class AppContext : DbContext
    {
        public DbSet<Pomodoro> Pomodoros { get; set; }
        public string DbPath { get; private set; }

        public AppContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}Database.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
            
            base.OnConfiguring(optionsBuilder);
        }
    }
}