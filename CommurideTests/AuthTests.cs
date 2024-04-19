using CommurideModels.DTOs.AppUser;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Runtime.CompilerServices;
using Xunit;

namespace CommurideTests
{
    public class AuthTests(CommurideWebApplicationFactory<Program> factory) : IClassFixture<CommurideWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();
        DateTime now = DateTime.UtcNow;

        [Fact]
        public async Task TestMethod1()
        {
            var response = await _client.GetAsync("api/Auth/GetAppUser");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

       /* [Theory]
        [InlineData(new CreateAppUserDTO() { BirthDate = DateTime.MinValue, Password = "pass", Password2 = "pass", Username = "test_user" })]
        public async Task Register(CreateAppUserDTO userDTO)
        {
            

        }*/
    }
}