using LinqToDB.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace DM.Database
{
    public class DBConnectionSettings : ILinqToDBSettings
    {
        public DBConnectionSettings(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();

        public string DefaultConfiguration => "postgres";

        public string DefaultDataProvider => "PostgreSQL";

        private string ConnectionString { get; }

        public IEnumerable<IConnectionStringSettings> ConnectionStrings
        {
            get
            {
                return new[] {
                    new ConnectionStringSettings()
                    {
                        Name = "postgres",
                        ProviderName = "PostgreSQL",
                        ConnectionString = ConnectionString
                    }
                 };
            }
        
        }
        
    }

    public class ConnectionStringSettings : IConnectionStringSettings
    {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public bool IsGlobal => false;
    }
}
