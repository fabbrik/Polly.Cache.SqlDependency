using Polly.Caching;
using System.Data.SqlClient;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Caching
{
    internal class SqlDependencyCacheProvider : ISyncCacheProvider, IAsyncCacheProvider
    {
        private readonly MemoryCache Mem = MemoryCache.Default;

        public SqlDependencyCacheProvider(SqlDependencyCacheOptions options)
        {
            Options = options;

            Start();
        }

        public SqlDependencyCacheOptions Options { get; }

        public void Put(string key, object value, Ttl ttl)
        {
            var item = new CacheItem(key)
            {
                Value = value,
                RegionName = Options.CacheRegionName
            };
            Mem.Set(item, BuildCachePolicy(key));
        }

        public Task PutAsync(string key, object value, Ttl ttl, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            Put(key, value, ttl);
            return Task.FromResult(true);
        }

        public (bool, object) TryGet(string key)
        {
            return Mem.Contains(key) ? (true, Mem.Get(key))
                : (false, null);
        }

        public Task<(bool, object)> TryGetAsync(string key, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            return Task.FromResult(TryGet(key));
        }

        protected virtual CacheItemPolicy BuildCachePolicy(string key)
        {
            CacheItemPolicy policy = new CacheItemPolicy
            {
            };
            policy.ChangeMonitors.Add(new SqlCacheMonitor(TrackDependency(key)));

            return policy;
        }

        protected virtual string GetCommandText(string key)
        {
            return string.Format(Options.CommandText, key);
        }

        protected virtual SqlCommand GetSqlCommand(SqlConnection conn, string key)
        {
            var cmd = conn.CreateCommand();

            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = GetCommandText(key);
            cmd.Notification = null;

            return cmd;
        }

        protected void Start()
        {
            Stop();
            SqlDependency.Start(Options.ConnectionString);
        }

        protected void Stop()
        {
            SqlDependency.Stop(Options.ConnectionString);
        }

        protected virtual SqlDependency TrackDependency(string key)
        {
            using (var connection = new SqlConnection(Options.ConnectionString))
            {
                connection.Open();

                var cmd = GetSqlCommand(connection, key);

                var dependency = new SqlDependency(cmd);

                cmd.ExecuteReader();

                return dependency;
            }
        }
    }
}