using CommurideModels.DTOs.AppUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CommurideTests
{
    public class AuthTests(CommurideWebApplicationFactory<Program> factory, ITestOutputHelper output) : IClassFixture<CommurideWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();
        private readonly ITestOutputHelper _output = output;

        // Use of TheoryData for strongly typed data tests
        public static TheoryData<CreateAppUserDTO> data = new TheoryData<CreateAppUserDTO>() {
           /* new CreateAppUserDTO() { Username = "test_user", Password = "pass", Password2 = "pass", BirthDate = new DateTime() },
            new CreateAppUserDTO() { Username = "test_user2", Password = "pass", Password2 = "pass", BirthDate = new DateTime() },
            new CreateAppUserDTO() { Username = "test_user3", Password = "pass", Password2 = "pass", BirthDate = new DateTime() },*/
            new CreateAppUserDTO() { Username = "test_user4", Password = "pass", Password2 = "pass", BirthDate = new DateTime() }
        };

        [Fact]
        public async Task TestMethod1()
        {
            var response = await _client.GetAsync("api/Auth/GetAppUser");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [MemberData(nameof(data))]
        public async Task Register(CreateAppUserDTO appUserDTO)
        {
            string stringContent = JsonSerializer.Serialize(appUserDTO);
            var content = new StringContent(stringContent, encoding: Encoding.UTF8, mediaType: "application/json");
            _output.WriteLine(await content.ReadAsStringAsync());
            var response = await _client.PostAsync("api/Auth/Register", content);
            await Console.Out.WriteLineAsync(await response.Content.ReadAsStringAsync());
            _output.WriteLine(await response.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
    }
}