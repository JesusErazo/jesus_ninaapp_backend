using Microsoft.AspNetCore.Mvc;
using NinaApp.Core.DTO;
using NinaApp.Core.ServiceContracts;

namespace NinaApp.API.Controllers
{
  [ApiController]
  [Route("api/auth")]
  public class AuthController: BaseApiController
  {
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService) {
      _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
      var loginResult = await _authService.Login(request);
      return HandleResult(loginResult);
    }
  }
}
