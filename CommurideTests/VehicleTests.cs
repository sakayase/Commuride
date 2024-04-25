using CommurideModels.DTOs.Vehicle;
using Models;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace CommurideTests
{
    [Trait("Category", "VehicleController")]
public class VehicleControllerTests : IClassFixture<CommurideWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CommurideWebApplicationFactory<Program> _factory;

    public VehicleControllerTests(CommurideWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    // Use of TheoryData for strongly typed data tests
    public static TheoryData<CreateVehicleDTO> CreateTestData = new TheoryData<CreateVehicleDTO>() {
        new CreateVehicleDTO { VehicleId = 100, Registration = "SG-267-ZT", Brand = "Porsche", Model = "GT3RS", Category = (Models.Vehicle.CategoryVehicle)1, CO2=10, Motorization=(Models.Vehicle.MotorizationVehicle)2, NbPlaces = 2, Status = (Models.Vehicle.StatusVehicle)1, URLPhoto = "urltest" },
        new CreateVehicleDTO { VehicleId = 101, Registration = "JS-090-MQ", Brand = "Opel", Model = "Agila", Category = (Models.Vehicle.CategoryVehicle)1, CO2=1, Motorization=(Models.Vehicle.MotorizationVehicle)2, NbPlaces = 5, Status = (Models.Vehicle.StatusVehicle)1, URLPhoto = "urltest" },
    };

    [Theory]
    [MemberData(nameof(CreateTestData))]
    public async Task CreateVehicle(CreateVehicleDTO vehicleDTO)
    {
        // Arrange
        string registerStringContent = JsonSerializer.Serialize(vehicleDTO);
        var content = new StringContent(registerStringContent, encoding: Encoding.UTF8, mediaType: "application/json");

        // Act
        var response = await _client.PostAsync("api/Vehicle/PostVehicle", content);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<Vehicle>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.NotNull(responseObject);
        Assert.Equal(vehicleDTO.Registration, responseObject.Registration);
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
    public async Task GetVehicles_ReturnsOkResult_WithListOfVehicles()
    {
        // Arrange
        var response = await _client.GetAsync("api/Vehicle/GetAllVehicles");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<List<Vehicle>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.NotNull(responseObject);
    }

    [Fact]
    public async Task GetVehicle_ReturnsOkResult_WithVehicle()
    {
        // Arrange
        var createVehicleResponse = await _client.PostAsync("api/Vehicle/PostVehicle", new StringContent("{\"Registration\": \"SG-267-ZT\", \"Brand\": \"Porsche\", \"Model\": \"GT3RS\", \"Category\": 1, \"CO2\": 10, \"Motorization\": 2, \"NbPlaces\": 2, \"Status\": 1, \"URLPhoto\": \"urltest\"}", Encoding.UTF8, "application/json"));
        createVehicleResponse.EnsureSuccessStatusCode();
        var createdVehicleId = JsonSerializer.Deserialize<Vehicle>(await createVehicleResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).Id;
        var getVehicleResponse = await _client.GetAsync($"api/Vehicle/GetVehicle/{createdVehicleId}");

        // Assert
        getVehicleResponse.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", getVehicleResponse.Content.Headers.ContentType.ToString());
        var responseString = await getVehicleResponse.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<Vehicle>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.NotNull(responseObject);
        Assert.Equal(createdVehicleId, responseObject.Id);
        Assert.Equal("SG-267-ZT", responseObject.Registration);
        Assert.Equal("Porsche", responseObject.Brand);
        Assert.Equal("GT3RS", responseObject.Model);
        Assert.Equal((Models.Vehicle.CategoryVehicle)1, responseObject.Category);
        Assert.Equal(10, responseObject.CO2);
        Assert.Equal((Models.Vehicle.MotorizationVehicle)2, responseObject.Motorization);
        Assert.Equal(2, responseObject.NbPlaces);
        Assert.Equal((Models.Vehicle.StatusVehicle)1, responseObject.Status);
        Assert.Equal("urltest", responseObject.URLPhoto);
    }



    [Theory]
    [MemberData(nameof(CreateTestData))]
    public async Task DeleteVehicle(CreateVehicleDTO vehicleDTO)
    {
        // Arrange
        var createVehicleResponse = await _client.PostAsync("api/Vehicle/PostVehicle", new StringContent("{\"Registration\": \"SG-267-ZT\", \"Brand\": \"Porsche\", \"Model\": \"GT3RS\", \"Category\": 1, \"CO2\": 10, \"Motorization\": 2, \"NbPlaces\": 2, \"Status\": 1, \"URLPhoto\": \"urltest\"}", Encoding.UTF8, "application/json"));
        createVehicleResponse.EnsureSuccessStatusCode();
        var createdVehicleId = JsonSerializer.Deserialize<Vehicle>(await createVehicleResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).Id;

        // Act
        var deleteResponse = await _client.DeleteAsync($"api/Vehicle/DeleteVehicle/Delete/{createdVehicleId}");

        // Assert
        deleteResponse.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }
}
}