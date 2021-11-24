using System;

namespace MyPomodoro.Core
{
    public static class IntToTimespan
    {
        public static TimeSpan Convert(int minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }
    }
}