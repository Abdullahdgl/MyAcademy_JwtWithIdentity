using JwtProject.Web.DTOs.Categories;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace JwtProject.Web.Services.Categories
{
	public class CategoryService(HttpClient _client) : ICategoryService
	{
		public async Task<List<ResultCategoryDto>> GetAllAsync()
		{
			
			var response = await _client.GetFromJsonAsync<List<ResultCategoryDto>>("categories");
			return response;
		}
	}
}
