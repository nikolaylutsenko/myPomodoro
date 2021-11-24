using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Autofac;
using MyPomodoro.Core.Entities;
using MyPomodoro.Core.Interfaces;
using MyPomodoro.Dal;
using ShellProgressBar;
using Spectre.Console;

namespace MyPomodoro
{
    public class Program
    {
        // Algorithm:
        // Start app
        // Read settings.json
        // Connect to SqLite
        // Choose type of pomodoro - work, short break, long break
        // Enter name(optional)
        // Start timer
        // Show status bar
        // Sound playing
        // End timer
        // Save to Db
        // Ask what is next - by plan or personal chose

        private static async Task Main(string[] args)
        {
            // Create your builder.
            var builder = new ContainerBuilder();

            // Usually you're only interested in exposing the type
            // via its interface:
            builder.RegisterType<Service<Pomodoro>>().As<IService<Pomodoro>>();

            // However, if you want BOTH services (not as common)
            // you can say so:
            //uilder.RegisterType<SomeType>().AsSelf().As<IService>();

            var Container = builder.Build();
            var settingsAddress = Environment.CurrentDirectory + @"\settings.json";
            using StreamReader sr = new StreamReader(settingsAddress);
            var settings = await JsonSerializer.DeserializeAsync<AppSettings>(sr.BaseStream);

            Begin: // use for return to beginning
            Console.WriteLine("Choose what to start - 1)Concentrate; 2)Short break; 3)Long break?");
            var key = Console.ReadKey();
            Console.WriteLine();

            var keepRunning = true;

            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
            {
                Console.WriteLine("You press cancel. Return to begin.");
                e.Cancel = true;
                keepRunning = false;
            };

            int i = 0;
            SoundPlayer typewriter = new SoundPlayer();
            typewriter.SoundLocation = Environment.CurrentDirectory + @"\ticking.wav";
            Pomodoro pomodoro = null;

            switch (key.KeyChar)
            {
                case '1':
                    // here must be method incupsulate all this shit with sound and ui
                    Console.WriteLine("Some comment?");
                    var comment = Console.ReadLine();
                    
                    TimeOnly beginTime;
                    await Task.Run(() =>
                    {
                        const int totalTicks = 10;
                        var option = new ProgressBarOptions
                        {
                            ForegroundColor = ConsoleColor.Yellow,
                            ForegroundColorDone = ConsoleColor.DarkGreen,
                            BackgroundColor = ConsoleColor.DarkGray,
                            BackgroundCharacter = '\u2593'
                        };

                        pomodoro = new Pomodoro
                        {
                            Type = PomodoroType.ShortBreak,
                            Comment = string.IsNullOrEmpty(comment) ? null : comment,
                            StartTime = TimeOnly.FromDateTime(DateTime.Now),
                            EndTime = null,
                            PomodoroDate = DateTime.Now,
                            IsSuccessful = false
                        };

                        var timePlus = pomodoro.StartTime.Value.Second + totalTicks;

                        typewriter.PlayLooping();

                        using var pbar = new ProgressBar(totalTicks, pomodoro.Type.ToString(), option);

                        TimeOnly tickTime = beginTime;

                        while (keepRunning)
                        {
                            var currentTime = TimeOnly.FromDateTime(DateTime.Now);

                            if (tickTime.Second < currentTime.Second)
                            {
                                pbar.Tick();
                                tickTime = currentTime;
                            }

                            if (timePlus < currentTime.Second)
                            {
                                keepRunning = false;
                            }
                        }
                        pomodoro.EndTime = TimeOnly.FromDateTime(DateTime.Now);
                        typewriter.Stop();
                    });
                    pomodoro.IsSuccessful = true;
                    using (var scope = Container.BeginLifetimeScope())
                    {
                        try
                        {
                            var repository = scope.Resolve<IService<Pomodoro>>();
                            repository.AddAsync(pomodoro);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }
                    Task.WaitAll();
                    // save into db
                    break;
                case '2':
                    break;
                case '3':
                    break;
                case '4':
                    Console.WriteLine("Show all pomodoros");
                    IEnumerable<Pomodoro> pomodoros = new List<Pomodoro>();
                    using (var scope = Container.BeginLifetimeScope())
                    {
                        try
                        {
                            var repository = scope.Resolve<IService<Pomodoro>>();
                            pomodoros = await repository.GetAllAsync();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }
                    break;
                default:
                    MessageWriter.WriteWarning("Wrong enter");
                    goto Begin;
            }

            Console.WriteLine("FINISH!");
        }
    }
};