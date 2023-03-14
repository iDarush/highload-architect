using Architect.Web.BLL.Services.Interfaces;
using Architect.Web.Dto.Requests;
using Architect.Web.Dto.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Architect.Web.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserLoginResponse>> Login(
        [FromBody] UserLoginRequest request,
        CancellationToken cancellationToken)
    {
        var token = await _authService.Login(
            request.GetIdAsGuid(),
            request.Password,
            cancellationToken);

        return Ok(new UserLoginResponse(token));
    }
}
