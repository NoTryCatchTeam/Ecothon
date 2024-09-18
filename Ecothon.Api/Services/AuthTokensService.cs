using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Ecothon.Api.Definitions.Auth;
using Ecothon.Api.Definitions.Constants;
using Microsoft.IdentityModel.Tokens;

namespace Ecothon.Api.Services;

public class AuthTokensService : IAuthTokensService
{
    private readonly IConfiguration _configuration;

    public AuthTokensService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AuthToken CreateAccessToken(string email)
    {
        var authClaims = new List<Claim>
        {
            new (ClaimTypes.Email, email),
        };

        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(int.Parse(_configuration.GetValue<string>(ConfigurationConstants.Secrets.Jwt.ACCESS_TOKEN_EXPIRES_IN_MINS)));

        var token = new JwtSecurityToken(
            issuer: _configuration.GetValue<string>(ConfigurationConstants.Secrets.Jwt.ISSUER),
            audience: _configuration.GetValue<string>(ConfigurationConstants.Secrets.Jwt.AUDIENCE),
            expires: expiresAt.UtcDateTime,
            claims: authClaims,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>(ConfigurationConstants.Secrets.Jwt.SECRET))),
                SecurityAlgorithms.HmacSha256)
        );

        return new AuthToken(new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }

    public AuthToken CreateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(int.Parse(_configuration.GetValue<string>(ConfigurationConstants.Secrets.Jwt.REFRESH_TOKEN_EXPIRES_IN_MINS)));

        return new AuthToken(Convert.ToBase64String(randomNumber), expiresAt.UtcDateTime);
    }

    public bool ValidateAccessToken(string accessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = _configuration.GetValue<string>(ConfigurationConstants.Secrets.Jwt.AUDIENCE),
            ValidIssuer = _configuration.GetValue<string>(ConfigurationConstants.Secrets.Jwt.ISSUER),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>(ConfigurationConstants.Secrets.Jwt.SECRET))),
            ValidateLifetime = false,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var validateTokenPrincipal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return validateTokenPrincipal != null;
    }
}
