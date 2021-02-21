using System.Threading.Tasks;

namespace MrMatrix.Net.AllSamples
{
    public interface IPresentationSample
    {
        Task Cleanup();
        Task Prepare();
        Task Run();
    }
}