using Microsoft.AspNetCore.Mvc;
using NinaApp.Core.DTO;
using NinaApp.Core.ServiceContracts;

namespace NinaApp.API.Controllers
{
  [ApiController]
  [Route("api/users")]
  public class UsersController: BaseApiController
  {
    private readonly IUsersService _usersService;
    public UsersController(IUsersService usersService) {
      _usersService = usersService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLogin userLogin)
    {
      var loginResult = await _usersService.AuthenticateUser(userLogin);

      return HandleResult(loginResult);
    }


    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] Pagination pagination)
    {
      var usersResult = await _usersService.GetUsers(pagination.Page, pagination.PageSize);

      return HandlePagedResult(usersResult);
    }

    [HttpGet("{userID:int}")]
    public async Task<IActionResult> GetUserByID(int userID)
    {
      var userResult = await _usersService.GetUserByID(userID);

      return HandleResult(userResult);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(UserCreation user)
    {
      var creationResult = await _usersService.CreateUser(user);

      return HandleResult(creationResult);
    }

    [HttpPut("{userID:int}")]
    public async Task<IActionResult> UpdateUser(int userID, UserUpdation user)
    {
      var updationResult = await _usersService.UpdateUser(userID, user);

      return HandleResult(updationResult);
    }

    [HttpDelete("{userID:int}")]
    public async Task<IActionResult> DeleteUser(int userID)
    {
      var deletionResult = await _usersService.DeleteUser(userID);

      return HandleResult(deletionResult);
    }
  }
}
