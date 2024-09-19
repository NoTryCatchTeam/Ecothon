using System.Security.Claims;
using System.Text;
using Ecothon.Api.Dal;
using Ecothon.Api.Definitions.Constants;
using Ecothon.Api.Definitions.Identity;
using Ecothon.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secrets.json", false, true);

if (!builder.Environment.IsProduction())
{
    builder.Configuration.AddJsonFile($"secrets.{builder.Environment.EnvironmentName}.json", false, true);
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    opt =>
    {
        opt.AddSecurityDefinition(
            JwtBearerDefaults.AuthenticationScheme,
            new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = HeaderNames.Authorization,
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = JwtBearerDefaults.AuthenticationScheme,
            });

        opt.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme,
                        },
                    },
                    []
                },
            });
    });

builder.Services
    .AddIdentity<AppIdentityUser, IdentityRole<int>>(
        opt =>
        {
            opt.Lockout.AllowedForNewUsers = false;

            opt.SignIn.RequireConfirmedAccount = false;
            opt.SignIn.RequireConfirmedEmail = false;
            opt.SignIn.RequireConfirmedPhoneNumber = false;

            opt.Password.RequireDigit = false;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequiredLength = 6;
            opt.Password.RequiredUniqueChars = 0;

            // options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            opt.User.RequireUniqueEmail = true;
        })
    .AddEntityFrameworkStores<PostgreSqlDbContext>()
    .AddApiEndpoints();

builder.Services.AddDbContext<IDbContext, PostgreSqlDbContext>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IAuthTokensService, AuthTokensService>();
builder.Services.AddTransient<IUsersService, UsersService>();

builder.Services
    .AddAuthentication(
        options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
    .AddJwtBearer(
        opt =>
        {
            opt.SaveToken = true;
            // We don't have https now
            opt.RequireHttpsMetadata = false; // builder.Environment.IsProduction();

            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration.GetValue<string>(ConfigurationConstants.Secrets.Jwt.ISSUER),
                ValidAudience = builder.Configuration.GetValue<string>(ConfigurationConstants.Secrets.Jwt.AUDIENCE),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>(ConfigurationConstants.Secrets.Jwt.SECRET))),
                ClockSkew = TimeSpan.Zero,
                NameClaimType = ClaimTypes.Email,
            };
        });

builder.Services.AddCors(
    options =>
    {
        // TODO Add real host using appsettings
        var origins = new List<string>
        {
            "http://localhost:5195",
            "https://localhost:7264",
            "http://localhost:5282",
            "https://localhost:7282",
            "http://45.142.36.65",
            "http://45.142.36.65:8080",
            "http://ecothon.notrycatch.team",
        };

        options.AddPolicy(
            "wasm",
            policy => policy
                .WithOrigins(origins.ToArray())
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("wasm");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
