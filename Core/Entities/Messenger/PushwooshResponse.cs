namespace Core.Entities.KeyVault;

public class KeyPairSettings
{
    public string Key { get; set; }
    public string Val { get; set; }
}

public class AllSettings
{
    public KeyPairSettings attr { get; set; }
}
