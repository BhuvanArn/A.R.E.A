using Database;
using Database.Dao;
using Database.Entities;
using Database.Service;
using Microsoft.EntityFrameworkCore;

namespace DatabaseTests
{
    public class UserServiceTests : IDisposable
    {
        private readonly DatabaseContext _dbContext;
        private readonly DaoFactory _daoFactory;
        private readonly UserService _userService;
        
        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new DatabaseContext(options);

            _daoFactory = new DaoFactory(_dbContext);
            _userService = new UserService(_daoFactory);
        }

        [Fact]
        public async Task CreateUserAsync_WithValidUser_AddsUserToDatabase()
        {
            var user = new User
            {
                Username = "BasicUser",
                Email = "email@test.com",
                Password = "BasicPassword"
            };

            await _userService.CreateUserAsync(user);
            var createdUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == "BasicUser");
            Assert.NotNull(createdUser);
            Assert.Equal("email@test.com", createdUser.Email);
            Assert.Equal("BasicPassword", createdUser.Password);
        }

        [Fact]
        public async Task GetUserByIdAsync_WithExistingUser_ReturnsUser()
        {
            var user = new User
            {
                Username = "TestUser",
                Email = "testuser@test.com",
                Password = "TestPassword"
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            var retrievedUser = await _userService.GetUserByIdAsync(user.Id);

            Assert.NotNull(retrievedUser);
            Assert.Equal(user.Id, retrievedUser.Id);
            Assert.Equal("TestUser", retrievedUser.Username);
            Assert.Equal("testuser@test.com", retrievedUser.Email);
            Assert.Equal("TestPassword", retrievedUser.Password);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            var user1 = new User
            {
                Username = "User1",
                Email = "user1@test.com",
                Password = "Password1"
            };
            var user2 = new User
            {
                Username = "User2",
                Email = "user2@test.com",
                Password = "Password2"
            };
            _dbContext.Users.AddRange(user1, user2);
            await _dbContext.SaveChangesAsync();

            var users = await _userService.GetAllUsersAsync();

            Assert.NotNull(users);
            Assert.Equal(2, users.Count());
            var retrievedUser1 = users.FirstOrDefault(u => u.Username == "User1");
            var retrievedUser2 = users.FirstOrDefault(u => u.Username == "User2");
            Assert.NotNull(retrievedUser1);
            Assert.NotNull(retrievedUser2);

            Assert.Equal("user1@test.com", retrievedUser1.Email);
            Assert.Equal("Password1", retrievedUser1.Password);

            Assert.Equal("user2@test.com", retrievedUser2.Email);
            Assert.Equal("Password2", retrievedUser2.Password);
        }

        [Fact]
        public async Task UpdateUserAsync_WithExistingUser_UpdatesUserInDatabase()
        {
            var user = new User
            {
                Username = "UpdateUser",
                Email = "updateuser@test.com",
                Password = "OldPassword"
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            user.Email = "updatedemail@test.com";
            user.Password = "NewPassword";

            await _userService.UpdateUserAsync(user);

            var updatedUser = await _dbContext.Users.FindAsync(user.Id);
            Assert.NotNull(updatedUser);
            Assert.Equal("UpdateUser", updatedUser.Username);
            Assert.Equal("updatedemail@test.com", updatedUser.Email);
            Assert.Equal("NewPassword", updatedUser.Password);
        }

        [Fact]
        public async Task DeleteUserAsync_WithExistingUser_DeletesUserFromDatabase()
        {
            var user = new User
            {
                Username = "DeleteUser",
                Email = "deleteuser@test.com",
                Password = "DeletePassword"
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            await _userService.DeleteUserAsync(user.Id);

            var deletedUser = await _dbContext.Users.FindAsync(user.Id);
            Assert.Null(deletedUser);
        }

        [Fact]
        public async Task FindUsersAsync_WithMatchingUsers_ReturnsUsers()
        {
            var user1 = new User
            {
                Username = "FindUser1",
                Email = "finduser1@test.com",
                Password = "Password1"
            };
            var user2 = new User
            {
                Username = "FindUser2",
                Email = "finduser2@test.com",
                Password = "Password2"
            };
            var user3 = new User
            {
                Username = "OtherUser",
                Email = "otheruser@test.com",
                Password = "Password3"
            };
            _dbContext.Users.AddRange(user1, user2, user3);
            await _dbContext.SaveChangesAsync();

            var foundUsers = await _userService.FindUsersAsync(u => u.Username.StartsWith("Find"));

            Assert.NotNull(foundUsers);
            Assert.Equal(2, foundUsers.Count());
            var retrievedUser1 = foundUsers.FirstOrDefault(u => u.Username == "FindUser1");
            var retrievedUser2 = foundUsers.FirstOrDefault(u => u.Username == "FindUser2");
            Assert.NotNull(retrievedUser1);
            Assert.NotNull(retrievedUser2);

            Assert.Equal("finduser1@test.com", retrievedUser1.Email);
            Assert.Equal("Password1", retrievedUser1.Password);

            Assert.Equal("finduser2@test.com", retrievedUser2.Email);
            Assert.Equal("Password2", retrievedUser2.Password);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}