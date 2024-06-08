using CommurideModels.DbContexts;
using CommurideModels.DTOs.AppUser;
using CommurideModels.DTOs.Carpool;
using CommurideModels.DTOs.Rent;
using CommurideModels.DTOs.Vehicle;
using CommurideTests.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Models;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CommurideTests
{
    /// <summary>
    /// Integration tests of carpool controller
    /// </summary>
    /// <param name="factory"></param>
    /// <param name="output"></param>
    public class CarpoolTest(CommurideWebApplicationFactory<Program> factory, ITestOutputHelper output) : IClassFixture<CommurideWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();
        private readonly ITestOutputHelper _output = output;
        LoginDTO loginDTO = new LoginDTO() { Password = "admin", Username = "admin" };
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        internal async Task<HttpResponseMessage> CreateARent(PostRentDTO postRentDTO)
        {

            return await _client.PostAsync("api/Rent/PostRent", new StringContent(JsonSerializer.Serialize(postRentDTO), encoding: Encoding.UTF8, mediaType: "application/json"));
        }

        internal async Task<HttpResponseMessage> CreateACarpool(PostCarpoolDTO postCarpoolDTO)
        {

            return await _client.PostAsync("api/Carpool/PostCarpool", new StringContent(JsonSerializer.Serialize(postCarpoolDTO), encoding: Encoding.UTF8, mediaType: "application/json"));
        }

		/// <summary>
		/// Login user, populate the authorization header
		/// </summary>
		/// <param name="loginDTO"></param>
		/// <returns></returns>
		internal async Task<HttpResponseMessage> LoginTestUser(LoginDTO loginDTO)
		{
			var loginStringContent = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, mediaType: "application/json");
			var res = await _client.PostAsync("api/Auth/Login", loginStringContent);
			var content = await res.Content.ReadAsStringAsync();
			String token = content.Split(' ')[1];
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			return res;
		}

		/// <summary>
		/// Logout connected user, cleans the token in the authorization header
		/// </summary>
		/// <returns></returns>
		internal async Task<HttpResponseMessage> LogoutTestUser()
		{
			var res = await _client.GetAsync("api/Auth/Logout");
			var content = await res.Content.ReadAsStringAsync();
			_client.DefaultRequestHeaders.Authorization = null;
			return res;
		}

		internal void CleanDB()
        {
            using (var scope = factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationDbContext>();

                Utilities.ReinitializeDbForTests(db);
            }
        }

        /// <summary>
        /// Test the creation of a carpool
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SuccessCreateACarpool()
        {
            //Reinitialiser la db avant le test
            CleanDB();

            // login with createduser in dbcontext
            await LoginTestUser(loginDTO);


            // Create a new rent and a new carpool 
            DateTime startDate = DateTime.Now.AddMonths(1);
            PostRentDTO postRentDTO = new PostRentDTO() { EndDate = startDate.AddDays(7), StartDate = startDate, VehicleId = 1 };
            PostCarpoolDTO postCarpoolDTO = new PostCarpoolDTO() { AddressArrival = "Lens", AddressLeaving = "Lille", DateDepart = startDate, vehicleId = 1 };
            var resptest = await CreateARent(postRentDTO);
            var resp = await CreateACarpool(postCarpoolDTO);

            // Deserialise the resp to get the id
            var carpool = await JsonSerializer.DeserializeAsync<Carpool>(await resp.Content.ReadAsStreamAsync(), options);

            Assert.NotNull(carpool);
            Assert.Equal(carpool!.Vehicle.Id, postCarpoolDTO.vehicleId);

            // It should pass
            Assert.True(resp.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
        }


        /// <summary>
        /// Test the fail of a creation of a carpool its vehicle doesnt belong to the user
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task FailCreateACarpool()
        {
            // ARRANGE
            DateTime startDate = DateTime.Now.AddMonths(1);
            PostRentDTO postRentDTO = new PostRentDTO() { EndDate = startDate.AddDays(7), StartDate = startDate, VehicleId = 1 };
            PostCarpoolDTO postCarpoolDTO = new PostCarpoolDTO() { AddressArrival = "Lens", AddressLeaving = "Lille", DateDepart = startDate.AddDays(8), vehicleId = 2 };
            
            // Clean DB, login and create a new rent at DateTime.Now.AddMonths(1) and ends 7 days later
            CleanDB();
            await LoginTestUser(loginDTO);
            await CreateARent(postRentDTO);

            // ACT
            // Create a new carpool with a vehicle that doesnt belong to the user at the time (doesnt register a rent at the carpool date)
            var resp = await CreateACarpool(postCarpoolDTO);

            // ASSERT
            // It should fail
            Assert.False(resp.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
        }

        /// <summary>
        /// Test the success of an update (test new date and new vehicle)
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SuccessUpdateACarpool()
        {
            // ARRANGE
            DateTime startDate = DateTime.Now.AddMonths(1);
            PostRentDTO postRentDTO1 = new PostRentDTO() { EndDate = startDate.AddDays(7), StartDate = startDate, VehicleId = 1 };
            PostRentDTO postRentDTO2 = new PostRentDTO() { EndDate = startDate.AddDays(14), StartDate = startDate.AddDays(7), VehicleId = 2 };
            PostCarpoolDTO postCarpoolDTO = new PostCarpoolDTO() { AddressArrival = "Lens", AddressLeaving = "Lille", DateDepart = startDate, vehicleId = 1 };

            // Clean DB, login and create two new rents, with vehicle 1 and 2 and different dates, and a carpool with 1st vehicle
            CleanDB();
            await LoginTestUser(loginDTO);
            await CreateARent(postRentDTO1);
            await CreateARent(postRentDTO2);
            var postResp = await CreateACarpool(postCarpoolDTO);
            var carpool = await JsonSerializer.DeserializeAsync<Carpool>(await postResp.Content.ReadAsStreamAsync(), options);
            Assert.NotNull(carpool);

            // ACT
            // Update the created Carpool with a new vehicle and a new date in between the new rent dates
            DateTime newStartDate = startDate.AddDays(10);
            UpdateCarpoolDTO updateCarpoolDTO = new UpdateCarpoolDTO() { dateDepart = newStartDate, vehicleId = 2 };
            var resp = await _client.PutAsync($"api/Carpool/UpdateCarpool/{carpool!.Id}", new StringContent(JsonSerializer.Serialize(updateCarpoolDTO), encoding: Encoding.UTF8, mediaType: "application/json"));
            var updatedCarpool = await JsonSerializer.DeserializeAsync<Carpool>(await resp.Content.ReadAsStreamAsync(), options);

            // ASSERT
            Assert.NotNull(updatedCarpool);
            Assert.Equal(carpool.Id, updatedCarpool!.Id);
            Assert.Equal(updateCarpoolDTO.vehicleId, updatedCarpool.Vehicle.Id);
            Assert.Equal(updateCarpoolDTO.dateDepart, updatedCarpool.DateDepart);
            Assert.True(resp.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        }

        /// <summary>
        /// Test the failure of an update with wrong dates or wrong vehicle 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task FailUpdateACarpool()
        {
            // ARRANGE
            DateTime startDate = DateTime.Now.AddMonths(1);
            PostRentDTO postRentDTO = new PostRentDTO() { EndDate = startDate.AddDays(7), StartDate = startDate, VehicleId = 1 };
            PostCarpoolDTO postCarpoolDTO = new PostCarpoolDTO() { AddressArrival = "Lens", AddressLeaving = "Lille", DateDepart = startDate, vehicleId = 1 };

            // Clean DB, login and create two new rents, with vehicle 1 and a carpool with vehicle 1
            CleanDB();
            await LoginTestUser(loginDTO);
            await CreateARent(postRentDTO: postRentDTO);
            var postResp = await CreateACarpool(postCarpoolDTO);

            // Deserialize carpool to get id
            var carpool = await JsonSerializer.DeserializeAsync<Carpool>(await postResp.Content.ReadAsStreamAsync(), options);
            Assert.NotNull(carpool);

            // ACT
            // Update the created Carpool with a new vehicle or a new date outside rented time
            UpdateCarpoolDTO updateCarpoolDTO1 = new UpdateCarpoolDTO() { dateDepart = startDate, vehicleId = 2 };
            UpdateCarpoolDTO updateCarpoolDTO2 = new UpdateCarpoolDTO() { dateDepart = startDate.AddYears(1), vehicleId = 1 };

            var resp1 = await _client.PutAsync($"api/Carpool/UpdateCarpool/{carpool!.Id}", new StringContent(JsonSerializer.Serialize(updateCarpoolDTO1), encoding: Encoding.UTF8, mediaType: "application/json"));
            var resp2 = await _client.PutAsync($"api/Carpool/UpdateCarpool/{carpool!.Id}", new StringContent(JsonSerializer.Serialize(updateCarpoolDTO2), encoding: Encoding.UTF8, mediaType: "application/json"));

            // ASSERT
            //It should fail
            Assert.False(resp1.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, resp1.StatusCode);
            Assert.False(resp2.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, resp2.StatusCode);


        }

        /// <summary>
        /// Test the success of a deletion
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SuccessDeleteACarpool()
        {
            // ARRANGE
            DateTime startDate = DateTime.Now.AddMonths(1);
            PostRentDTO postRentDTO = new PostRentDTO() { EndDate = startDate.AddDays(7), StartDate = startDate, VehicleId = 1 };
            PostCarpoolDTO postCarpoolDTO = new PostCarpoolDTO() { AddressArrival = "Lens", AddressLeaving = "Lille", DateDepart = startDate, vehicleId = 1 };

            // Clean DB, login and create a new Carpool at DateTime.Now.AddMonths(1)
            CleanDB();
            await LoginTestUser(loginDTO);
            await CreateARent(postRentDTO: postRentDTO);
            var postResp = await CreateACarpool(postCarpoolDTO);
            
            // Deserialise the resp to get the id
            var carpool = await JsonSerializer.DeserializeAsync<Carpool>(await postResp.Content.ReadAsStreamAsync(), options);
            Assert.NotNull(carpool);

            // ACT
            // Delete the created Carpool
            var resp = await _client.DeleteAsync($"api/Carpool/DeleteCarpool/{carpool!.Id}");
            
            // ASSERT
            // It should pass
            Assert.True(resp.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, resp.StatusCode);

        }

        /// <summary>
        /// Test the failure of a deletion if you're not the user of the carpool
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task FailDeleteACarpool()
        {
            // ARRANGE
            DateTime startDate = DateTime.Now.AddMonths(1);
            PostRentDTO postRentDTO = new PostRentDTO() { EndDate = startDate.AddDays(7), StartDate = startDate, VehicleId = 1 };
            PostCarpoolDTO postCarpoolDTO = new PostCarpoolDTO() { AddressArrival = "Lens", AddressLeaving = "Lille", DateDepart = startDate, vehicleId = 1 };

            // Clean DB, login and create a new Carpool at DateTime.Now.AddMonths(1)
            CleanDB();
            await LoginTestUser(loginDTO);
            await CreateARent(postRentDTO: postRentDTO);
            var postResp = await CreateACarpool(postCarpoolDTO);

            //Logout
            await LogoutTestUser();

            // Deserialise the resp to get the id
            var carpool = await JsonSerializer.DeserializeAsync<Carpool>(await postResp.Content.ReadAsStreamAsync(), options);
            Assert.NotNull(carpool);

            // ACT
            // Delete the created Carpool
            var resp = await _client.DeleteAsync($"api/Carpool/DeleteCarpool/{carpool!.Id}");

            // ASSERT
            // It should fail
            Assert.False(resp.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, resp.StatusCode);
        }
    }
}