using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TlpArchitectureCore.Extensions;
public static class ServiceCollectionMongoDbExtensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IMongoClient>(provider => new MongoClient(connectionString));
        services.AddScoped<IMongoDatabase>(provider =>
        {
            var client = provider.GetRequiredService<IMongoClient>();
            return client.GetDatabase("tlp");
        });

        return services;
    }

    public static IServiceCollection AddMongoDb(this IServiceCollection services, Action<MongoClientSettings>? settingsAction = null)
    {
        services.AddScoped<IMongoClient>(provider =>
        {
            var settings = new MongoClientSettings();
            settingsAction?.Invoke(settings);
            return new MongoClient(settings);
        });

        services.AddScoped<IMongoDatabase>(provider =>
        {
            var client = provider.GetRequiredService<IMongoClient>();
            return client.GetDatabase("tlp");
        });

        return services;
    }
}
