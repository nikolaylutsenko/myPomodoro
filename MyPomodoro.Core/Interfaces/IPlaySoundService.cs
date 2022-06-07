namespace MyPomodoro.Core.Interfaces;

public interface IPlaySoundService : IDisposable
{
    Task PlayAsync(string name);
    void Play(string name);
    Task StopAsync();
}