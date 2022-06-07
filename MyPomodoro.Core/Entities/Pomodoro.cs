using System;
using System.ComponentModel.DataAnnotations;

namespace MyPomodoro.Core.Entities;

public record Pomodoro
{
    [Key]
    public int Id { get; init; }
    public TimeOnly? StartTime { get; init; }
    public TimeOnly? EndTime { get; init; }
    public bool IsSuccessful { get; init; }
    public PomodoroType Type { get; init; }
    public string? Comment { get; init; }
    public DateTime PomodoroDate { get; init; }
}