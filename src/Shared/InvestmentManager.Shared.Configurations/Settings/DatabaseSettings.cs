using System.ComponentModel.DataAnnotations;

namespace InvestmentManager.Shared.Configurations.Settings;

public class DatabaseSettings
{
    [Required]
    public string DatabaseName { get; set; }

    [Required]
    public string ConnectionString { get; set; }
}
