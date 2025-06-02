namespace XmlCore.Shared.Interface;

public interface IArticleInterface <T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T> GetByIdAsync(Guid id);

    Task<T> GetArticleByName(string articleName);


    Task<T> GetArticleByNumber(string articleNumber);
    Task CreateAsync(T entity);
    Task<IEnumerable<T>> GetByCategoryAsync(int Category);
    Task<T> UpdateAsync(T entity, string articleNumber);

    Task<T> SwitchStatusOnArticle(T entity, string articleNumber);
    Task DeleteAsync(string articleNumber);
}