using System.ComponentModel.DataAnnotations;

namespace InvestmentManager.Shared.Configurations.Settings;

public class JwtSettings
{   
    [Required]
    public string SecretKey { get; set; }
    
    [Required]
    public string Issuer { get; set; }
    
    [Required]
    public int ExpirationInMinutes { get; set; }
}
