using Ecothon.Api.Services;
using Ecothon.Dtos.Requests;
using Ecothon.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Ecothon.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost]
    [Route("sign-up")]
    [ProducesResponseType<UserItemResponse>(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> SignUpAsync([FromBody] CreateUserRequest data)
    {
        try
        {
            return Ok(await _authService.SignUpAsync(data));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Couldn't sign in user");
        }

        return BadRequest();
    }

    [HttpPost]
    [Route("sign-in/basic")]
    [ProducesResponseType<AuthSuccessResponse>(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> SignInViaBasicAsync([FromBody] SignInBasicRequest data)
    {
        try
        {
            return Ok(await _authService.SignInBasicAsync(data));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Couldn't sign in user");
        }

        return BadRequest();
    }
    
    [HttpPost]
    [Route("refresh")]
    [ProducesResponseType<AuthSuccessResponse>(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokensRequest data)
    {
        try
        {
            return Ok(await _authService.RefreshAsync(data));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Couldn't sign in user");
        }

        return BadRequest();
    }
}
