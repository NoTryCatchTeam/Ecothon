using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Ecothon.Web.Definitions.Contants;

namespace Ecothon.Web.HttpMessageHandlers;

public class AuthorizationHttpMessageHandler : HttpClientHandler
{
    private readonly Func<Task<string>> _accessTokenProvider;

    public AuthorizationHttpMessageHandler(Func<Task<string>> accessTokenProvider)
    {
        _accessTokenProvider = accessTokenProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _accessTokenProvider());

        return await base.SendAsync(request, cancellationToken);
    }
}
