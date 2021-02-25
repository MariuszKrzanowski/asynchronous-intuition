using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace MrMatrix.Net.AllSamples.Samples2
{
    public class PresentaionSample3A : IPresentationSample
    {

        public PresentaionSample3A()
        {

        }



        public Task Prepare()
        {
            return Task.CompletedTask;
        }

        public async Task Run()
        {
            // modified code from MSDN example 
            // see: https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.dataflow.actionblock-1?view=net-5.0

            const int messageCount = 10;
            const int defaultTimeout = 300;

            var workerBlock = new ActionBlock<int>(millisecondsTimeout => Thread.Sleep(millisecondsTimeout));


            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < messageCount; i++)
            {
                workerBlock.Post(defaultTimeout);
            }
            workerBlock.Complete();
            await workerBlock.Completion;
            stopwatch.Stop();
            Console.WriteLine($"ElapsedMilliseconds={stopwatch.ElapsedMilliseconds}");
        }

        public Task Cleanup()
        {
            return Task.CompletedTask;
        }

    }
}
