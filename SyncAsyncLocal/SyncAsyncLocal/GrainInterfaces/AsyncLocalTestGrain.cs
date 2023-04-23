using Microsoft.Extensions.Logging;

namespace Grains
{
    public class AsyncLocalTestGrain : Grain, IAsyncLocalTestGrain
    {
        private readonly ILogger _logger;

        public static ValueForUnittest<int> TestAsync { get; set; } = new(5);

        public AsyncLocalTestGrain(ILogger<AsyncLocalTestGrain> logger) => _logger = logger;

        public async Task PrintAsyncLocal1()
        {
            _logger.LogInformation($"Test Async Value 1 : {TestAsync.Value}");

            var grain = GrainFactory.GetGrain<IAsyncLocalTestGrain>(1);

            await grain.PrintAsyncLocal2();
        }

        public async Task PrintAsyncLocal2()
        {
            await Task.Delay(1);

            _logger.LogInformation($"Test Async Value 2 : {TestAsync.Value}");
        }
    }
}