using Microsoft.EntityFrameworkCore;
using XmlCore.Shared.Entities;
using XmlCore.Shared.Interface;

namespace XmlCore.DataAccess.Repositories;

public class CompanyRepository : ICompanyInterface<Company>
{
    private readonly XmlCoreDbContext _context;

    public CompanyRepository(XmlCoreDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Company>> GetAllAsync()
    {
        return await _context.Companies.ToListAsync();
    }

    public async Task<Company> GetByIdAsync(int id)
    {
        return await _context.Companies.Include(c => c.Orders).FirstOrDefaultAsync(c => c.Id == id);

    }

    public async Task CreateAsync(Company company)
    {
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();
    }

    public async Task<Company> UpdateAsync(Company company, int id)
    {
        var companyToUpdate = await _context.Companies
            .Include(c => c.Orders)
            .ThenInclude(o => o.ArticlesInOrderList)
            .ThenInclude(o => o.Article).ThenInclude(a => a.Category).FirstOrDefaultAsync(c => c.Id == company.Id);

        if (companyToUpdate is null)
        {
            return null;
        }

        companyToUpdate.CompanyName = company.CompanyName;
        companyToUpdate.Email = company.Email;
        companyToUpdate.Password = company.Password;
        companyToUpdate.Phone = company.Phone;
        companyToUpdate.Address = company.Address;
        companyToUpdate.City = company.City;
        companyToUpdate.Country = company.Country;
        companyToUpdate.Orders = company.Orders;

        await _context.SaveChangesAsync();
        return companyToUpdate;

    }

    public async Task DeleteAsync(int id)
    {
        var companyToRemove = await _context.Companies.FindAsync(id);

        if (companyToRemove is null)
        {
            return;
        }

        _context.Companies.Remove(companyToRemove);
        await _context.SaveChangesAsync();

    }

    public async Task<Company?> GetAllOrdersForCompany(int id)
    {
        return await _context.Companies
            .Include(c => c.Orders)
            .ThenInclude(o => o.ArticlesInOrderList)
            .ThenInclude(ao => ao.Article)
            .ThenInclude(a => a.Category)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}