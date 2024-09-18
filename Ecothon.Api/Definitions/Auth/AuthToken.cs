namespace Ecothon.Api.Definitions.Auth;

public struct AuthToken
{
    public AuthToken(string token, DateTimeOffset expiresAt)
    {
        Token = token;
        ExpiresAt = expiresAt;
    }

    public string Token { get; }

    public DateTimeOffset ExpiresAt { get; }
}
