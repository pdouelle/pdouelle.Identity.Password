using IdentityModel.Client;

namespace pdouelle.Identity.Password;

public interface IPasswordRequestTokenManagementService
{
    public Task<string> GetAccessTokenAsync(string clientName, PasswordTokenRequest passwordTokenRequest, bool forceRenewal, CancellationToken cancellationToken);
}