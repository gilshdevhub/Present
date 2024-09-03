using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Core.Entities.KeyVault;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class AzureKeyVaultService : IAzureKeyVaultService
{
    private readonly SecretClient _client;
    private readonly IConfiguration _configuration;

    public AzureKeyVaultService(IConfiguration configuration)
    {
        _configuration = configuration;

        var keyVaultUrl = _configuration.GetValue<string>("AzureKeyVaultEndpoint");

        _client = new SecretClient(vaultUri: new Uri(keyVaultUrl), credential: new DefaultAzureCredential());
                         }

    public async Task<string> GetSecret(string secretName)
    {

        var secret = await _client.GetSecretAsync(secretName);

        return secret.Value.Value;
    }

    public async Task<List<KeyPairSettings>> GetSecrets()
    {
        List<KeyVaultSecret> res = new();
        AsyncPageable<SecretProperties> secreteProps = _client.GetPropertiesOfSecretsAsync();
        await foreach (var secretProperties in secreteProps)
        {
            KeyVaultSecret secretWithValue = await _client.GetSecretAsync(secretProperties.Name, secretProperties.Version);
            res.Add(secretWithValue);
        }
        List<KeyPairSettings> reslist = new();
        foreach (var item in res)
        {
            KeyPairSettings _attr = new()
            {
                Key = item.Name,
                Val = item.Value
            };
            reslist.Add(_attr);
        }
        return reslist;
    }

}
