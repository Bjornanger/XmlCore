using XmlCore.Shared.DTO;
using XmlCore.Shared.Interface;

namespace XmlCore.Client.Services;

public class OrderService : IOrderInterface<OrderDTO>
{
    private readonly HttpClient _httpClient;

    public OrderService(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("XmlCoreAPI");
    }

    public async Task<IEnumerable<OrderDTO>> GetAllAsync()
    {
        var allOrders = await _httpClient.GetFromJsonAsync<IEnumerable<OrderDTO>>("api/order/orders");
        return await Task.FromResult(allOrders);
    }

    public async Task<OrderDTO> GetByIdAsync(int id)
    {
        var orderToShow = await _httpClient.GetFromJsonAsync<OrderDTO>($"api/order/{id}");
        return await Task.FromResult(orderToShow);
    }

    public async Task CreateAsync(OrderDTO order)
    {
        var response = await _httpClient.PostAsJsonAsync("api/order/order", order);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to create order.");
        }
    }

    public async Task<OrderDTO> UpdateAsync(OrderDTO order, int id)//ToDO kunna uppdatera Orders
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteFromJsonAsync<OrderDTO>($"api/order/remove/{id}");

    }

    public async Task<OrderDTO> GetOrderByOrderNumberAsync(int orderNumber)
    {
        var orderByNumb = await _httpClient.GetFromJsonAsync<OrderDTO>($"api/order/ordernumber/{orderNumber}");
        return await Task.FromResult(orderByNumb);
    }
 
}