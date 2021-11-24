using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public static event Action OnStop;

        private static async Task Main(string[] args)
        {
            // Create your builder.
            var builder = new ContainerBuilder();

            // Usually you're only interested in exposing the type
            // via its interface:
            builder.RegisterType<Service<Pomodoro>>().As<IService<Pomodoro>>();

            // create IOC container
            var container = builder.Build();

            // read settings file into object
            var settingsAddress = Environment.CurrentDirectory + @"\settings.json";
            using StreamReader sr = new StreamReader(settingsAddress);
            var settings = await JsonSerializer.DeserializeAsync<AppSettings>(sr.BaseStream);

            Begin: // use for return to beginning
            Console.WriteLine("Choose what to start - 1)Concentrate; 2)Short break; 3)Long break; 4)Show all todays pomodoros?");
            var key = Console.ReadKey();
            Console.WriteLine();

            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
            {
                Console.WriteLine("You press cancel. Return to begin.");
                e.Cancel = true;
                OnStop?.Invoke();
            };

            var typewriter = new SoundPlayer();
            typewriter.SoundLocation = Environment.CurrentDirectory + @"\ticking.wav";
            Pomodoro pomodoro = null;

            switch (key.KeyChar)
            {
                case '1':
                    // here must be method encapsulate all this shit with sound and ui
                    // method must apply pomodoro type and return pomodoro result object

                    PomodoroService pomodoroService = new PomodoroService();
                    OnStop += pomodoroService.StopLoop;
                    pomodoro = await pomodoroService.RunAsync(PomodoroType.Concentration, typewriter, settings.PlaySound);
                    OnStop -= pomodoroService.StopLoop;
                    // save if chosen option in settings
                    if (settings.StoreInDb)
                    {
                        using (var scope = container.BeginLifetimeScope())
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
                    }

                    //
                    break;
                case '2':
                    break;
                case '3':
                    break;
                case '4':
                    // case for list all today pomodoros
                    Console.WriteLine("Show all pomodoros");
                    IEnumerable<Pomodoro> pomodoros = new List<Pomodoro>();
                    using (var scope = container.BeginLifetimeScope())
                    {
                        var repository = scope.Resolve<IService<Pomodoro>>();
                        pomodoros = await repository.GetAllAsync();
                    }

                    foreach (var pom in pomodoros.Where(x => x.PomodoroDate.Date == DateTime.Now.Date))
                    {
                        Console.WriteLine(pom);
                    }
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey();
                    //
                    break;
                default:
                    MessageWriter.WriteWarning("Wrong enter");
                    goto Begin;
            }

            Console.Clear();
            Console.WriteLine("FINISH!");
            goto Begin;
        }
    }
};