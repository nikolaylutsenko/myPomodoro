using Microsoft.EntityFrameworkCore;
using MyPomodoro.Core.Entities;

namespace MyPomodoro.Dal.Context;

public class AppContext : DbContext
{
    public DbSet<Pomodoro>? Pomodoros { get; set; }
    public string DbPath { get; private set; }

    public AppContext()
    {
        var path = Environment.CurrentDirectory;
        DbPath = $"{path}{Path.DirectorySeparatorChar}DbFile{Path.DirectorySeparatorChar}Database.db";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");

        base.OnConfiguring(optionsBuilder);
    }
}