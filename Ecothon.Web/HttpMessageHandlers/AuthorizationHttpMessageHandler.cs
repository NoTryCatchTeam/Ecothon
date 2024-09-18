using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Ecothon.Web.Definitions.Contants;

namespace Ecothon.Web.HttpMessageHandlers;

public class AuthorizationHttpMessageHandler : HttpClientHandler
{
    private readonly ILocalStorageService _localStorageService;

    public AuthorizationHttpMessageHandler(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _localStorageService.GetItemAsStringAsync(LocalStorageConstants.ACCESS_TOKEN, cancellationToken));

        return await base.SendAsync(request, cancellationToken);
    }
}
