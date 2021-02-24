using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MrMatrix.Net.AllSamples.Samples4
{
    public class PresentationSample5A : IPresentationSample
    {
        readonly SemaphoreSlim[] _semaphoreSlim = new SemaphoreSlim[]
            {
                new SemaphoreSlim(1,1),
                new SemaphoreSlim(1,1),
                new SemaphoreSlim(1,1),
                new SemaphoreSlim(1,1),
                new SemaphoreSlim(1,1),
                new SemaphoreSlim(1,1)
        };


        public Task Cleanup()
        {
            return Task.CompletedTask;
        }

        public Task Prepare()
        {
            return Task.CompletedTask;
        }

        public async Task Run()
        {
            Console.WriteLine();
            const int batchSize = 50;
            var tasks = new List<Task>();
            for (int id = 0; id < batchSize; id++)
            {
                tasks.Add(MultipleLoopsWork());
            }
            await Task.WhenAll(tasks.ToArray());
            Console.WriteLine();
        }

        private async Task MultipleLoopsWork()
        {
            for (int i = 0; i < 20; i++)
            {
                await WriterUnitOfWork();
                await Task.Yield();
            }
        }

        private async Task WriterUnitOfWork()
        {
            var tasks = new Task[_semaphoreSlim.Length];

            for (int i = 0; i < _semaphoreSlim.Length; i++)
            {
                tasks[i] = _semaphoreSlim[i].WaitAsync();

                #region uncomment 1
                //for (int k = 0; k < 1000; k++) 
                //{ 
                //}
                #endregion

                #region uncomment 2
                // await tasks[0]; // This is not an error
                // await tasks[i];
                #endregion 
            }

            #region uncomment 3
            // await tasks[0]; // This is not an error
            #endregion
            await Task.WhenAll(tasks);

            await Task.Yield();

            Console.Write(".");
            foreach (var s in _semaphoreSlim)
            {
                s.Release();
            }

        }
    }
}