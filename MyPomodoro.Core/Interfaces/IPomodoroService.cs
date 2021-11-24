using System;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using Humanizer;
using MyPomodoro.Core.Entities;
using ShellProgressBar;

namespace MyPomodoro.Core.Interfaces
{
    public interface IPomodoroService
    {
        Task<Pomodoro> RunAsync(PomodoroType pomodoroType, SoundPlayer typewriter, bool playSound);
    }

    public class PomodoroService : IPomodoroService
    {
        private bool _keepRunning = true;

        public Task<Pomodoro> RunAsync(PomodoroType pomodoroType, SoundPlayer typewriter, bool playSound)
        {
            Console.WriteLine("Some comment?");
            var comment = Console.ReadLine();

            TimeOnly beginTime;
            const int totalTicks = 10;
            
            var option = new ProgressBarOptions
            {
                ForegroundColor = ConsoleColor.Yellow,
                ForegroundColorDone = ConsoleColor.DarkGreen,
                BackgroundColor = ConsoleColor.DarkGray,
                BackgroundCharacter = '\u2593'
            };

            var pomodoro = new Pomodoro
            {
                Type = pomodoroType,
                Comment = string.IsNullOrEmpty(comment) ? null : comment,
                StartTime = TimeOnly.FromDateTime(DateTime.Now),
                EndTime = null,
                PomodoroDate = DateTime.Now,
                IsSuccessful = false
            };

            var timePlus = pomodoro.StartTime.Value.Add(TimeOnly.Parse("00:00:10").ToTimeSpan());
            
            if(playSound)
                typewriter.PlayLooping();

            using var pbar = new ProgressBar(totalTicks, pomodoro.Type.ToString().Humanize(LetterCasing.Title), option);

            TimeOnly tickTime = beginTime;
            var isSuccessful = false;
            
            while (_keepRunning)
            {
                var currentTime = TimeOnly.FromDateTime(DateTime.Now);

                if (tickTime.Second < currentTime.Second)
                {
                    pbar.Tick();
                    tickTime = currentTime;
                }

                if (timePlus < currentTime)
                {
                    _keepRunning = false;
                    isSuccessful = true;
                }
            }

            var pomodoroUpdated = pomodoro with
            {
                EndTime = TimeOnly.FromDateTime(DateTime.Now),
                IsSuccessful = isSuccessful
            };

            typewriter.Stop();
            typewriter.Dispose();

            return Task.FromResult(pomodoroUpdated);
        }

        public void StopLoop()
        {
            _keepRunning = false;
        }
    }
}