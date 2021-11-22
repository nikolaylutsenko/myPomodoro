using System;

namespace MyPomodoro.Core.Entities
{
    public class Pomodoro
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsSuccessful { get; set; }
        public PomodoroType Type { get; set; }
        public string? Comment { get; set; }
        public TimeOnly Time { get; set; }
    }
}