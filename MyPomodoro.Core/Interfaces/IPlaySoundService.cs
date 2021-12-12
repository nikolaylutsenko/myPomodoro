using System.Media;

namespace MyPomodoro.Core.Interfaces;

public interface IPlaySoundService : IDisposable
{
    Task PlayAsync();
    Task StopAsync();
}

public class PlaySoundService : IPlaySoundService
{
    private readonly SoundPlayer _soundPlayer = new ();
    
    public PlaySoundService(string path)
    {
        _soundPlayer.SoundLocation = Environment.CurrentDirectory + path;
    }
    
    public async Task PlayAsync()
    {
        await Task.Run(() => _soundPlayer.PlayLooping());
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