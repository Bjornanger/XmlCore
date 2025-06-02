using Microsoft.AspNetCore.Mvc;
using XmlCore.Shared.DTO;
using XmlCore.Shared.Entities;
using XmlCore.Shared.Interface;

namespace XmlCore.API.EndpointExtensions;

public static class ArticleEndpoint
{
    public static IEndpointRouteBuilder MapArticleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/article");
        group.MapPost("/", AddArticle);
        group.MapGet("/articles", GetAllArticles);
        group.MapGet("/{articleNumber}", GetByArticleNumber);
        group.MapGet("article/{id:guid}", GetById);
        group.MapPut("/{articleNumber}", UpdateArticle);
        group.MapPut("/status/{articleNumber}", SwitchStatusOnArticle);
        group.MapDelete("/{articleNumber}", DeleteArticle);
        group.MapGet("/articleName/{articleName}",GetByArticleName);

        return app;
    }

    private static async Task<IResult> GetByArticleName(IArticleInterface<Article> articleRepository, string articleName)
    {
        var articleToGet = await articleRepository.GetArticleByName(articleName);

        if (articleToGet != null)
        {
            return Results.NotFound("No Article with that Name found.");
        }

        var foundArticle = new ArticleDTO
        {
            Id = articleToGet.Id,
            ArticleNumber = articleToGet.ArticleNumber,
            Name = articleToGet.Name,
            Description = articleToGet.Description,
            Price = articleToGet.Price,
            Category = articleToGet.Category.Id,
            InOrder = null,
            Quantity = 0,
            Status = articleToGet.Status,
            Stock = articleToGet.Stock,
            
        };

        return Results.Ok(foundArticle);
    }


    private static async Task<IResult> GetById([FromServices]IArticleInterface<Article> articleRepository,[FromRoute] Guid id) 
    {
        var getSpecArticle = await articleRepository.GetByIdAsync(id);
        if (getSpecArticle is null)
        {
            return Results.NotFound("No Article with that Id.");
        }

        var foundArticle = new ArticleDTO
        {
            Id = getSpecArticle.Id,
            ArticleNumber = getSpecArticle.ArticleNumber,
            Name = getSpecArticle.Name,
            Description = getSpecArticle.Description,
            Price = getSpecArticle.Price,
            Category = getSpecArticle.Category.Id,
            InOrder = null,
            Quantity = 0,
            Status = getSpecArticle.Status,
            Stock = getSpecArticle.Stock,
            
        };
        
        return Results.Ok(foundArticle);

    }

    private static async Task<IResult> AddArticle(IArticleInterface<Article> articleRepository, ICategoryInterface<Category> categoryRepository, ArticleDTO article)
    {

        var articleExists = await articleRepository.GetArticleByName(article.Name);

        if (articleExists != null )
        {
            return Results.BadRequest($"Article with Name: {article.Name} already exists.");
        }
        var categoryToAdd = await categoryRepository.GetByIdAsync(article.Category);
        if (categoryToAdd is null)
        {
            return Results.BadRequest("No Category found");
        }
        
        string categoryForNumber = categoryToAdd.Name;
        string firstThreeLetters = categoryForNumber.Substring(0, 3);
        
        var newArticleNumber = string.Empty;
        int highestNumber = 0;
        List<int> allArtNumbers = [];
        int generatedNumber = 1000;


        var allArticles = await articleRepository.GetAllAsync();

        if (allArticles == null || !allArticles.Any())
        {
           
            newArticleNumber = firstThreeLetters + generatedNumber;
        }


        var articleByCategory =
            allArticles.Where(a => a.Category.Name == categoryForNumber).ToList();

        if (articleByCategory == null || !articleByCategory.Any())
        {
            newArticleNumber = firstThreeLetters + generatedNumber;
        }
        else
        {
            var lastDigitsInArticleNumber = articleByCategory.Select(a => a.ArticleNumber.Substring(3)).ToList();
            allArtNumbers = lastDigitsInArticleNumber.Select(int.Parse).ToList();
            highestNumber = allArtNumbers.Max();
            int newNumber = highestNumber + 1;
            newArticleNumber = firstThreeLetters + newNumber;
        }
        
        var newArticle = new Article
        {
            ArticleNumber = newArticleNumber,
            Name = article.Name,
            Description = article.Description,
            Price = article.Price,
            Category = categoryToAdd,
            ArticlesInOrderList = null,
            Status = true,
            Stock = article.Stock
        };
        
        await articleRepository.CreateAsync(newArticle);
        return Results.Ok("New article added.");
        
    }

    private static async Task<IEnumerable<ArticleDTO>> GetAllArticles(IArticleInterface<Article> articleRepository)
    {
        var articleList = await articleRepository.GetAllAsync();

        if (articleList is null)
        {
            Results.BadRequest("No Articles found.");
            return null;
        }

        var articleToShow = articleList.Select(a => new ArticleDTO
        {
            Id = a.Id,
            ArticleNumber = a.ArticleNumber,
            Name = a.Name,
            Description = a.Description,
            Price = a.Price,
            Category = a.Category.Id,
            Status = a.Status,
            Stock = a.Stock
        }).ToList();


        Results.Ok("Articles found.");
        return articleToShow.ToList();

    }

    private static async Task<IResult> GetByArticleNumber(IArticleInterface<Article> articleRepository, string articleNumber)
    {

        var articleToFind = await articleRepository.GetArticleByNumber(articleNumber);

        if (articleToFind is null)
        {
            return Results.NotFound("No article in database.");
        }
        

        ArticleDTO articleNumberToGet = new()
        {
            Id = articleToFind.Id,
            ArticleNumber = articleToFind.ArticleNumber,
            Name = articleToFind.Name,
            Description = articleToFind.Description,
            Price = articleToFind.Price,
            Quantity = 0,
            Category = articleToFind.Category.Id,
            InOrder = articleToFind.ArticlesInOrderList,
            Status = articleToFind.Status,
            Stock = articleToFind.Stock
        };

        return Results.Ok(articleNumberToGet);


    }

    private static async Task<IResult> UpdateArticle(IArticleInterface<Article> articleRepository,ICategoryInterface<Category> categoryRepository, ArticleDTO article, string articleNumber)
    {
        var articleToUpdate = await articleRepository.GetArticleByNumber(articleNumber);
        if (articleToUpdate is null)
        {
            return Results.BadRequest("No Article found.");
        }

        if (articleToUpdate.Category is null)
        {
            return Results.BadRequest("No Category in that Article found.");
        }

        Category changeCategory = await categoryRepository.GetByIdAsync(article.Category);



        Article newArticle = new()
        {
            Id = articleToUpdate.Id,
            ArticleNumber = articleToUpdate.ArticleNumber,
            Name = article.Name,
            Description = article.Description,
            Price = article.Price,
            Category = changeCategory,
            ArticlesInOrderList = article.InOrder,
            Status = article.Status,
            Stock = article.Stock
        };

        await articleRepository.UpdateAsync(newArticle, newArticle.ArticleNumber);
        return Results.Ok(newArticle);


    }

    private static async Task<IResult> SwitchStatusOnArticle(IArticleInterface<Article> articleRepository, string articleNumber)
    {
        var switchStatus = await articleRepository.GetArticleByNumber(articleNumber);

        if (switchStatus is null)
        {
            return Results.BadRequest("No Article found.");
        }

        switchStatus.Status = !switchStatus.Status;

        await articleRepository.SwitchStatusOnArticle(switchStatus, articleNumber);
        return Results.Ok("Switched status.");
    }

    private static async Task<IResult> DeleteArticle(IArticleInterface<Article> articleRepository, string articleNumber)
    {
        var articleToRemove = await articleRepository.GetArticleByNumber(articleNumber);
        if (articleToRemove is null)
        {
            return Results.BadRequest("No Article found.");
        }

        await articleRepository.DeleteAsync(articleToRemove.ArticleNumber);
        return Results.Ok("Article removed.");
    }
}