using XmlCore.Shared.DTO;

namespace XmlCore.Client.Services;

public class CompanyManager
{
    public CompanyDTO? Company { get; set; }



    public  bool Exists { get; private set; }

    public  void Set(CompanyDTO company)
    {
        Exists = true;
        Company = company;
    }

    public  void Clear()
    {
        Exists = false;
        Company = null;
    }

    public  CompanyDTO Get()
    {
        return Company;
    }
}