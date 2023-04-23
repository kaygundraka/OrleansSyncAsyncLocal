using GrainInterfaces;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;

namespace Grains
{
    public class AsyncLocalTestGrain : Grain, IAsyncLocalTestGrain, IIncomingGrainCallFilter, IOutgoingGrainCallFilter
    {
        private readonly ILogger _logger;

        [SyncAsyncLocal]
        public static AsyncLocal<int> TestAsync { get; set; } = new() { Value = 0 };


        public AsyncLocalTestGrain(ILogger<AsyncLocalTestGrain> logger) => _logger = logger;

        public Task PrintAsyncLocal()
        {
            _logger.LogInformation($"Test Async Value : {TestAsync.Value}");

            return Task.CompletedTask;
        }

        public async Task Invoke(IIncomingGrainCallContext context)
        {
            var properties = typeof(AsyncLocalTestGrain).GetProperties();

            foreach (var property in properties)
            {
                if (property.Attributes.GetType() != typeof(SyncAsyncLocal))
                {
                    continue;
                }

                var value = RequestContext.Get("property.Name");

                if (value == null)
                {
                    continue;
                }

                property.SetValue(null, value);
            }

            await context.Invoke();
        }

        public async Task Invoke(IOutgoingGrainCallContext context)
        {
            var properties = typeof(AsyncLocalTestGrain).GetProperties();

            foreach (var property in properties)
            {
                if (property.Attributes.GetType() != typeof(SyncAsyncLocal))
                {
                    continue;
                }

                RequestContext.Set("property.Name", property);
            }

            await context.Invoke();
        }
    }
}