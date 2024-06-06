using CommurideModels.DTOs.AppUser;
using CommurideModels.DTOs.Vehicle;
using Models;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace CommurideTests
{
    [Trait("Category", "VehicleTests")]
public class VehicleTests : IClassFixture<CommurideWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CommurideWebApplicationFactory<Program> _factory;

    public VehicleTests(CommurideWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    // Use of TheoryData for strongly typed data tests
    public static TheoryData<CreateVehicleDTO> CreateTestData = new TheoryData<CreateVehicleDTO>() {
        new CreateVehicleDTO { VehicleId = 666, Registration = "SG-267-ZT", Brand = "BrandTest", Model = "ModelTest", Category = (Models.Vehicle.CategoryVehicle)1, CO2=10, Motorization=(Models.Vehicle.MotorizationVehicle)2, NbPlaces = 2, Status = (Models.Vehicle.StatusVehicle)1, URLPhoto = "urltest" },
    };

    [Theory]
    [MemberData(nameof(CreateTestData))]
    public async Task CreateVehicle(CreateVehicleDTO vehicleDTO)
    {
        // Authorization ADMIN
        var loginDTO = new LoginDTO() { Password = "admin", Username = "ADMIN" };
        var loginContent = JsonSerializer.Serialize(loginDTO);
        var loginStringContent = new StringContent(loginContent, encoding: Encoding.UTF8, mediaType: "application/json");
        var loginResponse = await _client.PostAsync("api/Auth/Login", loginStringContent);
        
        // Arrange
        string registerStringContent = JsonSerializer.Serialize(vehicleDTO);
        var content = new StringContent(registerStringContent, encoding: Encoding.UTF8, mediaType: "application/json");

        // Act
        var response = await _client.PostAsync("api/Vehicle/PostVehicle", content);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType!.ToString());
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<Vehicle>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.NotNull(responseObject);
        Assert.Equal(vehicleDTO.Registration, responseObject!.Registration);
        Assert.Equal(vehicleDTO.Brand, responseObject.Brand);
        Assert.Equal(vehicleDTO.Model, responseObject.Model);
        Assert.Equal(vehicleDTO.Category, responseObject.Category);
        Assert.Equal(vehicleDTO.CO2, responseObject.CO2);
        Assert.Equal(vehicleDTO.Motorization, responseObject.Motorization);
        Assert.Equal(vehicleDTO.NbPlaces, responseObject.NbPlaces);
        Assert.Equal(vehicleDTO.Status, responseObject.Status);
        Assert.Equal(vehicleDTO.URLPhoto, responseObject.URLPhoto);
    }

    [Fact]
    public async Task GetAllVehicles()
    {
        // Authorization ADMIN
        var loginDTO = new LoginDTO() { Password = "admin", Username = "ADMIN" };
        var loginContent = JsonSerializer.Serialize(loginDTO);
        var loginStringContent = new StringContent(loginContent, encoding: Encoding.UTF8, mediaType: "application/json");
        var loginResponse = await _client.PostAsync("api/Auth/Login", loginStringContent);

        // Arrange
        var response = await _client.GetAsync("api/Vehicle/GetAllVehicles");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType!.ToString());
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<List<Vehicle>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.NotNull(responseObject);
    }

    [Fact]
    public async Task GetVehicleById()
    {
        // Authorization ADMIN
        var loginDTO = new LoginDTO() { Password = "admin", Username = "ADMIN" };
        var loginContent = JsonSerializer.Serialize(loginDTO);
        var loginStringContent = new StringContent(loginContent, encoding: Encoding.UTF8, mediaType: "application/json");
        var loginResponse = await _client.PostAsync("api/Auth/Login", loginStringContent);

        // Arrange
        var createVehicleResponse = await _client.PostAsync("api/Vehicle/PostVehicle", new StringContent("{\"Registration\": \"SG-267-ZT\", \"Brand\": \"BrandTest\", \"Model\": \"ModelTest\", \"Category\": 1, \"CO2\": 10, \"Motorization\": 2, \"NbPlaces\": 2, \"Status\": 1, \"URLPhoto\": \"urltest\"}", Encoding.UTF8, "application/json"));
        createVehicleResponse.EnsureSuccessStatusCode();
        var createdVehicleId = JsonSerializer.Deserialize<Vehicle>(await createVehicleResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!.Id;
        var getVehicleResponse = await _client.GetAsync($"api/Vehicle/GetVehicle/{createdVehicleId}");

        // Assert
        getVehicleResponse.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", getVehicleResponse.Content.Headers.ContentType!.ToString());
        var responseString = await getVehicleResponse.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<Vehicle>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.NotNull(responseObject);
        Assert.Equal(createdVehicleId, responseObject!.Id);
        Assert.Equal("SG-267-ZT", responseObject.Registration);
        Assert.Equal("BrandTest", responseObject.Brand);
        Assert.Equal("ModelTest", responseObject.Model);
        Assert.Equal((Models.Vehicle.CategoryVehicle)1, responseObject.Category);
        Assert.Equal(10, responseObject.CO2);
        Assert.Equal((Models.Vehicle.MotorizationVehicle)2, responseObject.Motorization);
        Assert.Equal(2, responseObject.NbPlaces);
        Assert.Equal((Models.Vehicle.StatusVehicle)1, responseObject.Status);
        Assert.Equal("urltest", responseObject.URLPhoto);
    }

[Fact]
public async Task UpdateVehicle()
{
    // Authorization ADMIN
    var loginDTO = new LoginDTO() { Password = "admin", Username = "ADMIN" };
    var loginContent = JsonSerializer.Serialize(loginDTO);
    var loginStringContent = new StringContent(loginContent, encoding: Encoding.UTF8, mediaType: "application/json");
    var loginResponse = await _client.PostAsync("api/Auth/Login", loginStringContent);

    // Arrange
    var originalVehicle = new CreateVehicleDTO
    {
        VehicleId = 100,
        Registration = "SG-267-ZT",
        Brand = "BrandTest",
        Model = "ModelTest",
        Category = (Models.Vehicle.CategoryVehicle)1,
        CO2 = 10,
        Motorization = (Models.Vehicle.MotorizationVehicle)2,
        NbPlaces = 2,
        Status = (Models.Vehicle.StatusVehicle)1,
        URLPhoto = "urltest"
    };

    // Create the original vehicle
    string originalVehicleStringContent = JsonSerializer.Serialize(originalVehicle);
    var originalContent = new StringContent(originalVehicleStringContent, encoding: Encoding.UTF8, mediaType: "application/json");
    var createResponse = await _client.PostAsync("api/Vehicle/PostVehicle", originalContent);
    createResponse.EnsureSuccessStatusCode();
    var createdVehicleId = JsonSerializer.Deserialize<Vehicle>(await createResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!.Id;

    // Update the vehicle
    var updatedVehicle = new UpdateVehicleDTO
    {
        Id = createdVehicleId,
        Brand = "UpdatedBrand",
        Model = "UpdatedModel",
        Category = (Models.Vehicle.CategoryVehicle)2,
        CO2 = 15,
        Motorization = (Models.Vehicle.MotorizationVehicle)1,
        NbPlaces = 4,
        Status = (Models.Vehicle.StatusVehicle)2,
        URLPhoto = "updatedurl"
    };
    string updatedVehicleStringContent = JsonSerializer.Serialize(updatedVehicle);
    var updatedContent = new StringContent(updatedVehicleStringContent, encoding: Encoding.UTF8, mediaType: "application/json");

    // Act
    var updateResponse = await _client.PutAsync($"api/Vehicle/UpdateVehicle/{createdVehicleId}", updatedContent);

    // Assert
    updateResponse.EnsureSuccessStatusCode();
    Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);
}

    [Theory]
    [MemberData(nameof(CreateTestData))]
    public async Task DeleteVehicle(CreateVehicleDTO vehicleDTO)
    {
        // Authorization ADMIN
        var loginDTO = new LoginDTO() { Password = "admin", Username = "ADMIN" };
        var loginContent = JsonSerializer.Serialize(loginDTO);
        var loginStringContent = new StringContent(loginContent, encoding: Encoding.UTF8, mediaType: "application/json");
        var loginResponse = await _client.PostAsync("api/Auth/Login", loginStringContent);

        // Arrange
        var createVehicleResponse = await _client.PostAsync("api/Vehicle/PostVehicle", new StringContent("{\"Registration\": \"SG-267-ZT\", \"Brand\": \"BrandTest\", \"Model\": \"ModelTest\", \"Category\": 1, \"CO2\": 10, \"Motorization\": 2, \"NbPlaces\": 2, \"Status\": 1, \"URLPhoto\": \"urltest\"}", Encoding.UTF8, "application/json"));
        createVehicleResponse.EnsureSuccessStatusCode();
        var createdVehicleId = JsonSerializer.Deserialize<Vehicle>(await createVehicleResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!.Id;

        // Act
        var deleteResponse = await _client.DeleteAsync($"api/Vehicle/DeleteVehicle/Delete/{createdVehicleId}");

        // Assert
        deleteResponse.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }
}
}