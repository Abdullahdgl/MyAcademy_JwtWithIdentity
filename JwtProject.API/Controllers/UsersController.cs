using JwtProject.API.Dtos.UserDtos;
using JwtProject.API.Entites;
using JwtProject.API.Options;
using JwtProject.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JwtProject.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController(UserManager<AppUser> _userManager,
		IJwtService _jwtService, IOptions<JwtOptions> jwtOptions) : ControllerBase
	{

		private readonly JwtOptions _jwtOptions = jwtOptions.Value;

		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterUserDto model)
		{
			var user = new AppUser
			{
				Email = model.Email,
				UserName = model.UserName,
				FirstName = model.FirstName,
				LastName = model.LastName

			};

			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}
			return Ok("Kullanıcı Kaydı Başarılı");
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginUserDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.UserNameOrEmail);
			if (user is null)
			{
				user = await _userManager.FindByNameAsync(model.UserNameOrEmail);
				if (user is null)
				{
					return BadRequest("Böyle bir kullanıcı bulunamadı");
				}
			}
			
			var result = await _userManager.CheckPasswordAsync(user, model.Password);
			
			if(!result)
			{
				return BadRequest("Email/Kullanıcı adı veya şifre hatalı");
			}

			var token = await _jwtService.GenerateTokenAsync(user);
			return Ok(new { Token = token, ExpireTime=DateTime.Now.AddMinutes(_jwtOptions.ExpireInMinutes) });
		}

		
	}
}
