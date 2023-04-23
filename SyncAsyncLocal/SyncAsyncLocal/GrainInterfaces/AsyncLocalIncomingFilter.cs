using Orleans.Runtime;

namespace Grains
{
    public class AsyncLocalIncomingFilter : IIncomingGrainCallFilter
    {
        public async Task Invoke(IIncomingGrainCallContext context)
        {
            string? unittestTag = RequestContext.Get("UnittestTag") as string;

            UnittestAsyncLocalContext.Handler(unittestTag);

            await context.Invoke();
        }
    }
}
