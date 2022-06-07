using MyPomodoro.Core.Entities;

namespace MyPomodoro.Core.Interfaces;

public interface IPomodoroService
{
    Task RunAsync(PomodoroType pomodoroType);
    bool IsSuccess();
    void AbortPomodoro();
}
