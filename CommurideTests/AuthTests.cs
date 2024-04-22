using CommurideModels.DTOs.AppUser;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace CommurideTests
{
    public class AuthTests(CommurideWebApplicationFactory<Program> factory, ITestOutputHelper output) : IClassFixture<CommurideWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();
        private readonly ITestOutputHelper _output = output;

        // Use of TheoryData for strongly typed data tests
        public static TheoryData<CreateAppUserDTO> registerTestData = new TheoryData<CreateAppUserDTO>() {
            new () { Username = "test_user", Password = "pass", Password2 = "pass" },
            new () { Username = "test_user2", Password = "pass", Password2 = "pass" },
            new () { Username = "test_user3", Password = "pass", Password2 = "pass" },
            new () { Username = "test_user4", Password = "pass", Password2 = "pass" }
        };

        [Theory]
        [MemberData(nameof(registerTestData))]
        public async Task RegisterLoginLogout(CreateAppUserDTO appUserDTO)
        {
            //REGISTER
            string registerStringContent = JsonSerializer.Serialize(appUserDTO);
            var content = new StringContent(registerStringContent, encoding: Encoding.UTF8, mediaType: "application/json");
            var response = await _client.PostAsync("api/Auth/Register", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            //LOGIN
            var loginDTO = new LoginDTO() { Password = appUserDTO.Password, Username = appUserDTO.Username };
            var loginContent = JsonSerializer.Serialize(loginDTO);
            var loginStringContent = new StringContent(loginContent, encoding: Encoding.UTF8, mediaType: "application/json");
            var loginResponse = await _client.PostAsync("api/Auth/Login", loginStringContent);
            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            Assert.Contains(appUserDTO.Username, loginResponseContent);

            //LOGOUT
            var responseLogout = await _client.GetAsync("api/Auth/Logout");
            var contentLogout = await responseLogout.Content.ReadAsStringAsync();
            Assert.Contains("User Disconnected", contentLogout);
        }
    }
}