using System;
using Cabother.Organizer.Application.Interfaces.DataProviders.Teams;
using Cabother.Organizer.Infraestructure.Data.Configs;
using Cabother.Organizer.Infraestructure.DataProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Cabother.Organizer.Infraestructure.Extensions
{
    public static class DataExtensions
    {
        private const string EnvHost = "DB_ORGANIZER_HOST";
        private const string EnvDatabase = "DB_ORGANIZER_DATABASE";
        private const string EnvUsername = "DB_ORGANIZER_USER";
        private const string EnvPassword = "DB_ORGANIZER_PASSWORD";
        private const string EnvPort = "DB_ORGANIZER_PORT";

        private const string MigrationsHistoryTable = "__EFMigrationsHistory";
        private const string MigrationsAssembly = "Cabother.Organizer.Infrastructure";

        public static void AddOrganizerData(this IServiceCollection services, IConfiguration configuration)
        {
            var stringConnection = GetConnectionString(configuration);

            services.AddDbContext<OrganizerDbContext>(builder =>
            {
                builder.UseNpgsql(stringConnection,
                    options =>
                    {
                        options.MigrationsAssembly(MigrationsAssembly);
                        options.SetPostgresVersion(new Version(9, 5));
                        options.MigrationsHistoryTable(MigrationsHistoryTable, OrganizerDbContext.Schema);
                    });

#if DEBUG
                builder.EnableDetailedErrors();
                builder.EnableSensitiveDataLogging();
#endif
            });
        }

        public static void AddOrganizerDataProviders(this IServiceCollection services)
        {
            services.AddTransient<IInsertTeam, PostgresProvider>();
            services.AddTransient<IValidateTeamName, PostgresProvider>();
        }

        private static string GetConnectionString(IConfiguration configuration)
        {
            var builderConnection = new NpgsqlConnectionStringBuilder
            {
                Host = configuration.GetValue<string>(EnvHost),
                Username = configuration.GetValue<string>(EnvUsername),
                Password = configuration.GetValue<string>(EnvPassword),
                Database = configuration.GetValue<string>(EnvDatabase),
                Port = configuration.GetValue<int>(EnvPort)
            };

            return builderConnection.ToString();
        }
    }
}