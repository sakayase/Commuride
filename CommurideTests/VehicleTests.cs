using CommurideModels.DTOs.Vehicle;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace CommurideTests
{
    public class VehicleTests(CommurideWebApplicationFactory<Program> factory, ITestOutputHelper output) : IClassFixture<CommurideWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();
        private readonly ITestOutputHelper _output = output;

        // Use of TheoryData for strongly typed data tests
        public static TheoryData<CreateVehicleDTO> registerTestData = new TheoryData<CreateVehicleDTO>() {
            new () { VehicleId = 100, Registration = "SG-267-ZT", Brand = "Porsche", Model = "GT3RS", Category = (Models.Vehicle.CategoryVehicle)1, CO2=10, Motorization=(Models.Vehicle.MotorizationVehicle)2, NbPlaces = 2, Status = (Models.Vehicle.StatusVehicle)1, URLPhoto = "urltest" },
            new () { VehicleId = 101, Registration = "JS-090-MQ", Brand = "Opel", Model = "Agila", Category = (Models.Vehicle.CategoryVehicle)1, CO2=1, Motorization=(Models.Vehicle.MotorizationVehicle)2, NbPlaces = 5, Status = (Models.Vehicle.StatusVehicle)1, URLPhoto = "urltest" },
        };

        [Theory]
        [MemberData(nameof(registerTestData))]
        public async Task CreateVehicle(CreateVehicleDTO vehicleDTO)
        {
            //REGISTER
            string registerStringContent = JsonSerializer.Serialize(vehicleDTO);
            var content = new StringContent(registerStringContent, encoding: Encoding.UTF8, mediaType: "application/json");
            var response = await _client.PostAsync("api/Vehicle/PostVehicle", content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}