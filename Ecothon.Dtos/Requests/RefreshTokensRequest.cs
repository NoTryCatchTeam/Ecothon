namespace Ecothon.Dtos.Requests;

public class RefreshTokensRequest
{
    public string Email { get; set; }

    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }
}
