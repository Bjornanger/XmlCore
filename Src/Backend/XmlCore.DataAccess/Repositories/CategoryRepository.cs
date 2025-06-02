using Microsoft.EntityFrameworkCore;
using XmlCore.Shared.Entities;
using XmlCore.Shared.Interface;

namespace XmlCore.DataAccess.Repositories;

public class CategoryRepository : ICategoryInterface<Category>
{
    private readonly XmlCoreDbContext _context;

    public CategoryRepository(XmlCoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return _context.Categories;
    }

    public async Task<Category> GetByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task CreateAsync(Category entity)
    {
        _context.Categories.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<Category> UpdateAsync(Category entity, int id)
    {
        var categoryToUpdate = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

        if (categoryToUpdate is null)
        {
            return null;
        }


        categoryToUpdate.Name = entity.Name;

        _context.Categories.Update(categoryToUpdate);
        await _context.SaveChangesAsync();
        return categoryToUpdate;


    }

    public async Task DeleteAsync(int id)
    {
        var categoryToDelete = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

        if (categoryToDelete is null)
        {
            return;
        }

        _context.Categories.Remove(categoryToDelete);
        await _context.SaveChangesAsync();

    }
}