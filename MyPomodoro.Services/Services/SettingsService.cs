using System.Text.Json;
using MyPomodoro.Core.Entities;
using MyPomodoro.Core.Interfaces;

namespace MyPomodoro.Services.Services;

public class SettingsService : ISettingsService
{
    private const string SettingsFileName = @"\settings.json";

    public async Task<AppSettings> GetSettings()
    {
        var settingsAddress = Environment.CurrentDirectory + SettingsFileName;
        using StreamReader sr = new StreamReader(settingsAddress);
        var settings = await JsonSerializer.DeserializeAsync<AppSettings>(sr.BaseStream);

        if (settings is null)
            throw new Exception("Settings file not read.");

        return settings;
    }
}