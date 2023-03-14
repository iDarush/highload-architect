using Architect.Web.BLL.Services.Interfaces;
using Architect.Web.Dto;
using Architect.Web.Dto.Requests;
using Architect.Web.Dto.Responses;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Architect.Web.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public UserController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserRegisterResponse>> Register(
        [FromBody] UserRegisterRequest request,
        CancellationToken cancellationToken)
    {
        var userId = await _authService.Register(request, cancellationToken);
        return Ok(new UserRegisterResponse(userId.ToString()));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> Get(
        Guid id,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetUser(id, cancellationToken);
        if (user == null)
        {
            return NotFound(NotFoundResponse.User(id));
        }

        return Ok(user.Adapt<UserResponse>());
    }
}
