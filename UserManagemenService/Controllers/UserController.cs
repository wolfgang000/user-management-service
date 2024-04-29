using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UserManagemenService.DAL;
using UserManagemenService.DTO;
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

    [HttpPost(Name = "CreateUser")]
    [ProducesResponseType<User>(StatusCodes.Status201Created)]
    public async Task<ActionResult> Insert([FromBody] CreateUserDto userRequest)
    {
        var validator = new CreateUserDtoValidator();
        var validationResult = validator.Validate(userRequest);
        if (!validationResult.IsValid) 
        {
            return BadRequest(validationResult.ToDictionary());
        }

        var user = await _userService.Insert(userRequest.GetUser());
        return Created($"api/users/{user.Id}", user);
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


    [HttpDelete("{id}", Name = "DeleteUserById")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult>DeleteById(long id)
    {
        var user = await _userService.GetById(id);
        if(user != null) {
            await _userService.Delete(user);
            return NoContent();
        } else {
            return NotFound();
        }
    }


    [HttpPatch("{id}", Name = "UpdateUserById")]
    [ProducesResponseType<User>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult>UpdateById(long id, [FromBody] User userRequest)
    {
        var user = await _userService.GetById(id);
        if(user != null) {
            user.CopyValues(userRequest);
            await _userService.Update(user);
            return Ok(user);
        } else {
            return NotFound();
        }
    }
}
