using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using UserManagemenService.DAL;
using UserManagemenService.Models;
using Xunit;

namespace UserManagemenService.Tests.Integration;
[Collection("Database collection")]
public class BasicTests 
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly DatabaseFixture _dbFixture;

    public BasicTests(WebApplicationFactory<Program> factory, DatabaseFixture fixture)
    {
        _factory = factory;
        _dbFixture = fixture;
    }

    [Fact]
    public async Task ShouldCreateNewUser()
    {
        // Arrange
        await _dbFixture.ResetDatabase();
        var client = _factory.CreateClient();

        // Act
        var body = new {
            name = "Marco Polo",
            birthdate = "1999-01-01"
        };
        var response = await client.PostAsJsonAsync("/api/users", body);

        // Assert
        if (response.IsSuccessStatusCode)
        {
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var returnedUser = await response.Content.ReadFromJsonAsync<User>();
            Assert.Equal("Marco Polo", returnedUser!.Name);
            Assert.Equal(0, returnedUser!.Birthdate.CompareTo(new DateOnly(1999, 1, 1)));
            Assert.True(returnedUser.Active);
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception(errorResponse);
        }
    }

    [Fact]
    public async Task ShouldGetAllActiveUsers()
    {
        // Arrange
        await _dbFixture.ResetDatabase();
        var client = _factory.CreateClient();
        using (var dbContext = new UserManagemenServiceContext(_dbFixture.dbContextOptions))
        {
            var userRepo = new UserRepository(dbContext);
            await userRepo.Insert(new User{
                Name = "Marco Polo",
                Birthdate = new DateOnly(1999, 1, 1),
                Active = true
            });
            await userRepo.Insert(new User{
                Name = "Albert Mac",
                Birthdate = new DateOnly(1999, 1, 1),
                Active = false
            });
        }


        // Act
        var response = await client.GetAsync("/api/users/");

        // Assert
        if (response.IsSuccessStatusCode)
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var returnedUsers = await response.Content.ReadFromJsonAsync<IEnumerable<User>>();
            Assert.Single(returnedUsers!);
            var returnedUser = returnedUsers!.First();
            Assert.Equal("Marco Polo", returnedUser!.Name);
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception(errorResponse);
        }
    }

    [Fact]
    public async Task ShouldUpdateUserActiveField()
    {
        // Arrange
        await _dbFixture.ResetDatabase();
        var client = _factory.CreateClient();
        User newUser;
        using (var dbContext = new UserManagemenServiceContext(_dbFixture.dbContextOptions))
        {
            var userRepo = new UserRepository(dbContext);
            newUser = await userRepo.Insert(new User{
                Name = "Marco Polo",
                Birthdate = new DateOnly(1999, 1, 1),
                Active = true
            });
        }


        // Act
        var body = new {
            active = false
        };
        var response = await client.PatchAsJsonAsync($"/api/users/{newUser.Id}", body);

        // Assert
        if (response.IsSuccessStatusCode)
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var returnedUser = await response.Content.ReadFromJsonAsync<User>();
            Assert.False(returnedUser!.Active);

            var userDetail = await client.GetFromJsonAsync<User>($"/api/users/{newUser.Id}");
            Assert.False(userDetail!.Active);
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception(errorResponse);
        }
    }

    [Fact]
    public async Task ShouldDeleteUser()
    {
        // Arrange
        await _dbFixture.ResetDatabase();
        var client = _factory.CreateClient();
        User newUser;
        using (var dbContext = new UserManagemenServiceContext(_dbFixture.dbContextOptions))
        {
            var userRepo = new UserRepository(dbContext);
            newUser = await userRepo.Insert(new User{
                Name = "Marco Polo",
                Birthdate = new DateOnly(1999, 1, 1),
                Active = true
            });
        }


        // Act
        var response = await client.DeleteAsync($"/api/users/{newUser.Id}");

        // Assert
        if (response.IsSuccessStatusCode)
        {
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var userDetailResponse = await client.GetAsync($"/api/users/{newUser.Id}");
            Assert.Equal(HttpStatusCode.NotFound, userDetailResponse.StatusCode);
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception(errorResponse);
        }
    }
}