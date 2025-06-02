using Microsoft.EntityFrameworkCore;
using XmlCore.Shared.Entities;
using XmlCore.Shared.Interface;

namespace XmlCore.DataAccess.Repositories;

public class ArticleRepository :IArticleInterface<Article>
{
    private readonly XmlCoreDbContext _context;

    public ArticleRepository(XmlCoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Article>> GetAllAsync()
    {
        return _context.Articles.Include(a => a.Category)
            .Include(a => a.ArticlesInOrderList).ToList();
    }

    public async Task<Article> GetByIdAsync(Guid id)
    {
        return _context.Articles.Include(a => a.Category).FirstOrDefault(a => a.Id == id);
    }

    public async Task<Article> GetArticleByName(string articleName)
    {
        return _context.Articles
            .Include(a => a.Category)
            .FirstOrDefault(a => a.Name.ToLower() == articleName.ToLower());
    }

    public async Task<Article> GetArticleByNumber(string articleNumber)
    {
        return await _context.Articles.Include(a =>a.Category)
            .FirstOrDefaultAsync(a => a.ArticleNumber == articleNumber);

    }

   public async Task CreateAsync(Article article)
    {
        await _context.Articles.AddAsync(article);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Article>> GetByCategoryAsync(int categoryId)
    {
        return _context.Articles.Include(a => a.Category.Id == categoryId).Where(a => a.Category.Id == categoryId);

    }

    public async Task<Article> UpdateAsync(Article art, string articleNumber)
    { var articleToUpdate = await _context.Articles
            .Include(a => a.Category)
            .FirstOrDefaultAsync(a => a.Id == art.Id);



        if (articleToUpdate == null)
        {
            return null;
        }
        
        articleToUpdate.ArticleNumber = art.ArticleNumber;
        articleToUpdate.Name = art.Name;
        articleToUpdate.Price = art.Price;
        articleToUpdate.Category = art.Category;
        articleToUpdate.Description = art.Description;
        articleToUpdate.Status = art.Status;
        articleToUpdate.Stock = art.Stock;

       
        await _context.SaveChangesAsync();
        return articleToUpdate;
    }

    public async Task<Article> SwitchStatusOnArticle(Article art, string articleNumber)
    {

        var articleToUpdate = await _context.Articles
            .Include(a => a.Category)
            .FirstOrDefaultAsync(a => a.ArticleNumber == art.ArticleNumber);

        if (articleToUpdate == null)
        {
            return null;
        }


       

        _context.Articles.Update(articleToUpdate);
        await _context.SaveChangesAsync();
        return articleToUpdate;
    }

    public async Task DeleteAsync(string articleNumber)
    {
        var article = await _context.Articles.FirstOrDefaultAsync(a => a.ArticleNumber == articleNumber);

        var articleToRemove = _context.Articles.FindAsync(article.Id);

        if (articleToRemove == null)
        {
            return;
        }

        _context.Articles.Remove(await articleToRemove);
        await _context.SaveChangesAsync();
    }
}