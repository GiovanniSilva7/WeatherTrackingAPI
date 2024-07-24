using FirebaseAdmin.Auth.Hash;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IUserRepository _userRepository;

	public AuthController(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] RegisterModel model)
	{
		var user = new User
		{
            Id = Guid.NewGuid().ToString(),
			Username = model.Username,
			Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
			Email = model.Email,
			PhotoUrl = model.PhotoUrl
		};

		await _userRepository.CreateUserAsync(user);
		return Ok();
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userRepository.GetByUsernameAsync(model.Username);
		if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
		{
			return Unauthorized();
		}

		// Gerar token JWT e retornar
		var token = GenerateTokenJWT(user);
		return Ok(token);
	}

    private string GenerateTokenJWT(User user)
    {
        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes("your_secret_key");
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Name, user.Username)
			}),
			Expires = DateTime.UtcNow.AddDays(7),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};
		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
    }
}
