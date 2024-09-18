using Ecothon.Api.Definitions.Auth;

namespace Ecothon.Api.Services;

public interface IAuthTokensService
{
    AuthToken CreateAccessToken(string email);

    AuthToken CreateRefreshToken();

    bool ValidateAccessToken(string accessToken);
}
