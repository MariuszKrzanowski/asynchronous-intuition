using MrMatrix.Net.AllSamples.Properties;
using MrMatrix.Net.AllSamples.Samples3;
using MrMatrix.Net.AllSamples.Samples4;
using MrMatrix.Net.AllSamples.Samples5;
using MrMatrix.Net.AllSamples.Samples1;
using MrMatrix.Net.AllSamples.Samples2;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MrMatrix.Net.AllSamples
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            do
            {
                Console.Clear();
                Console.WriteLine(Resources.OptionsToChoose);

                var chosenOption = ReadChosenOption();
                if ("Q" == chosenOption)
                {
                    return;
                }


                IPresentationSample presentationSample = chosenOption switch
                {
                    "1A" => new PresentaionSample1Async(),
                    "1B" => new PresentaionSample1NoAsync(),

                    "2A" => new PresentationSample5(false),
                    "2B" => new PresentationSample5(true),

                    "3A" => new PresentaionSample2A(),
                    "3B" => new PresentaionSample2B(),
                    "3C" => new PresentaionSample2C(),
                    "3D" => new PresentaionSample2D(),
                    "4A" => new PresentationSample3A(),
                    "4B" => new PresentationSample3B(),
                    "5A" => new PresentationSample4A(),
                    "5B" => new PresentationSample4B(),
                    
                    _ => null
                };

                if (presentationSample == null)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Unrecognized option {chosenOption}");
                    await Task.Delay(1000);
                    Console.ResetColor();
                    continue;
                }
                try
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Chosen Option {chosenOption}");
                    Console.ResetColor();



                    Stopwatch stopwatch = new Stopwatch();

                    Console.WriteLine("**** Prepare ****");
                    await presentationSample.Prepare();
                    Console.WriteLine("**** Run ****");
                    stopwatch.Start();
                    await presentationSample.Run();
                    stopwatch.Stop();
                    Console.WriteLine("**** Cleanup ****");
                    await presentationSample.Cleanup();

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(FormattableString.Invariant($"DONE in {stopwatch.Elapsed}"));
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex);
                }
                finally
                {
                    Console.ResetColor();
                }
                Console.WriteLine("Press ENTER to continue or Q and ENTER to continue.");

                chosenOption = ReadChosenOption();
                if ("Q" == chosenOption)
                {
                    return;
                }
            }
            while (true);
        }

        private static string ReadChosenOption()
        {
            return Console.ReadLine()?.ToUpperInvariant();
        }
    }
}
