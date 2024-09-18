using Ecothon.Api.Definitions.Identity;
using Ecothon.Dtos.Responses;
using Microsoft.AspNetCore.Identity;

namespace Ecothon.Api.Services;

public class UsersService : IUsersService
{
    private readonly UserManager<AppIdentityUser> _userManager;

    public UsersService(SignInManager<AppIdentityUser> signInManager)
    {
        _userManager = signInManager.UserManager;
    }

    public async Task<UserItemResponse> GetByEmailAsync(string email)
    {
        var identity = await _userManager.FindByEmailAsync(email);

        return new UserItemResponse
        {
            Id = identity.Id,
            Email = identity.Email,
            Username = identity.UserName,
        };
    }
}
