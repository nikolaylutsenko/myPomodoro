using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using MyPomodoro.Core.Entities;

namespace MyPomodoro.Core.Settings
{
    public static class SettingsService
    {
        private const string SettingsFileName = @"\settings.json";
        
        public static async Task<AppSettings> GetSettings()
        {
            var settingsAddress = Environment.CurrentDirectory + SettingsFileName;
            using StreamReader sr = new StreamReader(settingsAddress);
            var settings = await JsonSerializer.DeserializeAsync<AppSettings>(sr.BaseStream);

            if (settings is null)
                throw new Exception("Settings file not read.");

            return settings;
        }
    }
}