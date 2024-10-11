using System.Security.Claims;
using Blazored.LocalStorage;
using Ecothon.Web.Definitions.Contants;
using Ecothon.Web.Definitions.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Ecothon.Web.Services;

public class JwtAuthenticationStateProviderMock : AuthenticationStateProvider, IAuthService
{
    private readonly ILocalStorageService _localStorageService;

    public JwtAuthenticationStateProviderMock(
        ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async Task<bool> SignInAsync(string email, string password)
    {
        await _localStorageService.RemoveItemAsync(LocalStorageConstants.ACCESS_TOKEN);
        await _localStorageService.RemoveItemAsync(LocalStorageConstants.REFRESH_TOKEN);

        await _localStorageService.SetItemAsStringAsync(LocalStorageConstants.ACCESS_TOKEN, "access_123");
        await _localStorageService.SetItemAsStringAsync(LocalStorageConstants.REFRESH_TOKEN, "refresh_123");

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

        return true;
    }

    public async Task SignOutAsync()
    {
        await _localStorageService.RemoveItemAsync(LocalStorageConstants.USER_INFO);
        await _localStorageService.RemoveItemAsync(LocalStorageConstants.ACCESS_TOKEN);
        await _localStorageService.RemoveItemAsync(LocalStorageConstants.REFRESH_TOKEN);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        const string EMAIL = "user@notrycatch.team";

        var identity = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, EMAIL),
                ],
            JwtBearerConstants.AUTHENTICATION_SCHEME);

        await _localStorageService.RemoveItemAsync(LocalStorageConstants.USER_INFO);
        await _localStorageService.SetItemAsync(
            LocalStorageConstants.USER_INFO,
            new LocalStorageUser
            {
                Email = EMAIL,
            });

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }
}
