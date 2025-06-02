namespace XmlCore.Shared.Interface;

public interface IService<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T> GetByIdAsync(int id);

    Task CreateAsync(T entity);
    
    Task<T> UpdateAsync(T entity, int id);
    Task DeleteAsync(int id);
}