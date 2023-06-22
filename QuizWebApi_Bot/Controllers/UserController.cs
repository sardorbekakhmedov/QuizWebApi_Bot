using Microsoft.AspNetCore.Mvc;
using QuizWebApi_Bot.Interfaces;

namespace QuizWebApi_Bot.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }



    [HttpGet("get_users")]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await _userRepository.GetAllUsersAsync());
    }
}