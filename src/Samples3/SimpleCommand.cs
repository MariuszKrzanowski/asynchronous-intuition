using System.Threading.Tasks;

namespace MrMatrix.Net.AllSamples.Samples2
{
    public class SimpleCommand
    {
        private readonly IsolatedState _isolatedState;
        readonly TaskCompletionSource<int> _taskCompletionSource;

        public SimpleCommand(IsolatedState isolatedState)
        {
            _taskCompletionSource = new TaskCompletionSource<int>();
            _isolatedState = isolatedState;
        }

        public async Task Execute()
        {
            await Task.Delay(30); // simulate some work
            _isolatedState.Increment();
            _taskCompletionSource.SetResult(_isolatedState.Counter);
        }
        public void Abort()
        {
            _taskCompletionSource.SetCanceled();
        }

        public Task<int> UpdatedValue => _taskCompletionSource.Task;
    }
}
