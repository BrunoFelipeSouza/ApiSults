using ApiSults.Domain.Shared;

namespace ApiSults.Domain.ConfigurationModule;

public class Configuration : Entity
{
    public string? Key { get; protected set; }
    public int AutomaticAtualizationIntervalInMinutes { get; set; }
    public bool AutomaticAtualizationEnabled { get; set; }
    public DateTime LastAtualization { get; set; }

    public Configuration() { }

    public void ConfigureKey(string key)
    {
        Key = key;
    }

    public void Atualize()
    {
        LastAtualization = DateTime.Now;
    }
}
