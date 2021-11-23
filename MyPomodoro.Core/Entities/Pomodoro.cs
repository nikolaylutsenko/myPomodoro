using System;
using System.ComponentModel.DataAnnotations;

namespace MyPomodoro.Core.Entities
{
    public class Pomodoro
    {
        [Key]
        public int Id { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public bool IsSuccessful { get; set; }
        public PomodoroType Type { get; set; }
        public string? Comment { get; set; }
        public DateTime PomodoroDate { get; set; }
    }
}