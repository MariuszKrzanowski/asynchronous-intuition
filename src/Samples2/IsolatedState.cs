namespace MrMatrix.Net.AllSamples.Samples2
{
    public class IsolatedState
    {
        private int _executionCounter;

        internal void Increment() => _executionCounter++;

        internal int Counter => _executionCounter;
    }
}
