using Ecothon.Dtos.Responses;

namespace Ecothon.Api.Services;

public interface IUsersService
{
    Task<UserItemResponse> GetByEmailAsync(string email);
}
