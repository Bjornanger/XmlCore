using Microsoft.EntityFrameworkCore;
using XmlCore.API.EndpointExtensions;
using XmlCore.DataAccess;
using XmlCore.DataAccess.Repositories;
using XmlCore.Shared.Entities;
using XmlCore.Shared.Interface;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("XmlCoreDB");
builder.Services.AddDbContext<XmlCoreDbContext>(options => options.UseSqlServer(connectionString));




builder.Services.AddScoped<IUserInterface<User>, UserRepository>();
builder.Services.AddScoped<ICategoryInterface<Category>, CategoryRepository>();
builder.Services.AddScoped<ICompanyInterface<Company>, CompanyRepository>();
builder.Services.AddScoped<IOrderInterface<Order>, OrderRepository>();
builder.Services.AddScoped<IArticleInterface<Article>, ArticleRepository>();
builder.Services.AddScoped<IArticlesInOrderInterface<ArticlesInOrder>, ArticlesInOrderRepository>();






var app = builder.Build();



app.MapOrderEndpoints();
app.MapUserEndpoints();
app.MapCategoryEndpoints();
app.MapCompanyEndpoints();
app.MapArticleEndpoints();
app.MapArticlesInOrderEndpoints();

app.Run();