using XmlCore.Shared.DTO;
using XmlCore.Shared.Entities;
using XmlCore.Shared.Interface;

namespace XmlCore.API.EndpointExtensions;

public static class OrderEndpoint
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/order");
        group.MapPost("/order", AddOrder);
        group.MapGet("/orders", GetAllOrders);
        group.MapGet("/ordernumber/{orderNumber}", GetOrderByOrderNumber);
        group.MapDelete("/remove/{id}", DeleteOrder);
        return app;
    }

    private static async Task<IResult> AddOrder(
        IOrderInterface<Order> orderRepository,
        ICompanyInterface<Company> companyRepository,
        IArticleInterface<Article> articleRepository,
        OrderDTO order)
    {
        var company = await companyRepository.GetByIdAsync(order.Company.Id);

        if (company is null)
        {
            return Results.NotFound("No Company found");
        }

        var articleList = new List<ArticlesInOrder>();

        foreach (var articlesInOrder in order.ArticlesInOrderList)
        {
            var article = await articleRepository.GetArticleByNumber(articlesInOrder.Article.ArticleNumber);
            

            if (article is null)
            {
                return Results.NotFound("No Articles found");
            }
            
            articleList.Add(new ArticlesInOrder
            {
                Article = article,
                Order = null,
                Amount = articlesInOrder.Amount

            });
        }
        

        var allOrder = await orderRepository.GetAllAsync();

        var checkOrderNumber = allOrder.MaxBy(o => o.OrderNumber);
        

        int newOrderNumber = 0;

        if (checkOrderNumber is null)
        {
            newOrderNumber = + 5000;
        }
        else
        {
            newOrderNumber = Convert.ToInt32(checkOrderNumber.OrderNumber) + 1;
        }
        

        var orderInCompany = new Order
        {
            Id = order.Id,
            OrderNumber = newOrderNumber,
            Company = company,
            TotalSum = articleList.Sum(a => a.Article.Price * a.Amount),
            DateOfOrder = DateTime.UtcNow,
            ArticlesInOrderList = articleList
        };

        await orderRepository.CreateAsync(orderInCompany);
        return Results.Ok("Order created.");

    }

    private static async Task<IEnumerable<OrderDTO>> GetAllOrders(
        IOrderInterface<Order> orderRepository, 
        ICompanyInterface<Company> companyRepository)
    {
        var orderList = await orderRepository.GetAllAsync();

        if (orderList is null)
        {
            Results.BadRequest("No orders found in DB.");
            return null;
        }

        
        var orderToShow = orderList.Select(static o => new OrderDTO
        {
            Id = o.Id,
            OrderNumber = o.OrderNumber,
            Company = new CompanyDTO
            {
                Id = o.Company.Id,
                CompanyName = o.Company.CompanyName,
                Email = o.Company.Email,
                Password = o.Company.Password,
                Phone = o.Company.Phone,
                Address = o.Company.Address,
                City = o.Company.City,
                Country = o.Company.Country,
                Orders = new List<OrderDTO>()
            },
            TotalSum = o.TotalSum,
            DateOfOrder = o.DateOfOrder,
            ArticlesInOrderList = o.ArticlesInOrderList.Select(static aIo => new ArticlesInOrderDTO
            {
                Id = aIo.Id,
                Article = new ArticleDTO
                {
                    Id = aIo.Article.Id,
                    ArticleNumber = aIo.Article.ArticleNumber,
                    Name = aIo.Article.Name,
                    Description = aIo.Article.Description,
                    Price = aIo.Article.Price,
                    Quantity = 0,
                    Category = aIo.Article.Category.Id,
                    InOrder = null,
                    Status = aIo.Article.Status,
                    Stock = aIo.Article.Stock
                },
                Order = null,
                Amount = aIo.Amount
            }).ToList()
            

        }).ToList();
        

        Results.Ok("All orders found");
        return orderToShow;
    }

    private static async Task<IResult> GetOrderByOrderNumber(IOrderInterface<Order> orderRepository, int orderNumber)
    {
        var specificOrder = await orderRepository.GetOrderByOrderNumberAsync(orderNumber);

        if (specificOrder is null)
        {
            return Results.NotFound("No orders with that number found.");
        }

        var foundOrder = new OrderDTO()
        {
            Id = specificOrder.Id,
            OrderNumber = specificOrder.OrderNumber,
            Company = new CompanyDTO
            {
                Id = specificOrder.Company.Id,
                CompanyName = specificOrder.Company.CompanyName,
                Email = specificOrder.Company.Email,
                Password = specificOrder.Company.Password,
                Phone = specificOrder.Company.Phone,
                Address = specificOrder.Company.Address,
                City = specificOrder.Company.City,
                Country = specificOrder.Company.Country,
                Orders = null
            },
            TotalSum = specificOrder.TotalSum,
            DateOfOrder = specificOrder.DateOfOrder,
            ArticlesInOrderList = specificOrder.ArticlesInOrderList.Select( aio => new ArticlesInOrderDTO
            {
                Id = aio.Id,
                Article = new ArticleDTO
                {
                    Id = aio.Article.Id,
                    ArticleNumber = aio.Article.ArticleNumber,
                    Name = aio.Article.Name,
                    Description = aio.Article.Description,
                    Price = aio.Article.Price,
                    Quantity = aio.Amount,
                    Category = aio.Article.Category.Id,
                    InOrder = null,
                    Status = aio.Article.Status,
                    Stock = aio.Article.Stock
                },
                Order = null,
                Amount = aio.Amount
            }).ToList()
        };


        return Results.Ok(foundOrder);
    }

    private static async Task<IResult> DeleteOrder(IOrderInterface<Order> orderRepository, int id)
    {
        var orderToDelete = await orderRepository.GetByIdAsync(id);

        if (orderToDelete is null)
        {
            return Results.NotFound("No Order with that ID was found");
        }

        await orderRepository.DeleteAsync(orderToDelete.Id);
        return Results.Ok("Order deleted.");
    }
}