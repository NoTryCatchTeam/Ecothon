using Ecothon.Api.Definitions.Constants;
using Ecothon.Api.Definitions.Entities;
using Ecothon.Api.Definitions.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ecothon.Api.Dal;

public class PostgreSqlDbContext : IdentityDbContext<AppIdentityUser, IdentityRole<int>, int>, IDbContext
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly string _connectionString;

    public PostgreSqlDbContext(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
        _connectionString = string.Format(
            configuration.GetValue<string>(ConfigurationConstants.AppSettings.Db.CONNECTION_STRING),
            configuration.GetValue<string>(ConfigurationConstants.Secrets.Db.DB_USERNAME),
            configuration.GetValue<string>(ConfigurationConstants.Secrets.Db.DB_PASSWORD),
            configuration.GetValue<string>(ConfigurationConstants.Secrets.Db.DB_HOST),
            configuration.GetValue<string>(ConfigurationConstants.Secrets.Db.DB_DB_NAME));
    }

    public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options
            .UseNpgsql(_connectionString)
            .LogTo(Console.WriteLine, [RelationalEventId.CommandExecuting]);

        if (_hostingEnvironment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppIdentityUser>()
            .HasMany(x => x.RefreshTokens)
            .WithOne(x => x.User);
    }
}
