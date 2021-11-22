using System.Media;
using ShellProgressBar;

namespace MyPomodoro
{
    public class Program
    {
        
        // Algorithm:
        // Start app
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
            Begin: // use for return to beginning
            Console.WriteLine("Choose what to start - 1)Concentrate; 2)Short break; 3)Long break?");
            var key = Console.ReadKey();
            Console.WriteLine();
            
            var keepRunning = true;
             
            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e) {
                Console.WriteLine("You press cancel. Return to begin.");
                e.Cancel = true;
                keepRunning = false;
            };
            
            int i = 0;
            SoundPlayer typewriter = new SoundPlayer();
            typewriter.SoundLocation = Environment.CurrentDirectory + @"\ticking.wav";
            
            
            
            switch (key.KeyChar)
            {
                case '1':
                    // here must be method incupsulate all this shit with sound and ui
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
                        
                        beginTime = TimeOnly.FromDateTime(DateTime.Now);
                        var timePlus = beginTime.Second + totalTicks;
                        
                        typewriter.PlayLooping();
                        
                        using var pbar = new ProgressBar(totalTicks, "test", option);
                        
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
                        
                        typewriter.Stop();
                    });
                    
                    Task.WaitAll();
                    // save into db
                    break;
                case '2':
                    break;
                case '3':
                    break;
                default:
                    MessageWriter.WriteWarning("Wrong enter");
                    goto Begin;
            }

            Console.WriteLine("FINISH!");
            
            
            

            
            
        }
    }
};



