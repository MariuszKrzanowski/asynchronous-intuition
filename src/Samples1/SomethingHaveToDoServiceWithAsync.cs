using System;
using System.Threading.Tasks;

namespace MrMatrix.Net.AllSamples.Samples1
{
    public class SomethingHaveToDoServiceWithAsync : ISomethingHaveToDoService
    {
        private readonly ServiceType _serviceType;

        public SomethingHaveToDoServiceWithAsync(ServiceType serviceType)
        {
            _serviceType = serviceType;
        }

        public async Task<decimal> DownloadSomeDecimal()
        {
            if (_serviceType == ServiceType.FailingFast)
            {
                throw new Exception("Validation exception");
            }

            return await CallToExternalUrl();
        }

        private async Task<decimal> CallToExternalUrl()
        {
            switch (_serviceType)
            {
                case ServiceType.Fast:
                    await Task.Delay(2000);
                    return 2.11m;
                case ServiceType.Slow:
                    await Task.Delay(5000);
                    return 5.55m;
            }

            await Task.Delay(500);
            throw new Exception("HTTP Response 500");
        }
    }


}