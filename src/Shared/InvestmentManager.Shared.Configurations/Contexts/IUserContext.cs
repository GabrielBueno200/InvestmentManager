using InvestmentManager.Shared.Utilities.Abstractions;

namespace InvestmentManager.Shared.Configurations.Contexts;

public interface IUserContext
{
    string? GetToken();
    SummarizedUser GetLoggedUser();
}
