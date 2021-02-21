using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MrMatrix.Net.AllSamples.Samples3
{
    public class PresentationSample3A : IPresentationSample
    {
        private class Result
        {
            public int OriginatorId { get; set; }

            public string AddedValue { get; set; }
            public string GeneratedValue { get; set; } = "----------";

            public TimeSpan Elapsed { get; set; }

            public int TimespanA { get; set; }
            public int TimespanB { get; set; }
        }


        private ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
        private int _counter = 0;
        private int _timespan = 0;
        ConcurrentDictionary<int, string> _concurrentDictionary;
        ConcurrentQueue<Result> _testResults;

        public PresentationSample3A()
        {
            _manualResetEvent = new ManualResetEvent(false);
            _counter = 0;
            _timespan = 0;
            _concurrentDictionary = new ConcurrentDictionary<int, string>();
            _testResults = new ConcurrentQueue<Result>();
        }



        public Task Prepare()
        {


            return Task.CompletedTask;

        }

        public async Task Run()
        {
            const int batchSize = 5;
            var tasks = new List<Task>();
            for (int id = 0; id < batchSize; id++)
            {
                var originatorId = id;
                tasks.Add(Task.Factory.StartNew(() => SingleThread(originatorId), TaskCreationOptions.LongRunning));
            }

            _manualResetEvent.Set();
            await Task.WhenAll(tasks.ToArray());
            // When the key is ready

            tasks = new List<Task>();
            for (int id = batchSize; id < 2 * batchSize; id++)
            {
                var originatorId = id;
                tasks.Add(Task.Factory.StartNew(() => SingleThread(originatorId), TaskCreationOptions.LongRunning));
            }
            await Task.WhenAll(tasks.ToArray());
        }

        private void SingleThread(int originatorId)
        {
            var result = new Result()
            {
                OriginatorId = originatorId
            };

            _manualResetEvent.WaitOne();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            result.TimespanA = Interlocked.Increment(ref _timespan);
            result.AddedValue = _concurrentDictionary.GetOrAdd(1, (key) =>
            {
                var currentCounter = Interlocked.Increment(ref _counter);
                Thread.Sleep(currentCounter * 400);
                result.GeneratedValue = $"id:{originatorId:00} c:{currentCounter:00}";
                return result.GeneratedValue;

            });
            result.TimespanB = Interlocked.Increment(ref _timespan);
            sw.Stop();
            result.Elapsed = sw.Elapsed;
            _testResults.Enqueue(result);
        }

        public Task Cleanup()
        {
            List<Result> results = new List<Result>();

            while (_testResults.TryDequeue(out var result))
            {
                results.Add(result);
            }


            foreach (var result in results.OrderBy(r => r.OriginatorId))
            {
                Console.WriteLine();
                Console.Write($" Id {result.OriginatorId:00} |");
                for (int i = 1; i <= _timespan; i++)
                {
                    Console.ForegroundColor = Console.BackgroundColor;
                    if (i == result.TimespanA)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }
                    if (i == result.TimespanB)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }
                    Console.Write("■");
                    Console.ResetColor();
                }

                Console.Write($"|    ");
                Console.Write($"[{result.GeneratedValue}]");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write($"[{result.AddedValue}]");
                Console.ResetColor();
                Console.Write($"[{result.Elapsed}]");
            }

            return Task.CompletedTask;
        }

    }
}