using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MrMatrix.Net.AllSamples.Samples4
{
    public class PresentationSample4B : IPresentationSample
    {
        SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);
        ManualResetEvent _taskDisposingStarted = new ManualResetEvent(false);
        ManualResetEvent _taskWaitingStarted = new ManualResetEvent(false);
        CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public Task Prepare()
        {
            return Task.CompletedTask;
        }

        public async Task Run()
        {
            List<Task> allTasks = new List<Task>();
            allTasks.Add(TaskDisposing());
            allTasks.Add(TaskWaiting(1));
            allTasks.Add(TaskWaiting(2));
            allTasks.Add(TaskWaiting(3));

            _taskDisposingStarted.WaitOne();
            _taskWaitingStarted.WaitOne();
            await Task.WhenAll(allTasks.ToArray());
        }

        private async Task TaskDisposing()
        {
            _taskDisposingStarted.Set();


            Console.WriteLine("TaskDisposing before WaitAsync.");
            await _semaphoreSlim.WaitAsync();
            Console.WriteLine("TaskDisposing after WaitAsync.");
            await Task.Delay(2000);
            Console.WriteLine("TaskDisposing before Release.");
            _semaphoreSlim.Release();
            Console.WriteLine("TaskDisposing after Release.");
            await Task.Delay(2000);
            Console.WriteLine("************ TaskDisposing before Dispose.");
            //_cancellationTokenSource.Cancel();
            _semaphoreSlim.Dispose();
            _cancellationTokenSource.Cancel();
            Console.WriteLine("************ TaskDisposing after Dispose.");
            Console.WriteLine("TaskDisposing DONE");

        }

        private async Task TaskWaiting(int id)
        {
            try
            {
                _taskWaitingStarted.Set();

                for (int repeat = 0; repeat < 3; repeat++)
                {
                    //await Task.Delay(9_000);
                    await Task.Delay(1_000);

                    Console.WriteLine($"TaskWaiting({id}) before WaitAsync.");

                    #region Variant with WaitAsync
                    await _semaphoreSlim.WaitAsync();
                    #endregion


                    #region
                    //if (!await _semaphoreSlim.WaitAsync(TimeSpan.FromSeconds(10)))
                    //{
                    //    Console.WriteLine($"TaskWaiting({id}) after Timeout.");
                    //    return;
                    //}
                    #endregion

                    #region
                    //if (!await _semaphoreSlim.WaitAsync(TimeSpan.FromSeconds(10), _cancellationTokenSource.Token))
                    //{
                    //    Console.WriteLine($"TaskWaiting({id}) after Timeout.");
                    //    return;
                    //}
                    #endregion

                    #region
                    //await _semaphoreSlim.WaitAsync(_cancellationTokenSource.Token);
                    #endregion

                    Console.WriteLine($"TaskWaiting({id}) after WaitAsync.");

                    await Task.Delay(2000);
                    Console.WriteLine($"TaskWaiting({id}) before Release.");
                    _semaphoreSlim.Release();
                    Console.WriteLine($"TaskWaiting({id}) after Release.");
                }
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine($"TaskWaiting({id}) Canceled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TaskWaiting({id}) ERROR {ex.Message}.");
            }
            finally
            {
                Console.WriteLine($"TaskWaiting({id}) DONE.");
            }
        }

        public Task Cleanup()
        {
            return Task.CompletedTask;
        }
    }
}