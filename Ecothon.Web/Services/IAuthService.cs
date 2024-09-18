namespace Ecothon.Web.Services;

public interface IAuthService
{
    Task<bool> SignInAsync(string email, string password);

    Task SignOutAsync();
}
