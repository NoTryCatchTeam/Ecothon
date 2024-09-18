using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using Ecothon.Dtos.Requests;
using Ecothon.Dtos.Responses;
using Ecothon.Web.Definitions.Contants;
using Ecothon.Web.Definitions.LocalStorage;
using Ecothon.Web.Helpers;
using Microsoft.AspNetCore.Components.Authorization;

namespace Ecothon.Web.Services;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider, IAuthService
{
    private readonly ILocalStorageService _localStorageService;
    private readonly ILogger<JwtAuthenticationStateProvider> _logger;
    private readonly HttpClient _httpClient;

    public JwtAuthenticationStateProvider(
        IHttpClientFactory httpClientFactory,
        ILocalStorageService localStorageService,
        ILogger<JwtAuthenticationStateProvider> logger)
    {
        _localStorageService = localStorageService;
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient(HttpClientConstants.HttpClientNames.API);
    }

    public async Task<bool> SignInAsync(string email, string password)
    {
        try
        {
            var signInResponse = await _httpClient.PostAsJsonAsync("auth/sign-in/basic", new SignInBasicRequest { Email = email, Password = password });

            signInResponse.EnsureSuccessStatusCode();

            var authResponse = JsonHelpers.Deserialize<AuthSuccessResponse>(await signInResponse.Content.ReadAsStringAsync());

            await _localStorageService.RemoveItemAsync(LocalStorageConstants.ACCESS_TOKEN);
            await _localStorageService.RemoveItemAsync(LocalStorageConstants.REFRESH_TOKEN);

            await _localStorageService.SetItemAsStringAsync(LocalStorageConstants.ACCESS_TOKEN, authResponse.AccessToken);
            await _localStorageService.SetItemAsStringAsync(LocalStorageConstants.REFRESH_TOKEN, authResponse.RefreshToken);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return false;
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
        var identity = new ClaimsIdentity();

        try
        {
            var userResponse = await _httpClient.GetAsync("user/info");

            if (userResponse.StatusCode == HttpStatusCode.Unauthorized &&
                !await RefreshTokensAsync())
            {
                await SignOutAsync();

                throw new Exception("Failed to get user info and refresh tokens");
            }

            var user = JsonHelpers.Deserialize<UserItemResponse>(await userResponse.Content.ReadAsStringAsync());

            identity = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Email, user.Email),
                ],
                JwtBearerConstants.AUTHENTICATION_SCHEME);

            await _localStorageService.RemoveItemAsync(LocalStorageConstants.USER_INFO);
            await _localStorageService.SetItemAsync(
                LocalStorageConstants.USER_INFO,
                new LocalStorageUser
                {
                    Email = user.Email,
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    private async Task<bool> RefreshTokensAsync()
    {
        try
        {
            var localUser = await _localStorageService.GetItemAsync<LocalStorageUser>(LocalStorageConstants.USER_INFO);
            var accessToken = await _localStorageService.GetItemAsStringAsync(LocalStorageConstants.ACCESS_TOKEN);
            var refreshToken = await _localStorageService.GetItemAsStringAsync(LocalStorageConstants.REFRESH_TOKEN);

            if (localUser == null || accessToken == null || refreshToken == null)
            {
                return false;
            }

            var signInResponse = await _httpClient.PostAsJsonAsync(
                "auth/refresh",
                new RefreshTokensRequest { Email = localUser.Email, AccessToken = accessToken, RefreshToken = refreshToken });

            signInResponse.EnsureSuccessStatusCode();

            var authResponse = JsonHelpers.Deserialize<AuthSuccessResponse>(await signInResponse.Content.ReadAsStringAsync());

            await _localStorageService.RemoveItemAsync(LocalStorageConstants.ACCESS_TOKEN);
            await _localStorageService.RemoveItemAsync(LocalStorageConstants.REFRESH_TOKEN);

            await _localStorageService.SetItemAsStringAsync(LocalStorageConstants.ACCESS_TOKEN, authResponse.AccessToken);
            await _localStorageService.SetItemAsStringAsync(LocalStorageConstants.REFRESH_TOKEN, authResponse.RefreshToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return false;
    }
}
