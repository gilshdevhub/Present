namespace Core.Config;

public class CacheConfig
{
    public const string CacheSettings = "CacheSettings";
    public int AbsoluteExpiration { get; set; }
    public int SlidingExpiration { get; set; }
}
