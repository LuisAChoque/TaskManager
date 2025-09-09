using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User user)
    {
        var foundUser = _context.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);

        if (foundUser == null)
        {
            return Unauthorized();
        }

        HttpContext.Session.SetInt32("UserId", foundUser.Id);
        return Ok(new { message = "Login exitoso", userId = foundUser.Id });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Ok(new { message = "Sesión cerrada" });
    }

    [HttpGet("validate-session")]
    public IActionResult ValidateSession()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
        {
            return Unauthorized(new { message = "No hay sesión activa" });
        }

        return Ok(new { message = "Sesión activa", userId = userId });
    }

}
