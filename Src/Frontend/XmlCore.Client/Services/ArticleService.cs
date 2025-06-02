using XmlCore.Shared.DTO;
using XmlCore.Shared.Interface;

namespace XmlCore.Client.Services;

public class ArticleService : IArticleInterface<ArticleDTO>
{
    private readonly HttpClient _httpClient;

    public ArticleService(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("XmlCoreAPI");
    }

    public async Task<IEnumerable<ArticleDTO>> GetAllAsync()
    {
        var response = await _httpClient.GetAsync("api/article/articles");
        if (response.IsSuccessStatusCode)
        {
            var allArticles = await response.Content.ReadFromJsonAsync<IEnumerable<ArticleDTO>>();
            return allArticles;
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            return [new ArticleDTO { ErrorMessage = error }];
        }
    }

    public async Task<ArticleDTO> GetByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/article/article/{id}");
        if (response.IsSuccessStatusCode)
        {
            var article = await response.Content.ReadFromJsonAsync<ArticleDTO>();
            return article;
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            return new ArticleDTO { ErrorMessage = error };
        }
    }

    public async Task<ArticleDTO> GetArticleByName(string articleName)
    {
        var response = await _httpClient.GetAsync($"api/article/articleName/{articleName}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ArticleDTO>();
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            return new ArticleDTO { ErrorMessage = error };
        }

    }


    public async Task<ArticleDTO> GetArticleByNumber(string articleNumber)
    {
        var response = await _httpClient.GetAsync($"api/article/{articleNumber}");

        if (response.IsSuccessStatusCode)
        {
            var article = await response.Content.ReadFromJsonAsync<ArticleDTO>();
            return article;
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            return new ArticleDTO { ErrorMessage = error };
        }
    }
    
     public async Task CreateAsync(ArticleDTO article)
    {
       var response = await _httpClient.PostAsJsonAsync("api/article", article);
        if (!response.IsSuccessStatusCode)
        {
            Results.BadRequest("No Article Created.");
        }
    }

    public Task<IEnumerable<ArticleDTO>> GetByCategoryAsync(int Category)
    {
        throw new NotImplementedException();
    }

    public async Task<ArticleDTO> UpdateAsync(ArticleDTO article, string articleNumber)
    {
        await _httpClient.PutAsJsonAsync($"api/article/{articleNumber}", article);
        return await Task.FromResult(article);
    }

    public async Task<ArticleDTO> SwitchStatusOnArticle(ArticleDTO article, string articleNumber)
    {
        await _httpClient.PutAsJsonAsync($"api/article/status/{articleNumber}", article);
        return await Task.FromResult(article);
    }

    public async Task DeleteAsync(string articleNumber)
    {
        await _httpClient.DeleteAsync($"api/article{articleNumber}");

    }
}