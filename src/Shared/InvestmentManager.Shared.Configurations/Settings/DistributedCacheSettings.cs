using System.ComponentModel.DataAnnotations;

namespace InvestmentManager.Shared.Configurations.Settings;

public class DistributedCacheSettings
{
    [Required]
    public string ConnectionString { get; set; }
}
