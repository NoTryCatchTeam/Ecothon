using System.ComponentModel.DataAnnotations;
using Ecothon.Api.Definitions.Identity;

namespace Ecothon.Api.Definitions.Entities;

public class UserRefreshToken
{
    [Key]
    public int Id { get; set; }

    [Required]
    public AppIdentityUser User { get; set; }

    [Required]
    public string Token { get; set; }

    [Required]
    public DateTimeOffset ExpiresAt { get; set; }
}
