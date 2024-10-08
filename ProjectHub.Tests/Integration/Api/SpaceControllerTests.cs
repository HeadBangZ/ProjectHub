﻿using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ProjectHub.Application.DTOs.SpaceDtos;
using ProjectHub.Infrastructure.Data.Contexts;
using ProjectHub.Tests.Integration.Mocks;
using System.Net;
using System.Text;

namespace ProjectHub.Tests.Integration.Api;

public class SpaceControllerTests : IAsyncLifetime
{
    private ApiWebApplicationFactory _factory;
    private HttpClient _client;
    private readonly string version = "v1";

    [Fact]
    public async Task GetSpaces_ShoudlReturnOk()
    {
        var response = await _client.GetAsync($"api/{version}/spaces");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostSpace_ShouldReturnCreated()
    {

        var request = new CreateSpaceDtoRequest("Test Space", "Test Description");
        var json = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"api/{version}/spaces", json);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task PostSpace_ShouldReturnBadRequest()
    {

        var request = new CreateSpaceDtoRequest("", "Test Description");
        var json = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync($"api/{version}/spaces", json);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetSpaceById_ShouldReturnNotFound()
    {
        var id = Guid.NewGuid();
        var response = await _client.GetAsync($"api/{version}/spaces/{id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetSpaceById_ShouldReturnOk()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ProjectHubDbContext>();

            var spaces = SeedTestData.CreateMultipleEntityData(context, 1, SeedTestData.CreateSpaceData);
            var id = spaces.First().Id;

            var response = await _client.GetAsync($"api/{version}/spaces/{id.Id}");

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }

    [Fact]
    public async Task DeleteSpaceById_ShouldReturnNotFound()
    {
        var id = Guid.NewGuid();
        var response = await _client.DeleteAsync($"api/{version}/spaces/{id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteSpaceById_ShouldReturnNoContent()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ProjectHubDbContext>();

            var spaces = SeedTestData.CreateMultipleEntityData(context, 1, SeedTestData.CreateSpaceData);
            var id = spaces.First().Id;

            var response = await _client.DeleteAsync($"api/{version}/spaces/{id.Id}");

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }

    [Fact]
    public async void UpdateSpaceById_NotValidated_ShouldReturnBadRequest()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ProjectHubDbContext>();
            var spaces = SeedTestData.CreateMultipleEntityData(context, 1, SeedTestData.CreateSpaceData);

            var id = spaces.First().Id;
            var dto = new UpdateSpaceDtoRequest(id.Id, "a", null, null);
            var json = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"api/{version}/spaces/{id.Id}", json);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }

    [Fact]
    public async void UpdateSpaceById_IdMismatch_ShouldReturnBadRequest()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ProjectHubDbContext>();
            var spaces = SeedTestData.CreateMultipleEntityData(context, 1, SeedTestData.CreateSpaceData);

            var id = spaces.First().Id;
            var dto = new UpdateSpaceDtoRequest(Guid.NewGuid(), "space updated", null, null);
            var json = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"api/{version}/spaces/{id.Id}", json);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }

    [Fact]
    public async void UpdateSpaceById_ShouldReturnNotFound()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ProjectHubDbContext>();
            var spaces = SeedTestData.CreateMultipleEntityData(context, 1, SeedTestData.CreateSpaceData);

            var id = Guid.NewGuid();
            var dto = new UpdateSpaceDtoRequest(id, "space updated", null, null);
            var json = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"api/{version}/spaces/{id}", json);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

    [Fact]
    public async void UpdateSpaceById_ShouldReturnNoContent()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ProjectHubDbContext>();
            var spaces = SeedTestData.CreateMultipleEntityData(context, 1, SeedTestData.CreateSpaceData);

            var id = spaces.First().Id;
            var dto = new UpdateSpaceDtoRequest(id.Id, "space updated", null, null);
            var json = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"api/{version}/spaces/{id.Id}", json);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }

    public async Task InitializeAsync()
    {
        _factory = new ApiWebApplicationFactory();
        _client = _factory.CreateClient();

        await Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ProjectHubDbContext>();
            db.Database.EnsureDeleted();
        }

        _client.Dispose();
        _factory.Dispose();

        await Task.CompletedTask;
    }
}
