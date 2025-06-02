using XmlCore.Shared.DTO;
using XmlCore.Shared.Interface;

namespace XmlCore.Client.Services;

public class ArticleInOrderService : IArticlesInOrderInterface<ArticlesInOrderDTO>
{


    private readonly HttpClient _httpClient;

    public ArticleInOrderService(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("XmlCoreAPI");
    }
    public async Task<IEnumerable<ArticlesInOrderDTO>> GetAllAsync()
    {
        var AiOList =await _httpClient.GetFromJsonAsync<IEnumerable<ArticlesInOrderDTO>>("api/AiO");
        return await Task.FromResult(AiOList.ToList());
    }

    public Task<ArticlesInOrderDTO> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}