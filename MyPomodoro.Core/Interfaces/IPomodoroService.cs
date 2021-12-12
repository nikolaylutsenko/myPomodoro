using Autofac;
using Humanizer;
using MyPomodoro.Core.Entities;
using MyPomodoro.Core.Settings;
using ShellProgressBar;

namespace MyPomodoro.Core.Interfaces;

public interface IPomodoroService
{
    Task RunAsync(PomodoroType pomodoroType, IContainer container);
    void StopLoop();
}

public class PomodoroService : IPomodoroService
{
    private bool _keepRunning = true;
    private readonly AppSettings _settings;
    
    

    public PomodoroService()
    {
        // read settings file
        _settings = SettingsService.GetSettings().Result;
    }

    public async Task RunAsync(PomodoroType pomodoroType, IContainer container)
    {
        string? comment = null;
        PlaySoundService playSoundService = null;
        TimeOnly beginTime;
        // todo: moved this shit into switch statement below
        const int totalTicks = 10;
        TimeOnly tickTime = beginTime;
        var isSuccessful = false;
        
        if (pomodoroType == PomodoroType.Concentration)
        {
            Console.WriteLine("Some comment?");
            comment = Console.ReadLine();
        }

        switch (pomodoroType)
        {
            case PomodoroType.Concentration:
                playSoundService = new PlaySoundService(@"\ticking.wav");
                break;
            case PomodoroType.ShortBreak:
                // todo: sound for short break
                playSoundService = new PlaySoundService(@"\ticking.wav");
                break;
            case PomodoroType.LongBreak:
                // todo: sound for long break
                playSoundService = new PlaySoundService(@"\ticking.wav");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pomodoroType), pomodoroType, null);
        }

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

        if (_settings.PlaySound)
            await playSoundService.PlayAsync();

        using var pbar = new ProgressBar(totalTicks, pomodoro.Type.ToString().Humanize(LetterCasing.Title), option);

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
            
        // todo: play sound on finish

        var pomodoroUpdated = pomodoro with
        {
            EndTime = TimeOnly.FromDateTime(DateTime.Now),
            IsSuccessful = isSuccessful
        };

        await playSoundService.StopAsync();
        playSoundService.Dispose();

        if (_settings.StoreInDb)
        {
            await using var scope = container.BeginLifetimeScope();
            var repository = scope.Resolve<IService<Pomodoro>>();
            await repository.AddAsync(pomodoroUpdated);
        }
    }

    public void StopLoop()
    {
        _keepRunning = false;
    }
}