namespace Diatly.Foundation.Test
{
    using RestMockCore;

    public sealed class InMemoryWebHost
    {
        private static readonly object Padlock = new object();
        private static HttpServer instance = null;

        private InMemoryWebHost()
        {
        }

        public static HttpServer Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (instance == null)
                    {
                        instance = new HttpServer();
                        instance.Run();
                    }

                    return instance;
                }
            }
        }
    }
}
