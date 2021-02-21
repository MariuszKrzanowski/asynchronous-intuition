using System;
using System.Diagnostics;
using System.Threading;

namespace MrMatrix.Net.AllSamples.Samples5
{
    public sealed class Worker
    {
        private readonly int _id;
        private readonly ManualResetEvent _barier;
        private readonly Func<long> _action;
        private readonly Thread _thread;
        private Stopwatch _sw;
        private long _loopSize;

        public Worker(int id, ManualResetEvent barier, Func<long> action, bool @override)
        {
            _id = id;
            _barier = barier;
            _action = action;

            _thread = new Thread(new ThreadStart(Run));
            _sw = new Stopwatch();


            #region
            if (@override)
            {
                // override priority 
                _thread.Priority = _id >= 10 ? ThreadPriority.Highest : ThreadPriority.Lowest;
                _thread.IsBackground = _id < 10;
            }
            #endregion
        }

        public void Start()
        {
            _thread.Start();

        }

        private void Run()
        {
            _barier.WaitOne();
            _sw.Start();
            _loopSize = _action();
            _sw.Stop();


        }

        public (long loopSize, TimeSpan elapsed) JoinThreadAndTakeResult()
        {
            _thread.Join();
            return (_loopSize, _sw.Elapsed);
        }

    }
}