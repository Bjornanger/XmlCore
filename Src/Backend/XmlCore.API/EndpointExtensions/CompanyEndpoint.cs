using XmlCore.Shared.DTO;
using XmlCore.Shared.Entities;
using XmlCore.Shared.Interface;

namespace XmlCore.API.EndpointExtensions;

public static class CompanyEndpoint
{
    public static IEndpointRouteBuilder MapCompanyEndpoints(this IEndpointRouteBuilder app)
    {

        var group = app.MapGroup("api/company");
        group.MapPost("/", AddNewCompany);
        group.MapGet("/{id}", GetCompanyById);
        group.MapGet("/companies", GetAllCompanies);
        group.MapPut("/{id}", UpdateCompanyInfo);
        group.MapGet("/orders/{id}", GetOrderFromCompany);
        group.MapDelete("/remove/{id}", DeleteCompany);

        return app;
    }

    private static async Task<IResult> AddNewCompany(ICompanyInterface<Company> companyRepository, CompanyDTO company)
    {
        var newCompany = await companyRepository.GetAllAsync();

        if (newCompany.Any(c => c.CompanyName.ToLower() == company.CompanyName.ToLower()))
        {
            return Results.BadRequest("Company with this Name already exists.");
        }

        if (newCompany.Any(c => c.Email == company.Email))
        {
            return Results.BadRequest("Company with this Email already exists.");
        }

        Company createCompany = new ()
        {
            Id = company.Id,
            CompanyName = company.CompanyName,
            Email = company.Email,
            Password = company.Password,
            Phone = company.Phone,
            Address = company.Address,
            City = company.City,
            Country = company.Country,
            Orders = []
        };

        await companyRepository.CreateAsync(createCompany);
        return Results.Ok("Company created.");


    }

    private static async Task<IEnumerable<CompanyDTO>> GetAllCompanies(ICompanyInterface<Company> companyRepository)
    {
        var allCompanies = await companyRepository.GetAllAsync();
        if (allCompanies is null)
        {
            Results.BadRequest("No Companies in DB.");
            return null;
        }

        var companyList = allCompanies.Select(c => new CompanyDTO
        {
            Id = c.Id,
            CompanyName = c.CompanyName,
            Email = c.Email,
            Password = c.Password,
            Phone = c.Phone,
            Address = c.Address,
            City = c.City,
            Country = c.Country

        }).ToList();




        Results.Ok("Companies found");
        return companyList.ToList();


    }

    private static async Task<IResult> UpdateCompanyInfo(ICompanyInterface<Company> companyRepository,IOrderInterface<Order> orderRepository, CompanyDTO company, int id)
    {

        var companyToChange = await companyRepository.GetByIdAsync(id);

        if (companyToChange is null)
        {
            return Results.NotFound("No Company Id found.");
        }
        

        var updatedCompany = new Company
        {
            Id = companyToChange.Id,
            CompanyName = company.CompanyName,
            Email = company.Email,
            Password = company.Password,
            Phone = company.Phone,
            Address = company.Address,
            City = company.City,
            Country = company.Country,
            Orders = companyToChange.Orders

        };

        await companyRepository.UpdateAsync(updatedCompany, updatedCompany.Id);
        return Results.Ok(updatedCompany);

        

    }

    private static async Task<IResult> GetOrderFromCompany(ICompanyInterface<Company> companyRepository, int id)
    {
        var companyOrderToShow = await companyRepository.GetAllOrdersForCompany(id);

        if (companyOrderToShow is null)
        {
            return Results.BadRequest("No Company with that Id");
        }


        var allInfoFromCompany = new CompanyDTO
        {
            Id = companyOrderToShow.Id,
            CompanyName = companyOrderToShow.CompanyName,
            Email = companyOrderToShow.Email,
            Password = companyOrderToShow.Password,
            Phone = companyOrderToShow.Phone,
            Address = companyOrderToShow.Address,
            City = companyOrderToShow.City,
            Country = companyOrderToShow.Country,
            //Static in a Select() makes it more efficient and less memory are used because it only searches in that specific list.
            Orders = companyOrderToShow.Orders.Select(static o => new OrderDTO 
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                Company = new CompanyDTO(),
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
                        Category = aIo.Article.Category.Id,
                        InOrder = null,
                        Status = aIo.Article.Status,
                        Stock = aIo.Article.Stock
                    },
                    Order = null,
                    Amount = aIo.Amount
                }).ToList()
            }).ToList(),
        };
        return Results.Ok(allInfoFromCompany);
    }

    private static async Task<IResult> GetCompanyById(ICompanyInterface<Company> companyRepository, int id)
    {
        var companyToFind = await companyRepository.GetByIdAsync(id);

        if
            (companyToFind is null)
        {
            return Results.BadRequest("No Company with that Id was found.");
        }

        var companyToShow = new Company
        {
            Id = companyToFind.Id,
            CompanyName = companyToFind.CompanyName,
            Email = companyToFind.Email,
            Password = companyToFind.Password,
            Phone = companyToFind.Phone,
            Address = companyToFind.Address,
            City = companyToFind.City,
            Country = companyToFind.Country,
            Orders = companyToFind.Orders
        };
        return Results.Ok(companyToShow);
    }

    private static async Task<IResult> DeleteCompany(ICompanyInterface<Company> companyRepository, int id)
    {
        var companyToDelete = await companyRepository.GetByIdAsync(id);

        if (companyToDelete is null)
        {
            return Results.BadRequest("No Company with that Id was found");
        }

        await companyRepository.DeleteAsync(companyToDelete.Id);
        return Results.Ok("Company deleted.");

    }
}