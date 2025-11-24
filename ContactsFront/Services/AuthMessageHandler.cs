using System.Net.Http.Headers;

namespace ContactsFront.Services;

public class AuthMessageHandler : DelegatingHandler
{
    private readonly AuthService _auth;

    public AuthMessageHandler(AuthService auth)
    {
        _auth = auth;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _auth.GetTokenSafeAsync();
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
