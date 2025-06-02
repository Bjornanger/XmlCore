using XmlCore.Shared.Entities;
using XmlCore.Shared.Interface;

namespace XmlCore.Client.Services;

public class CategoryService : ICategoryInterface<Category>
{
    private readonly HttpClient _httpClient;

    public CategoryService(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("XmlCoreAPI");
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        var allCategories = await _httpClient.GetFromJsonAsync<IEnumerable<Category>>("api/category/categories");
        return await Task.FromResult(allCategories);
    }

    public async Task<Category> GetByIdAsync(int id)
    {
        var specCategory = await _httpClient.GetFromJsonAsync<Category>($"api/category/{id}");
        return await Task.FromResult(specCategory);
    }

    public async Task CreateAsync(Category entity)
    {
        var response = await _httpClient.PostAsJsonAsync("api/category", entity);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to add Category.");
        }
    }

    public async Task<Category> UpdateAsync(Category entity, int id)
    {
        await _httpClient.PutAsJsonAsync($"api/category{id}", entity);
        return await Task.FromResult(entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteFromJsonAsync<Category>($"api/category{id}");

    }
}