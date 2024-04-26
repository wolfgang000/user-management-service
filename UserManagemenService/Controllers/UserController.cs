using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UserManagemenService.DAL;
using UserManagemenService.Models;

namespace UserManagemenService.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger,IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet(Name = "GetUsers")]
    public async Task<IEnumerable<User>> Get()
    {
        var users = await _userService.GetUsers();
        return users;
    }

    [HttpGet("{id}", Name = "GetUserById")]
    [ProducesResponseType<User>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(long id)
    {
        var user = await _userService.GetById(id);
        return user == null ? NotFound() : Ok(user);
    }
}
