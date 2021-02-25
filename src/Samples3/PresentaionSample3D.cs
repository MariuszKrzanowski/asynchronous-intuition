using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace MrMatrix.Net.AllSamples.Samples2
{
    public class PresentaionSample3D : PresentaionSampleBase
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
}
