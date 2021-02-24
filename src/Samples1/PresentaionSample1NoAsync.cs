namespace MrMatrix.Net.AllSamples.Samples1
{
    internal class PresentaionSample1NoAsync : PresentaionSample1AsyncOrNoAsync
    {
        public PresentaionSample1NoAsync() : base(new[] {
                new SomethingHaveToDoServiceWithoutAsync(ServiceType.Fast),
                new SomethingHaveToDoServiceWithoutAsync(ServiceType.Slow),
                new SomethingHaveToDoServiceWithoutAsync(ServiceType.FailingFast),
                new SomethingHaveToDoServiceWithoutAsync(ServiceType.FailingSlow),
            })
        {
        }
    }


}