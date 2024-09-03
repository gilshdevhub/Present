using Core.Entities.KeyVault;

namespace Core.Interfaces;

public interface IAzureKeyVaultService
{
    Task<string> GetSecret(string secretName);
    Task<List<KeyPairSettings>> GetSecrets();
}
