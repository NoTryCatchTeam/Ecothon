using Ecothon.Api.Dal;
using Ecothon.Api.Definitions.Entities;
using Ecothon.Api.Definitions.Identity;
using Ecothon.Dtos.Requests;
using Ecothon.Dtos.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecothon.Api.Services;

public class AuthService : IAuthService
{
    private readonly SignInManager<AppIdentityUser> _signInManager;
    private readonly IAuthTokensService _authTokensService;
    private readonly IDbContext _dbContext;
    private readonly UserManager<AppIdentityUser> _userManager;

    public AuthService(
        SignInManager<AppIdentityUser> signInManager,
        IAuthTokensService authTokensService,
        IDbContext dbContext)
    {
        _signInManager = signInManager;
        _authTokensService = authTokensService;
        _dbContext = dbContext;
        _userManager = signInManager.UserManager;
    }

    public async Task<UserItemResponse> SignUpAsync(CreateUserRequest data)
    {
        if (await _userManager.FindByEmailAsync(data.Email) is not null)
        {
            throw new Exception("Email already exists");
        }

        var result = await _userManager.CreateAsync(new AppIdentityUser(data.Email.Split("@")[0]) { Email = data.Email }, data.Password);

        if (!result.Succeeded)
        {
            throw new Exception($"Couldn't create user: {string.Join(",", result.Errors.Select(x => x.Description).ToList())}");
        }

        var newUser = await _userManager.FindByEmailAsync(data.Email);

        return new UserItemResponse
        {
            Id = newUser.Id,
            Email = newUser.Email,
            Username = newUser.UserName,
        };
    }

    public async Task<AuthSuccessResponse> SignInBasicAsync(SignInBasicRequest data)
    {
        if (await _userManager.FindByEmailAsync(data.Email) is not { } user)
        {
            throw new Exception("User not found");
        }

        if (!(await _signInManager.CheckPasswordSignInAsync(user, data.Password, false)).Succeeded)
        {
            throw new Exception("Couldn't sign in user: wrong password");
        }

        var refreshToken = _authTokensService.CreateRefreshToken();

        var userRefreshToken = new UserRefreshToken
        {
            User = user,
            Token = refreshToken.Token,
            ExpiresAt = refreshToken.ExpiresAt,
        };

        await _dbContext.UserRefreshTokens.AddAsync(userRefreshToken);
        await _dbContext.SaveChangesAsync();

        return new AuthSuccessResponse
        {
            AccessToken = _authTokensService.CreateAccessToken(user.Email).Token,
            RefreshToken = userRefreshToken.Token,
        };
    }

    public async Task<AuthSuccessResponse> RefreshAsync(RefreshTokensRequest data)
    {
        if (await _userManager.FindByEmailAsync(data.Email) is not { })
        {
            throw new Exception("User not found");
        }

        if (!_authTokensService.ValidateAccessToken(data.AccessToken))
        {
            throw new Exception("Invalid access token provided");
        }

        if (await _dbContext.UserRefreshTokens.FirstOrDefaultAsync(x => x.User.Email == data.Email && x.Token == data.RefreshToken) is not { } refreshTokenEntity)
        {
            throw new Exception("Invalid refresh token provided");
        }

        if (refreshTokenEntity.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            throw new Exception("Refresh token expired");
        }

        var refreshToken = _authTokensService.CreateRefreshToken();

        refreshTokenEntity.Token = refreshToken.Token;
        refreshTokenEntity.ExpiresAt = refreshToken.ExpiresAt;

        _dbContext.UserRefreshTokens.Update(refreshTokenEntity);
        await _dbContext.SaveChangesAsync();

        return new AuthSuccessResponse
        {
            AccessToken = _authTokensService.CreateAccessToken(data.Email).Token,
            RefreshToken = refreshTokenEntity.Token,
        };
    }
}
