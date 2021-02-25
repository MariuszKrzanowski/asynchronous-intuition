using System.Threading.Tasks;

namespace MrMatrix.Net.AllSamples.Samples1
{
    public interface ISomethingHaveToDoService
    {
        Task<decimal> DownloadSomeDecimal();
    }
}