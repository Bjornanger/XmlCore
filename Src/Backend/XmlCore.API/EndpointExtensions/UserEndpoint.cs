using XmlCore.Shared.Entities;
using XmlCore.Shared.Interface;

namespace XmlCore.API.EndpointExtensions;

public static class UserEndpoint
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/user");
        group.MapGet("/users", GetAllUsers);
        group.MapPost("/", AddNewUser);
        group.MapGet("/{id}", GetUserById);
        group.MapDelete("/remove/{id}", DeleteUser);

        return app;
    }

    private static async Task<IResult> AddNewUser(IUserInterface<User> userRepository, User newUser)
    {
        var userList = await userRepository.GetAllAsync();

        if (userList.ToList().Any(u => u.Email.Equals(newUser.Email)))
        {
            return Results.BadRequest($"User with this Email: {newUser.Email} already Exists.");
        }


        var newAdmin = new User
        {
            Id = newUser.Id,
            Name = newUser.Name,
            Surname = newUser.Surname,
            Email = newUser.Email,
            Password = newUser.Password
        };

        await userRepository.CreateAsync(newAdmin);
        return Results.Ok("New Admin created.");
    }

    private static async Task<IEnumerable<User>> GetAllUsers(IUserInterface<User> userRepository)
    {
        var allUsers = await userRepository.GetAllAsync();

        if (allUsers is null)
        {
            Results.NotFound("No Users found in database");
            return null;

        }

        var userList = allUsers.Select(u => new User()
        {
            Id = u.Id,
            Name = u.Name,
            Surname = u.Surname,
            Email = u.Email,
            Password = u.Password
        }).ToList();




        Results.Ok(userList.ToList());
        return userList.ToList();
    }

    private static async Task<IResult> GetUserById(IUserInterface<User> userRepository, int id)
    {
        var getSpecUser = await userRepository.GetByIdAsync(id);

        if (getSpecUser is null)
        {
            return Results.NotFound($"Cant find this Id: {id} in DB");
        }
        
        var userToShow = new User
        {
            Id = getSpecUser.Id,
            Name = getSpecUser.Name,
            Surname = getSpecUser.Surname,
            Email = getSpecUser.Email,
            Password = getSpecUser.Password
        };

        return Results.Ok($"User found with Id:{userToShow.Id} and Name:{userToShow.Name}.");
    }

    private static async Task<IResult> DeleteUser(IUserInterface<User> userRepository, int id)
    {
        var findAdminToDelete = await userRepository.GetByIdAsync(id);

        if (findAdminToDelete is null)
        {
            return Results.BadRequest($"User Id cant be found {id}");
        }

        await userRepository.DeleteAsync(findAdminToDelete.Id);
        return Results.Ok($"Admin with Id: {findAdminToDelete.Id} and Name: {findAdminToDelete.Name} deleted.");
    }
}