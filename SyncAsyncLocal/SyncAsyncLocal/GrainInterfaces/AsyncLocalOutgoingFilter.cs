using Orleans.Runtime;

namespace Grains
{
    public class AsyncLocalOutgoingFilter : IOutgoingGrainCallFilter
    {
        public async Task Invoke(IOutgoingGrainCallContext context)
        {
            string? unittestTag = UnittestAsyncLocalContext.GetUnittestContext();

            if (unittestTag != null && unittestTag != string.Empty)
            {
                RequestContext.Set("UnittestTag", unittestTag);
            }

            await context.Invoke();
        }
    }
}
