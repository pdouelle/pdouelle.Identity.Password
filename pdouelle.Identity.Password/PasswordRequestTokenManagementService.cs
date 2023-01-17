using IdentityModel.Client;
using Microsoft.Extensions.Caching.Distributed;

namespace pdouelle.Identity.Password;

public sealed class PasswordRequestTokenManagementService : IPasswordRequestTokenManagementService
{
    private readonly HttpClient _httpClient;
    private readonly IDistributedCache _cache;

    public PasswordRequestTokenManagementService(
        HttpClient httpClient,
        IDistributedCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<string> GetAccessTokenAsync(
        string clientName,
        PasswordTokenRequest passwordTokenRequest,
        bool forceRenewal,
        CancellationToken cancellationToken)
    {
        if (forceRenewal is false)
        {
            var accessToken = await _cache.GetStringAsync(clientName, cancellationToken);
            if (accessToken is not null)
            {
                return accessToken;
            }
        }

        TokenResponse tokenResponse = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest, cancellationToken);

        await _cache.SetStringAsync(clientName, tokenResponse.AccessToken, cancellationToken);

        return tokenResponse.AccessToken;
    }
}