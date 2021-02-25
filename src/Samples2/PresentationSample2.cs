using MrMatrix.Net.AllSamples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MrMatrix.Net.AllSamples.Samples5
{
    public class PresentationSample2 : IPresentationSample
    {
        List<Worker> _workers = new List<Worker>();
        ManualResetEvent _barier = new ManualResetEvent(false);
        private bool _override;

        public PresentationSample2(bool @override)
        {
            _override = @override;
        }

        public Task Prepare()
        {
            for (int i = 1; i <= 20; i++)
            {
                CreateAndRegisterWorker(i);
            }
            _workers.ForEach(w => w.Start());
            return Task.CompletedTask;
        }

        private void CreateAndRegisterWorker(int id)
        {
            _workers.Add(new Worker(id, _barier, () => TestLoop(id * 100_000_000), _override));
        }

        public long TestLoop(long max)
        {
            unchecked
            {

                double tmp = 0;
                for (long i = 0; i < max; i++)
                {
                    tmp = i;
                }
                tmp = 0;
                return max;
            }
        }

        public Task Run()
        {

            _barier.Set();
            TestLoop(20 * 100_000_000);
            return Task.CompletedTask;
        }

        public Task Cleanup()
        {
            var agreagatedResults =
                _workers
                .Select(w => w.JoinThreadAndTakeResult())
                .OrderBy(s => s.elapsed)
                .ToList();

            agreagatedResults.ForEach(r => Console.WriteLine($"{r.elapsed} {r.loopSize,20:### ### ### ###}"));

            return Task.CompletedTask;
        }
    }
}