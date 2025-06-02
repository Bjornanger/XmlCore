namespace XmlCore.Shared.Interface;

public interface IOrderInterface <T> : IService<T> where T : class
{
    Task<T> GetOrderByOrderNumberAsync(int orderNumber);
    
}