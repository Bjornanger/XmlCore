namespace XmlCore.Shared.Interface;

public interface IArticlesInOrderInterface<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    

   

}