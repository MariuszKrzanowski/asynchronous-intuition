namespace MrMatrix.Net.AllSamples.Samples1
{
    internal class PresentaionSample1Async : PresentaionSample1AsyncOrNoAsync
    {
        public PresentaionSample1Async() : base(new[] {
                new SomethingHaveToDoServiceWithAsync(ServiceType.Fast),
                new SomethingHaveToDoServiceWithAsync(ServiceType.Slow),
                new SomethingHaveToDoServiceWithAsync(ServiceType.FailingFast),
                new SomethingHaveToDoServiceWithAsync(ServiceType.FailingSlow),
            })
        {
        }
    }


}