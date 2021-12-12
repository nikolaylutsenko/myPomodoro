using System;
using System.ComponentModel.DataAnnotations;

namespace MyPomodoro.Core.Entities
{
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

        public Pomodoro()
        {
        }
        
        public Pomodoro(TimeOnly startTime, TimeOnly endTime, PomodoroType pomodoroType, string comment, bool isSuccessful = false)
        {
            StartTime = startTime;
            EndTime = endTime;
            Type = pomodoroType;
            Comment = comment;
            IsSuccessful = isSuccessful;
            PomodoroDate = DateTime.Now;
        }
    }
}