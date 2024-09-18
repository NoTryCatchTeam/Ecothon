using Ecothon.Dtos.Requests;
using Ecothon.Dtos.Responses;

namespace Ecothon.Api.Services;

public interface IAuthService
{
    Task<UserItemResponse> SignUpAsync(CreateUserRequest data);

    Task<AuthSuccessResponse> SignInBasicAsync(SignInBasicRequest data);

    Task<AuthSuccessResponse> RefreshAsync(RefreshTokensRequest data);
}
