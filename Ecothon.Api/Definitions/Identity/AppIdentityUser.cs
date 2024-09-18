using Ecothon.Api.Definitions.Entities;
using Microsoft.AspNetCore.Identity;

namespace Ecothon.Api.Definitions.Identity;

public class AppIdentityUser : IdentityUser<int>
{
    public AppIdentityUser()
    {
    }

    public AppIdentityUser(string userName)
        : base(userName)
    {
    }

    public IEnumerable<UserRefreshToken> RefreshTokens { get; set; }
}
