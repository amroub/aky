namespace Diatly.Foundation.CacheManager
{
    using System;
    using System.Net;
    using StackExchange.Redis;

    public class RedisConnectionWrapper : IRedisConnectionWrapper
    {
        private readonly IRedisServerSettings settings;
        private readonly Lazy<string> connectionString;
        private readonly object @lock = new object();

        private volatile ConnectionMultiplexer connection;

        public RedisConnectionWrapper(IRedisServerSettings settings)
        {
            this.settings = settings;

            this.connectionString = new Lazy<string>(this.GetConnectionString);
        }

        public IDatabase Database(int? db = null) => this.GetConnection().GetDatabase(db ?? 0);

        public IServer Server(EndPoint endPoint) => this.GetConnection().GetServer(endPoint);

        public EndPoint[] GetEndpoints() => this.GetConnection().GetEndPoints();

        public void FlushDb(int? db = null)
        {
            var endPoints = this.GetEndpoints();

            foreach (var endPoint in endPoints)
            {
                this.Server(endPoint).FlushDatabase(db ?? 0);
            }
        }

        public void Dispose()
        {
            if (this.connection != null)
            {
                this.connection.Dispose();
            }
        }

        private string GetConnectionString()
        {
            return this.settings.ConnectionString;
        }

        private ConnectionMultiplexer GetConnection()
        {
            if (this.connection != null && this.connection.IsConnected)
            {
                return this.connection;
            }

            lock (this.@lock)
            {
                if (this.connection != null && this.connection.IsConnected)
                {
                    return this.connection;
                }

                if (this.connection != null)
                {
                    this.connection.Dispose();
                }

                this.connection = ConnectionMultiplexer.Connect(this.connectionString.Value);
            }

            return this.connection;
        }
    }
}
