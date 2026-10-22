using Microsoft.AspNetCore.Identity;

namespace JwtProject.API.Entites
{
	public class AppUser : IdentityUser<int>
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }

	}
}
