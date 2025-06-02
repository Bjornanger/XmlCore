using XmlCore.Shared.Entities;
using XmlCore.Shared.Interface;

namespace XmlCore.API.EndpointExtensions;

public static class CategoryEndpoint
{
    public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/category");
        group.MapGet("/categories", GetAllCategories);
        group.MapGet("/{id}", GetCategoryById);
        group.MapPost("/", AddNewCategory);
        group.MapPut("/{id}", UpdateCategory);
        group.MapDelete("/{id}", DeleteCategory);
        return app;
    }

    private static async Task<IResult> AddNewCategory(ICategoryInterface<Category> categoryRepository, Category category)
    {
        var newCategoryToAdd = await categoryRepository.GetAllAsync();

        if (newCategoryToAdd.ToList().Any(c => c.Name.ToLower().Equals(category.Name.ToLower())))
        {
            return Results.BadRequest($"Category with this Name: {category.Name} already exists.");
        }

        var newCategory = new Category
        {
            Name = category.Name
        };


        await categoryRepository.CreateAsync(newCategory);
        return Results.Ok($"Category Added.");
    }

    private static async Task<IEnumerable<Category>> GetAllCategories(ICategoryInterface<Category> categoryRepository)
    {
        var allCategories = await categoryRepository.GetAllAsync();
        if (allCategories is null)
        {
            Results.NotFound("No Categories found in Db");
            return null;
        }

        Results.Ok();
        return allCategories.ToList();

    }

    private static async Task<IResult> GetCategoryById(ICategoryInterface<Category> categoryRepository, int id)
    {
        var categoryToFind = await categoryRepository.GetByIdAsync(id);

        if (categoryToFind is null)
        {
         
            return Results.NotFound($"Category with {id} was not found.");
            
        }
        return Results.Ok($"Category: {categoryToFind.Name} retrieved from Database.");
    }

    private static async Task<IResult> UpdateCategory(ICategoryInterface<Category> categoryRepository,Category categoryFromUser, int id)
    {
        var categoryToUpdate = await categoryRepository.GetByIdAsync(id);

        if (categoryToUpdate is null)
        {
            return Results.NotFound($"Category with Id: {id} was not found.");
        }

        categoryToUpdate = new Category
        {
            Id = id,
            Name = categoryFromUser.Name
        };

        await categoryRepository.UpdateAsync(categoryToUpdate, id);
        return Results.Ok(categoryToUpdate);
    }

    private static async Task<IResult> DeleteCategory(ICategoryInterface<Category> categoryRepository, int id)
    {
        var categoryToRemove = await categoryRepository.GetByIdAsync(id);
        if (categoryToRemove is null)
        {
            return Results.NotFound($"Category with Id: {id} could not be found");
        }
        await categoryRepository.DeleteAsync(categoryToRemove.Id);
        return Results.Ok($"Category with Id: {categoryToRemove.Id} and Name: {categoryToRemove.Name} removed.");

    }
}