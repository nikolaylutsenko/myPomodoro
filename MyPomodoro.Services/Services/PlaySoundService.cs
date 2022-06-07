using MyPomodoro.Core.Interfaces;
using System.Media;

namespace MyPomodoro.Services.Services;

public class PlaySoundService : IPlaySoundService
{
    private readonly SoundPlayer _soundPlayer;
    private readonly string _pathToFolder;

    public PlaySoundService(string path)
    {
        _soundPlayer = new();
        _pathToFolder = Environment.CurrentDirectory + path;
    }

    public async Task PlayAsync(string name)
    {
        _soundPlayer.SoundLocation = _pathToFolder + name;
        await Task.Run(() => _soundPlayer.PlayLooping());
    }

    public void Play(string name)
    {
        _soundPlayer.SoundLocation = _pathToFolder + name;
        _soundPlayer.PlaySync();
    }

    public async Task StopAsync()
    {
        await Task.Run(() => _soundPlayer.Stop());
    }

    public void Dispose()
    {
        _soundPlayer.Dispose();
    }

}