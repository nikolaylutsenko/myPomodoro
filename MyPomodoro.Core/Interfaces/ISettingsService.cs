using MyPomodoro.Core.Entities;

namespace MyPomodoro.Core.Interfaces;

public interface ISettingsService
{
    Task<AppSettings> GetSettings();
}
