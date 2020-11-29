using Microsoft.Extensions.Options;

namespace Polly.Caching
{
    public class SqlDependencyCacheOptions : IOptions<SqlDependencyCacheOptions>
    {
        public SqlDependencyCacheOptions()
        {
        }

        public string CacheRegionName { get; set; }

        public string ConnectionString { get; set; }
        public string CommandText { get; set; }

        public string QueueName { get; set; }

        public SqlDependencyCacheOptions Value { get; set; }
    }
}