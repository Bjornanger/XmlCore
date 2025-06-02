using Microsoft.EntityFrameworkCore;
using XmlCore.Shared.Entities;
using XmlCore.Shared.Interface;

namespace XmlCore.DataAccess.Repositories;

public class UserRepository : IUserInterface<User>
{

    private readonly XmlCoreDbContext _context;

    public UserRepository(XmlCoreDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return _context.Users;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task CreateAsync(User user)
    {
        _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> UpdateAsync(User user, int id)
    {
        var userToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (userToUpdate != null)
        {
            return null;
        }

        userToUpdate.Name = user.Name;
        userToUpdate.Surname = user.Surname;
        userToUpdate.Email = user.Email;
        userToUpdate.Password = user.Password;

        _context.Users.Update(userToUpdate);
        await _context.SaveChangesAsync();
        return userToUpdate;
    }

    public async Task DeleteAsync(int id)
    {
        var userToRemove = await _context.Users.FindAsync(id);

        if (userToRemove is null)
        {
            return;
        }

        _context.Users.Remove(userToRemove);
        await _context.SaveChangesAsync();
    }
}