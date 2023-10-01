using Service.Common.Models;

namespace Service.Common.Interfaces.Infrastructure
{
    public interface ITokenService
    {
        string GetToken(AccountModel user);
    }
}