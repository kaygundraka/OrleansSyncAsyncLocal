namespace Grains
{
    public class UnittestAsyncLocalContext
    {
        private static object handlerLock = new();

        public static Dictionary<string, Action> Handlers = new Dictionary<string, Action>();

        public static AsyncLocal<string> CurrentContext = new() { Value = string.Empty };

        public static void SetUnittestContext(string tag)
        {
            CurrentContext.Value = tag;
        }

        public static string? GetUnittestContext()
        {
            return CurrentContext.Value;
        }

        public static void ResetUnittestContext()
        {
            CurrentContext.Value = string.Empty;
        }

        public static void RegisterHandler(string tag, Action handler)
        {
            lock (handlerLock)
            {
                if (Handlers.ContainsKey(tag))
                {
                    // 에러로그 출력
                    return;
                }

                Handlers.Add(tag, handler);
            }
        }

        public static void Handler(string? tag)
        {
            lock (handlerLock)
            {
                if (tag == null)
                {
                    return;
                }

                Handlers[tag]?.Invoke();
            }
        }

        public static void InitHandler() // Silo에서 실행되어야 함
        {
            RegisterHandler("Unittest_TestAsync1", () => ValueForUnittest<int>.UnittestSetter.Set(AsyncLocalTestGrain.TestAsync, 10));
            RegisterHandler("Unittest_TestAsync2", () => ValueForUnittest<int>.UnittestSetter.Set(AsyncLocalTestGrain.TestAsync, 20));
        }
    }
}
