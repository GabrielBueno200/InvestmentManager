using System.ComponentModel.DataAnnotations;

namespace Api.Auth.Infrastructure.Settings;

public class AesSettings
{
    [Required]
    public string Key { get; set; }

    [Required]
    public string Iv { get; set; }
}
