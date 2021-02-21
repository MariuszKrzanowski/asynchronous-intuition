using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace MrMatrix.Net.AllSamples.Samples2
{

    public class PresentaionSample2B : PresentaionSampleBase
    {
        protected override Task WaitingSimulation() => Task.CompletedTask;

        protected override async Task RegisterSingleCommand(ActionBlock<SimpleCommand> workerBlock, IsolatedState isolatedState, int taskId)
        {
            Console.WriteLine($"\t\tTask {taskId:000}\tA");
            var simpleCommand = new SimpleCommand(isolatedState);

            workerBlock.Post(simpleCommand);
            
            Console.WriteLine($"\t\tTask {taskId:000}\tB");
            var updatedValue = await simpleCommand.UpdatedValue;
            Console.WriteLine($"\t\tTask {taskId:000}\tC\tupdatedValue={updatedValue}");
        }
    }

    public class PresentaionSample2C : PresentaionSampleBase
    {
        protected override Task WaitingSimulation() => Task.Delay(100);

        protected override async Task RegisterSingleCommand(ActionBlock<SimpleCommand> workerBlock, IsolatedState isolatedState, int taskId)
        {
            Console.WriteLine($"\t\tTask {taskId:000}\tA");
            var simpleCommand = new SimpleCommand(isolatedState);

            workerBlock.Post(simpleCommand);

            Console.WriteLine($"\t\tTask {taskId:000}\tB");
            var updatedValue = await simpleCommand.UpdatedValue;
            Console.WriteLine($"\t\tTask {taskId:000}\tC\tupdatedValue={updatedValue}");
        }
    }

    public class PresentaionSample2D : PresentaionSampleBase
    {
        protected override Task WaitingSimulation() => Task.Delay(100);

        protected override async Task RegisterSingleCommand(ActionBlock<SimpleCommand> workerBlock, IsolatedState isolatedState, int taskId)
        {
            Console.WriteLine($"\t\tTask {taskId:000}\tA");
            var simpleCommand = new SimpleCommand(isolatedState);

            if (!workerBlock.Post(simpleCommand))
            {
                simpleCommand.Abort();
            }

            Console.WriteLine($"\t\tTask {taskId:000}\tB");
            var updatedValue = await simpleCommand.UpdatedValue;
            Console.WriteLine($"\t\tTask {taskId:000}\tC\tupdatedValue={updatedValue}");
        }
    }

    public abstract class PresentaionSampleBase : IPresentationSample
    {
        private const int DelayBeforeCompletionInMs = 1000;
        private const int DelayBeforeCleanupInMs = 5000;
       
        private const int NumberOfProducers = 50;

        protected readonly ActionBlock<SimpleCommand> _workerBlock;
        readonly List<Task> _executors;

        public PresentaionSampleBase()
        {
            _workerBlock = new ActionBlock<SimpleCommand>(cmd => cmd.Execute());
            _executors = new List<Task>();
        }

        public Task Prepare()
        {
            Task.Factory.StartNew(async () =>
            {
                IsolatedState isolatedState = new IsolatedState();
                for (int i = 0; i < NumberOfProducers; i++)
                {
                    var taskId = i;
                    _executors.Add(RegisterSingleCommand(_workerBlock, isolatedState, taskId));

                    await WaitingSimulation();
                }
            });

            return Task.CompletedTask;
        }

        protected abstract Task WaitingSimulation();
        protected abstract Task RegisterSingleCommand(ActionBlock<SimpleCommand> workerBlock, IsolatedState isolatedState, int taskId);

       

        public async Task Run()
        {

            await Task.Delay(DelayBeforeCompletionInMs);
            Console.WriteLine("ActionBlock Before Complete.");
            _workerBlock.Complete();
            Console.WriteLine("ActionBlock After Complete.");
            Console.WriteLine("Waiting for pending commands.");
            await _workerBlock.Completion;
            Console.WriteLine("ActionBlock has finished work.");
        }

        public async Task Cleanup()
        {
            await Task.Delay(DelayBeforeCleanupInMs);
            var summary = _executors.GroupBy(t => t.Status).Select(g => new { g.Key, Count = g.Count() }).ToList();
            Console.ForegroundColor = (summary.Count > 1) ? ConsoleColor.Yellow : ConsoleColor.Green;
            Console.WriteLine("Registered SingleCommand tasks statues:");
            summary.ForEach(s => Console.WriteLine($"{s.Key}\t{s.Count}"));
            Console.ResetColor();
            await Task.WhenAny(Task.Delay(2000), Task.WhenAll(_executors.ToArray()));
        }
    }
}
