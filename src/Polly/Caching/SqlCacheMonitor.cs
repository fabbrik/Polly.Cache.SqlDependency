using System;
using System.Data.SqlClient;
using System.Runtime.Caching;

namespace Polly.Caching
{
    /// <summary>
    /// The SqlChangeMonitor class wraps the ADO.NET SqlDependency class and adds change monitoring for SQL Server-based dependencies.
    /// Therefore, the SqlChangeMonitor class serves as a bridge between the ADO.NET SqlDependency object
    /// and the System.Runtime.Caching namespace.
    /// </summary>
    public class SqlCacheMonitor : ChangeMonitor
    {
        /// <summary>
        /// Initializes a new instance of the <c>SqlChangeMonitor</c> class.
        /// </summary>

        public SqlCacheMonitor(SqlDependency dependency, Func<Action<object>> onChange = null)
        {

            Dependency = dependency;
            OnChange = onChange;
            Initialize();         
        }
        public void Initialize()
        {
            InitDependency();
            InitializationComplete();

        }
        private void Dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            this.OnChanged(sender);
            if (OnChange != null)
            {
                OnChange.Invoke();
            }
        }

        public override string UniqueId => Guid.NewGuid().ToString();

        public SqlConnection Connection { get; }
        public SqlDependency Dependency { get; }
        public Func<Action<object>> OnChange { get; }

        protected virtual void InitDependency()
        {
            Dependency.OnChange += Dependency_OnChange;
        }

        protected override void Dispose(bool disposing)
        {
         
        }
    }
}