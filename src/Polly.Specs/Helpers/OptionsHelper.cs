using System;
using System.Collections.Generic;
using System.Text;

namespace Polly.Caching.SqlDependency.Specs.Helpers
{
    public class OptionsHelper
    {
    
    public static SqlDependencyCacheOptions GetDefaultOptions()
        {
            return new SqlDependencyCacheOptions
            {
                CacheRegionName = "UnitTest",
                CommandText = "Select * From CacheEntries",
                ConnectionString = "Data Source=(localdb)\\ProjectsV13;Initial Catalog=testsdb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
                QueueName = ""

            };
        }
    
    
    }
}
