using Ecothon.Api.Services;
using Ecothon.Dtos.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecothon.Api.Controllers;

[ApiController]
[Route("user")]
[Authorize]
public class UserController : Controller
{
    private readonly IUsersService _usersService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUsersService usersService, ILogger<UserController> logger)
    {
        _usersService = usersService;
        _logger = logger;
    }

    [HttpGet("info")]
    [ProducesResponseType<UserItemResponse>(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetAsync()
    {
        try
        {
            return Ok(await _usersService.GetByEmailAsync(User.Identity.Name));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Couldn't get user");

            return BadRequest();
        }
    }
}
