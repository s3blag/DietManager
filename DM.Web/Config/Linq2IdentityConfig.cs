using DM.Database;
using DM.Models.Models;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.PostgreSQL;
using LinqToDB.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Diet_Manager
{
    public static class Linq2IdentityConfig
    {
        public static void AddLinq2Identity<TKey>(this IServiceCollection services, string databaseConnectionString = null, string schemaName = "Users") where TKey : IEquatable<TKey>
        {
            if (databaseConnectionString != null)
            {
                DataConnection
                .AddConfiguration(
                    "Default",
                    databaseConnectionString,
                    new PostgreSQLDataProvider());

                DataConnection.DefaultConfiguration = "Default";
            }

            SetMapping<TKey>(schemaName);

            services.AddIdentity<ApplicationUser, LinqToDB.Identity.IdentityRole<TKey>>()
                .AddLinqToDBStores<TKey>(new DefaultConnectionFactory())
                .AddDefaultTokenProviders();

            TryCreateDatabaseAndSchema();

            TryCreateTables<TKey>();
        }

        private static void SetMapping<TKey>(string schemaName) where TKey : IEquatable<TKey> =>
            LinqToDB.Mapping.MappingSchema.Default.GetFluentMappingBuilder()
                          .Entity<ApplicationUser>()
                            .HasTableName("User")
                            .HasIdentity(_ => _.Id)
                            .HasSchemaName(schemaName)
                          .Entity<LinqToDB.Identity.IdentityRole<TKey>>()
                            .HasSchemaName(schemaName)
                            .HasTableName("Role")
                            .HasIdentity(_ => _.Id)
                          .Entity<LinqToDB.Identity.IdentityRoleClaim<TKey>>()
                            .HasSchemaName(schemaName)
                            .HasTableName("RoleClaim")
                            .HasIdentity(_ => _.Id)
                          .Entity<LinqToDB.Identity.IdentityUserClaim<TKey>>()
                            .HasSchemaName(schemaName)
                            .HasTableName("UserClaim")
                            .HasIdentity(_ => _.Id)
                          .Entity<LinqToDB.Identity.IdentityUserLogin<TKey>>()
                            .HasSchemaName(schemaName)
                            .HasTableName("UserLogin")
                          .Entity<LinqToDB.Identity.IdentityUserRole<TKey>>()
                            .HasSchemaName(schemaName)
                            .HasTableName("UserRole")
                          .Entity<LinqToDB.Identity.IdentityUserToken<TKey>>()
                            .HasSchemaName(schemaName)
                            .HasTableName("UserToken");

        private static void TryCreateDatabaseAndSchema(string databaseName = "DietManager", string schemaName = "Users")
        {
            using (var db = new DietManagerDB())
            {
                try
                {
                    string sql = $"CREATE DATABASE \"{databaseName}\"";
                    db.Execute(sql);
                }
                catch
                { }

                try
                {
                    string sql = $"CREATE SCHEMA \"{schemaName}\"";
                    db.Execute(sql);
                }
                catch
                { }
            }
        }

        private static void TryCreateTables<TKey>(string schemaName = "Users") where TKey : IEquatable<TKey>
        {
            using (var db = new DietManagerDB())
            {
                TryCreateTable<ApplicationUser>(db);
                TryCreateTable<LinqToDB.Identity.IdentityRole<TKey>>(db);
                TryCreateTable<LinqToDB.Identity.IdentityUserClaim<TKey>>(db);
                TryCreateTable<LinqToDB.Identity.IdentityRoleClaim<TKey>>(db);
                TryCreateTable<LinqToDB.Identity.IdentityUserLogin<TKey>>(db);
                TryCreateTable<LinqToDB.Identity.IdentityUserRole<TKey>>(db);
                TryCreateTable<LinqToDB.Identity.IdentityUserToken<TKey>>(db);
            }

            void TryCreateTable<T>(DietManagerDB db) where T : class
            {
                try
                {
                    db.CreateTable<T>();
                }
                catch
                { }
            }
        }

    }
}
