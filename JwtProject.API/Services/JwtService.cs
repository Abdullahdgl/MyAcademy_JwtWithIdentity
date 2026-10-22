using JwtProject.API.Entites;
using JwtProject.API.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtProject.API.Services
{
	public class JwtService : IJwtService
	{
				private readonly JwtOptions _jwtOptions;
		public JwtService(IOptions<JwtOptions> jwtOptions)
		{
			_jwtOptions = jwtOptions.Value;

		}

		public async Task<string> GenerateTokenAsync(AppUser user)
		{
			var claims = new List<Claim>
			{
				
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Email, user.Email)
			};


			//Kullanıcıya ait rolleri çekip claims listesine ekleyelim....


			SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(_jwtOptions.Key));

			JwtSecurityToken jwtSecurityToken = new(
				issuer: _jwtOptions.Issuer,
				audience: _jwtOptions.Audience,
				claims: claims,
				notBefore: DateTime.UtcNow,
				expires: DateTime.Now.AddMinutes(_jwtOptions.ExpireInMinutes),
				signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
			);

			var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

			return token;
		}
	}
}
