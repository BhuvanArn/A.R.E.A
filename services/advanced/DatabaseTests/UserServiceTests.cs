using Database;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class UserDatabaseHandlerTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IDbContextFactory<DatabaseContext> _contextFactory;
    private readonly IDatabaseHandler _databaseHandler;

    public UserDatabaseHandlerTests()
    {
        var services = new ServiceCollection();

        services.AddDbContextFactory<DatabaseContext>(options =>
            options.UseInMemoryDatabase(Guid.NewGuid().ToString())
        );

        services.AddScoped<IDatabaseHandler, DatabaseHandler>();

        _serviceProvider = services.BuildServiceProvider();

        _contextFactory = _serviceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();
        _databaseHandler = _serviceProvider.GetRequiredService<IDatabaseHandler>();
    }

    [Fact]
    public async Task CreateUserAsync_WithValidUser_AddsUserToDatabase()
    {
        var user = new User
        {
            Username = "BasicUser",
            Email = "email@test.com",
            Password = "BasicPassword",
            Salt = "test"
        };

        await _databaseHandler.AddAsync(user);

        using var context = _contextFactory.CreateDbContext();
        var createdUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "BasicUser");
        Assert.NotNull(createdUser);
        Assert.Equal("email@test.com", createdUser.Email);
        Assert.Equal("BasicPassword", createdUser.Password);
        Assert.Equal("test", createdUser.Salt);
    }

    [Fact]
    public async Task GetUserByIdAsync_WithExistingUser_ReturnsUser()
    {
        var user = new User
        {
            Username = "TestUser",
            Email = "testuser@test.com",
            Password = "TestPassword",
            Salt = "test"
        };

        using (var context = _contextFactory.CreateDbContext())
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        var retrievedUser = (await _databaseHandler.GetAsync<User>(s => s.Id == user.Id)).FirstOrDefault();

        Assert.NotNull(retrievedUser);
        Assert.Equal(user.Id, retrievedUser.Id);
        Assert.Equal("TestUser", retrievedUser.Username);
        Assert.Equal("testuser@test.com", retrievedUser.Email);
        Assert.Equal("TestPassword", retrievedUser.Password);
        Assert.Equal("test", retrievedUser.Salt);
    }

    [Fact]
    public async Task GetAllUsersAsync_ReturnsAllUsers()
    {
        var user1 = new User
        {
            Username = "User1",
            Email = "user1@test.com",
            Password = "Password1",
            Salt = "test"
        };
        var user2 = new User
        {
            Username = "User2",
            Email = "user2@test.com",
            Password = "Password2",
            Salt = "test2"
        };

        using (var context = _contextFactory.CreateDbContext())
        {
            context.Users.AddRange(user1, user2);
            await context.SaveChangesAsync();
        }

        var users = await _databaseHandler.GetAllAsync<User>();

        Assert.NotNull(users);
        Assert.Equal(2, users.Count());
        var retrievedUser1 = users.FirstOrDefault(u => u.Username == "User1");
        var retrievedUser2 = users.FirstOrDefault(u => u.Username == "User2");
        Assert.NotNull(retrievedUser1);
        Assert.NotNull(retrievedUser2);

        Assert.Equal("user1@test.com", retrievedUser1.Email);
        Assert.Equal("Password1", retrievedUser1.Password);
        Assert.Equal("test", retrievedUser1.Salt);

        Assert.Equal("user2@test.com", retrievedUser2.Email);
        Assert.Equal("Password2", retrievedUser2.Password);
        Assert.Equal("test2", retrievedUser2.Salt);
    }

    [Fact]
    public async Task UpdateUserAsync_WithExistingUser_UpdatesUserInDatabase()
    {
        var user = new User
        {
            Username = "UpdateUser",
            Email = "updateuser@test.com",
            Password = "OldPassword",
            Salt = "test"
        };

        using (var context = _contextFactory.CreateDbContext())
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        user.Email = "updatedemail@test.com";
        user.Password = "NewPassword";

        await _databaseHandler.UpdateAsync(user);

        using (var context = _contextFactory.CreateDbContext())
        {
            var updatedUser = await context.Users.FindAsync(user.Id);
            Assert.NotNull(updatedUser);
            Assert.Equal("UpdateUser", updatedUser.Username);
            Assert.Equal("updatedemail@test.com", updatedUser.Email);
            Assert.Equal("NewPassword", updatedUser.Password);
            Assert.Equal("test", updatedUser.Salt);
        }
    }

    [Fact]
    public async Task DeleteUserAsync_WithExistingUser_DeletesUserFromDatabase()
    {
        var user = new User
        {
            Username = "DeleteUser",
            Email = "deleteuser@test.com",
            Password = "DeletePassword",
            Salt = "test"
        };

        using (var context = _contextFactory.CreateDbContext())
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        await _databaseHandler.DeleteAsync(user);

        using (var context = _contextFactory.CreateDbContext())
        {
            var deletedUser = await context.Users.FindAsync(user.Id);
            Assert.Null(deletedUser);
        }
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
    }
}
