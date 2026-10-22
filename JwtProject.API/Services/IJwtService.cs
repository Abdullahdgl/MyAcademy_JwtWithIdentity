using JwtProject.API.Entites;

namespace JwtProject.API.Services
{
	public interface IJwtService
	{
		 Task<string> GenerateTokenAsync(AppUser user);
	}
}
