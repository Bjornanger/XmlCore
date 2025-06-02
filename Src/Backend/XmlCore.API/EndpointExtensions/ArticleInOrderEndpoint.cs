using XmlCore.Shared.DTO;
using XmlCore.Shared.Entities;
using XmlCore.Shared.Interface;

namespace XmlCore.API.EndpointExtensions;

public static class ArticleInOrderEndpoint
{
    public static IEndpointRouteBuilder MapArticlesInOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/AiO");

        group.MapGet("/", GetAll);


        return app;
    }

    private static async Task<IEnumerable<ArticlesInOrderDTO>> GetAll(IArticlesInOrderInterface<ArticlesInOrder> AiORepository)
    {
        var allAiO = await AiORepository.GetAllAsync();

        if (allAiO is null)
        {
            Results.BadRequest("No AiO in Db.");
            return null;
        }

        var articlesInAiO = allAiO.Select(a => new ArticlesInOrderDTO
        {
            Id = a.Id,
            Article = new ArticleDTO
            {
                Id = a.Article.Id,
                ArticleNumber = a.Article.ArticleNumber,
                Name = a.Article.ArticleNumber,
                Description = a.Article.Description,
                Price = a.Article.Price,
                Category = a.Article.Category.Id,
                InOrder = a.Article.ArticlesInOrderList,
                Status = a.Article.Status,
                Stock = a.Article.Stock
            },
            //Order = null,
            Amount = a.Amount
        }).ToList();


        Results.Ok("AiO Found");
        return articlesInAiO.ToList();


    }
}