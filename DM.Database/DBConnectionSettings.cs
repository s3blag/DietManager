using LinqToDB.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace DM.Database
{
    public class DBConnectionSettings : ILinqToDBSettings
    {
        public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();

        public string DefaultConfiguration => "postgres";

        public string DefaultDataProvider => "PostgreSQL";

        public IEnumerable<IConnectionStringSettings> ConnectionStrings => new []
        {
            new ConnectionStringSettings()
            {
                Name = "postgres",
                ProviderName = "PostgreSQL",
                ConnectionString = "Server=localhost;Port=5432;Database=DietManager;User Id=postgres;Password=root;"
            }
        };
    }

    public class ConnectionStringSettings : IConnectionStringSettings
    {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public bool IsGlobal => false;
    }
}
