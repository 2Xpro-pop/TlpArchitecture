using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TlpArchitectureCore.Extensions;
public static class ServiceCollectionMongoDbExtensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, string connectionString, Action<MongoClientSettings>? settingsAction = null)
    {
        services.AddScoped<IMongoClient>(provider =>
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settingsAction?.Invoke(settings);

            settings.LoggingSettings = new(provider.GetRequiredService<ILoggerFactory>());

            var client = new MongoClient(settings);

            return client;
        });
        services.AddScoped<IMongoDatabase>(provider =>
        {
            var client = provider.GetRequiredService<IMongoClient>();
            return client.GetDatabase("tlp");
        });

        return services;
    }
}

