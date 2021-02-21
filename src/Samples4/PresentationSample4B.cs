using System;
using System.Threading;
using System.Threading.Tasks;

namespace MrMatrix.Net.AllSamples.Samples4
{
    public class PresentationSample4B : IPresentationSample
    {
        ManualResetEvent _barier = new ManualResetEvent(false);
        SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);
        ManualResetEvent _taskDisposingStarted = new ManualResetEvent(false);
        ManualResetEvent _taskWaitingStarted = new ManualResetEvent(false);

        public Task Prepare()
        {
            return Task.CompletedTask;
        }

        public async Task Run()
        {
            var taskDisposing = Task.Run(TaskDisposing);
            var taskAwaiting = Task.Run(TaskWaiting);

            _taskDisposingStarted.WaitOne();
            _taskWaitingStarted.WaitOne();
            _barier.Set();
            await Task.WhenAll();
        }

        private async Task TaskDisposing()
        {
            await _semaphoreSlim.WaitAsync();
            _taskDisposingStarted.Set();
            _barier.WaitOne();
        }

        private async Task TaskWaiting()
        {
            _taskWaitingStarted.Set();
            _barier.WaitOne();
            await _semaphoreSlim.WaitAsync();

        }

        public Task Cleanup()
        {
            return Task.CompletedTask;
        }
    }
}