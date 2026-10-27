using JwtProject.Web.DTOs.UserDtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JwtProject.Web.Services.Users
{
	public class UserService(HttpClient client, IHttpContextAccessor _httpContextAccessor) : IUserService
	{
		// https://localhost:7034/api/
		public async Task<bool> LoginAsync(LoginUserDto loginUserDto)
		{
			var response = await client.PostAsJsonAsync("users/login", loginUserDto);

			if(!response.IsSuccessStatusCode)
			{
				return false;
			}

		   var result = await response.Content.ReadFromJsonAsync<TokenResponseDto>();

			var handler = new JwtSecurityTokenHandler();
			var token = handler.ReadJwtToken(result.Token);
			var claims = token.Claims.ToList();

			claims.Add(new Claim("Token", result.Token));

			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var autProperties = new AuthenticationProperties
			{
				ExpiresUtc = result.ExpireTime,
				IsPersistent = true //kalıcı olsun mu logout olana kadar

			};
			await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), autProperties);
			return true;

		}
	}
}
