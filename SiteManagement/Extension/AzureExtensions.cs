﻿using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Infrastructure.Services.Azure;
using System.Text.Json;

namespace SiteManagement.Extension;

public static class AzureExtensions
{
    public static IConfigurationBuilder AddAzureConfigurationServices(this IConfigurationBuilder builder, IHostEnvironment environment)
    {
               IConfiguration config = builder.Build();
        if (environment.IsDevelopment())
        {
            AddCachedConfiguration(builder, config, environment);
            return builder;
        }
        return AddAzureConfigurationServicesInternal(builder, config, environment);
    }

    private static IConfigurationBuilder AddAzureConfigurationServicesInternal(IConfigurationBuilder builder,
        IConfiguration currentConfig, IHostEnvironment environment)
    {
        string keyVaultEndpoint = currentConfig["AzureKeyVaultEndpoint"];
        string appTitle = currentConfig["APP_TITLE"];
        SecretClient secretClient = new(new Uri(keyVaultEndpoint), new ManagedIdentityCredential());
        _ = builder.AddAzureKeyVault(secretClient, new PrefixKeyVaultSecretManager($"{environment.EnvironmentName}--{appTitle}"));

        return builder;
    }

    private static void AddCachedConfiguration(IConfigurationBuilder config, IConfiguration currentConfig, IHostEnvironment environment)
    {
        UpdateCacheConfiguration($"appsettings.{environment}.json", currentConfig, environment, config);
        string path = $"appsettings.{environment}.json";
        string filename = $"appsettings.{environment}.json";

        if (!System.IO.File.Exists(filename) || System.IO.File.GetLastAccessTimeUtc(filename).AddHours(12) < DateTime.UtcNow)
        {
            UpdateCacheConfiguration(filename, currentConfig, environment, config);
        }
    }
    private static void UpdateCacheConfiguration(string filename, IConfiguration currentConfig, IHostEnvironment environment, IConfigurationBuilder config)
    {
        ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        IConfigurationRoot azureConfig = configurationBuilder.Build();
        string jsonString = JsonSerializer.Serialize(
            azureConfig.AsEnumerable().ToDictionary(a => a.Key, a => a.Value),
            options: new JsonSerializerOptions()
            {
                WriteIndented = true
            }
            );
        var keyVaultPairs = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
        config.AddInMemoryCollection(keyVaultPairs);
    }
}
