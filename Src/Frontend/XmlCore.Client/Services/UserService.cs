using XmlCore.Shared.Entities;
using XmlCore.Shared.Interface;

namespace XmlCore.Client.Services;

public class UserService : IUserInterface<User>
{
    private readonly HttpClient _httpClient;

    public UserService(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("XmlCoreAPI");
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var allUsers = await _httpClient.GetFromJsonAsync<IEnumerable<User>>("api/user/users");
        return await Task.FromResult(allUsers);
    }

    public Task<User> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task CreateAsync(User entity)
    {
        var response = await _httpClient.PostAsJsonAsync("api/user", entity);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to add User");
        }
    }

    public Task<User> UpdateAsync(User entity, int id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}