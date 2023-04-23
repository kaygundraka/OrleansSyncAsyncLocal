namespace Grains
{
    public interface IAsyncLocalTestGrain : IGrainWithIntegerKey
    {
        Task PrintAsyncLocal1();
        Task PrintAsyncLocal2();
    }
}