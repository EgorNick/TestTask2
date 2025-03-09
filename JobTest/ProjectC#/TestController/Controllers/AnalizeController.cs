using Microsoft.AspNetCore.Mvc;
using TestController.Models;
using TestController.Services;

namespace TestController.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalizeController : ControllerBase
{
    private readonly IUserService _userService;

    public AnalizeController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public IActionResult ProcessUsers([FromBody] List<User> users)
    {
        if (users == null || !users.Any())
        {
            return BadRequest("Ошибка: Список пользователей пуст");
        }

        var result = _userService.ProcessUsers(users);
        return Ok(result);
    }
}