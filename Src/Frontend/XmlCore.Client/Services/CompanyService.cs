using XmlCore.Shared.DTO;
using XmlCore.Shared.Interface;

namespace XmlCore.Client.Services;

public class CompanyService : ICompanyInterface<CompanyDTO>
{
    private readonly HttpClient _httpClient;

    public CompanyService(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("XmlCoreAPI");
    }

    public async Task<IEnumerable<CompanyDTO>> GetAllAsync()
    {
        var companyList = await _httpClient.GetFromJsonAsync<IEnumerable<CompanyDTO>>("api/company/companies");
        return await Task.FromResult(companyList);
    }

    public async Task<CompanyDTO> GetByIdAsync(int id)
    {
        var foundCompany = await _httpClient.GetFromJsonAsync<CompanyDTO>($"api/company/{id}");
        return await Task.FromResult(foundCompany);
    }

    public async Task CreateAsync(CompanyDTO company)
    {
        await _httpClient.PostAsJsonAsync("api/company", company);
    }

    public async Task<CompanyDTO> UpdateAsync(CompanyDTO company, int id)
    {
        await _httpClient.PutAsJsonAsync($"api/company/{id}", company);
        return await Task.FromResult(company);
    }


    public async Task<CompanyDTO> GetAllOrdersForCompany(int id)
    {
        var allOrders = await _httpClient.GetFromJsonAsync<CompanyDTO>($"api/company/orders/{id}");
        return await Task.FromResult(allOrders);
    }

    
    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteFromJsonAsync<CompanyDTO>($"api/company/remove/{id}");
    }
}