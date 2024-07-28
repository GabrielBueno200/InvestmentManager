
using InvestmentManager.Shared.Utilities.Abstractions;
using InvestmentManager.Shared.Utilities.Helpers;

namespace Api.Auth.Domain.Entities;

[Collection("users")]
public class User : BaseEntity
{
    public string Username { get; set; } 
    public string Email { get; set; } 
    public string PasswordHash { get; set; } 
    public string Role { get; set; }
}
    