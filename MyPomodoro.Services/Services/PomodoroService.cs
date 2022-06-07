using Humanizer;
using MyPomodoro.Core.Entities;
using MyPomodoro.Core.Interfaces;
using ShellProgressBar;

namespace MyPomodoro.Services.Services;

public class PomodoroService : IPomodoroService
{
    private bool _abortRequested = false;
    private readonly IStorageService<Pomodoro> _service;
    private readonly ISettingsService _settings;

    public PomodoroService(IStorageService<Pomodoro> service, ISettingsService settings)
    {
        _service = service;
        _settings = settings;
    }

    public async Task RunAsync(PomodoroType pomodoroType)
    {
        string? comment = null;
        var _appSettings = await _settings.GetSettings();
        var playSoundService = new PlaySoundService($"{Path.DirectorySeparatorChar}Sounds{Path.DirectorySeparatorChar}"); ;
        long totalTicks;
        var isSuccessful = false;

        if (pomodoroType == PomodoroType.Concentration)
        {
            Console.WriteLine("Some comment?");

            comment = Console.ReadLine();

            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.SetCursorPosition(0, Console.CursorTop - 1);

            if (!string.IsNullOrEmpty(comment) && comment.Length > 80)
            {
                comment = string.Concat(comment.Chunk(77).ToString(), "...");
            }
        }

        // create pomodoro
        var pomodoro = new Pomodoro
        {
            Type = pomodoroType,
            Comment = string.IsNullOrEmpty(comment) ? null : comment,
            StartTime = TimeOnly.FromDateTime(DateTime.Now),
            EndTime = null,
            PomodoroDate = DateTime.Now,
            IsSuccessful = false
        };

        // set total ticks for chosen pomodoro type
        switch (pomodoroType)
        {
            case PomodoroType.Concentration:
                totalTicks = new TimeSpan(0, _appSettings.ConcentrationTimeInMinutes, 0).Ticks;
                break;
            case PomodoroType.ShortBreak:
                totalTicks = new TimeSpan(0, _appSettings.ShortBreakInMinutes, 0).Ticks;
                break;
            case PomodoroType.LongBreak:
                totalTicks = new TimeSpan(0, _appSettings.LongBreakInMinutes, 0).Ticks;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pomodoroType), pomodoroType, null);
        }

        // set up progress bar options
        var option = new ProgressBarOptions
        {
            ForegroundColor = ConsoleColor.Yellow,
            ForegroundColorDone = ConsoleColor.DarkGreen,
            BackgroundColor = ConsoleColor.DarkGray,
            BackgroundCharacter = '\u2593',
            DisplayTimeInRealTime = true

        };

        using var bar = new FixedDurationBar(new TimeSpan(totalTicks), $"{pomodoro.Type.Humanize(LetterCasing.Title)}: {pomodoro.Comment}", option);

        // begin plaing sound
        if (_appSettings.PlaySound)
        {
            if (pomodoro.Type == PomodoroType.Concentration)
            {
                await playSoundService.PlayAsync("ticking_release.wav");
            }
            else
            {
                await playSoundService.PlayAsync("breack_relese.wav");
            }
        }

        while (!bar.IsCompleted)
        {
            if (_abortRequested)
            {
                break;
            }
        }

        await playSoundService.StopAsync();
        playSoundService.Dispose();

        bar.Dispose();

        if (!_abortRequested)
        {
            isSuccessful = true;
            playSoundService.Play("success_release.wav");
        }
        else
        {
            playSoundService.Play("error_release.wav");
        }

        var pomodoroUpdated = pomodoro with
        {
            EndTime = TimeOnly.FromDateTime(DateTime.Now),
            IsSuccessful = isSuccessful
        };

        if (_appSettings.StoreInDb)
        {
            await _service.AddAsync(pomodoroUpdated);
        }
    }

    public void AbortPomodoro()
    {
        _abortRequested = true;
    }

    public bool IsSuccess()
    {
        return !_abortRequested;
    }
}