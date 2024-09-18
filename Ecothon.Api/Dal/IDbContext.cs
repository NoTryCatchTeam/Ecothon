using Ecothon.Api.Definitions.Entities;
using Ecothon.Api.Definitions.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Ecothon.Api.Dal;

public interface IDbContext : IDisposable
{
    DbSet<AppIdentityUser> Users { get; set; }

    DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

    DatabaseFacade Database { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
