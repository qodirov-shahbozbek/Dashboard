using System.Security.Claims;

namespace Dashboard.Application.Abstractions
{
    public interface ITokenService
    {
        string GetToken(Claim[] claims);
    }
}