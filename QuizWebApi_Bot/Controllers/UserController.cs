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


    [HttpPost]
    public async Task<IActionResult> AddUserAsync(long userChatId, string? userName)
    {
        return Ok(await _userRepository.AddUserAsync(userChatId, userName));
    }

    [HttpGet("get_users")]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        return Ok(await _userRepository.GetAllUsersAsync());
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUserAsync(long userChatId)
    {
        await _userRepository.DeleteUserAsync(userChatId);
        return Ok();
    }
}