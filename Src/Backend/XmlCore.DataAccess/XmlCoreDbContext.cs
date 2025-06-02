using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using XmlCore.Shared.Entities;

namespace XmlCore.DataAccess;

public class XmlCoreDbContext : DbContext
{
    public DbSet<Article> Articles { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Company> Companies { get; set; }

    public DbSet<Order> Orders { get; set; }
    public DbSet<User> Users { get; set; }

    public DbSet<ArticlesInOrder> ArticlesInOrders { get; set; }


    public XmlCoreDbContext(DbContextOptions options) : base(options)
    {

    }
}