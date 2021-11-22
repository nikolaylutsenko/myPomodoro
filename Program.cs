using System.Media;
using System.Threading;

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
        
        private static void Main(string[] args)
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
                    var currentTime = TimeOnly.FromDateTime(DateTime.Now);
                    var timePlus = currentTime.Second + 10;
                    typewriter.PlayLooping();
                    while (keepRunning)
                    {
                        currentTime = TimeOnly.FromDateTime(DateTime.Now);
                        if (timePlus < currentTime.Second)
                        {
                            keepRunning = false;
                        }
                        Console.WriteLine(++i);
                        //Thread.Sleep(1000);
                    }
                    typewriter.Stop();
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



