namespace XmlCore.Shared.Interface;

public interface ICompanyInterface <T> : IService<T> where T : class
{
    Task<T> GetAllOrdersForCompany(int id);

    

}