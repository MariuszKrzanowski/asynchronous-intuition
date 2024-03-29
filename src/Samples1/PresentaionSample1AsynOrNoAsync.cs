﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MrMatrix.Net.AllSamples.Samples1
{
    internal abstract class PresentaionSample1AsyncOrNoAsync : IPresentationSample
    {
        ISomethingHaveToDoService[] _services;
        List<Task<decimal>> _tasks;
        TaskCompletionSource<decimal> _taskCompletionSource;

        protected PresentaionSample1AsyncOrNoAsync(ISomethingHaveToDoService[] services)
        {
            _services = services;
        }

        public Task Cleanup()
        {
            return Task.CompletedTask;
        }

        public Task Prepare()
        {
            _tasks = _services.Select(service => service.DownloadSomeDecimal()).ToList();
            _taskCompletionSource = new TaskCompletionSource<decimal>();

            _tasks.ForEach(task =>
            {
                task.ContinueWith(t => _taskCompletionSource.TrySetResult(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
            });

            return Task.CompletedTask;
        }

        public async Task Run()
        {
            var _ = _taskCompletionSource.Task.ContinueWith(t =>
              {
                  if (t.IsCompletedSuccessfully)
                  {
                      Console.WriteLine(FormattableString.Invariant($"Success {t.Result:000.00}"));
                  }
                  else
                  {
                      Console.WriteLine(t.Exception);
                  }
              });
            

            var ignore=Task.Factory.StartNew(async () =>
            {
                var x = Task.WhenAll(_tasks);
                var cont = x.ContinueWith(t =>
                 {
                     if (!t.IsCompletedSuccessfully)
                     {
                         _taskCompletionSource.TrySetException(t.Exception);
                     }
                 });
                await x;

            });

            await _taskCompletionSource.Task;
        }
    }


}