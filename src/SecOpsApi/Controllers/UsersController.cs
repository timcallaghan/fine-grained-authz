using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace SecOpsApi.Controllers;

[Authorize]
[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    [HttpGet("me")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(JwtPayload))]
    public IActionResult Me(CancellationToken ct)
    {
        var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(accessToken);
        return Ok(token.Payload);
    }
}