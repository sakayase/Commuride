﻿using CommurideModels.DbContexts;
using CommurideModels.DTOs.AppUser;
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
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace CommurideTests
{
    /// <summary>
    /// Integration tests of rent controller
    /// </summary>
    /// <param name="factory"></param>
    /// <param name="output"></param>
    public class RentTest(CommurideWebApplicationFactory<Program> factory, ITestOutputHelper output) : IClassFixture<CommurideWebApplicationFactory<Program>>
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

        internal async Task<HttpResponseMessage> LoginTestUser(LoginDTO loginDTO)
        {
            var loginStringContent = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, mediaType: "application/json");
            return await _client.PostAsync("api/Auth/Login", loginStringContent);
        }

        internal void CleanDB ()
        {
            using (var scope = factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationDbContext>();

                Utilities.ReinitializeDbForTests(db);
            }
        }

        /// <summary>
        /// Test the creation of a rent
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SuccessCreateARent()
        {
            //Reinitialiser la db avant le test
            CleanDB();

            // login with createduser in dbcontext
            await LoginTestUser(loginDTO);

            // Create a new rent 
            DateTime startDate = DateTime.Now.AddMonths(1);
            PostRentDTO postRentDTO = new PostRentDTO() { EndDate = startDate.AddDays(7), StartDate = startDate, VehicleId = 1 };
            var resp = await CreateARent(postRentDTO);
            // Deserialise the resp to get the id
            var rent = await JsonSerializer.DeserializeAsync<Rent>(await resp.Content.ReadAsStreamAsync(), options);
            Assert.NotNull(rent);
            Assert.Equal(rent!.Vehicle.Id, postRentDTO.VehicleId);

            // It should pass
            Assert.True(resp.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
        }


        /// <summary>
        /// Test the fail of a creation of a rent if its date is intersecting with another rent
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task FailCreateARent()
        {
            DateTime startDate = DateTime.Now.AddMonths(1);
            PostRentDTO postRentDTO = new PostRentDTO() { EndDate = startDate.AddDays(7), StartDate = startDate, VehicleId = 1 };

            // Clean DB, login and create a new Rent at DateTime.Now.AddMonths(1)
            CleanDB();
            await LoginTestUser(loginDTO);
            await CreateARent(postRentDTO);

            // Create a new rent at the same dates
            var resp = await CreateARent(postRentDTO);

            // It should fail
            Assert.False(resp.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.InternalServerError, resp.StatusCode);
        }

        /// <summary>
        /// Test the succes of an update
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SuccessUpdateARent()
        {
            DateTime startDate = DateTime.Now.AddMonths(1);
            PostRentDTO postRentDTO = new PostRentDTO() { EndDate = startDate.AddDays(7), StartDate = startDate, VehicleId = 1 };

            // Clean DB, login and create a new Rent at DateTime.Now.AddMonths(1)
            CleanDB();
            await LoginTestUser(loginDTO);
            var postResp = await CreateARent(postRentDTO);


            var rent = await JsonSerializer.DeserializeAsync<Rent>(await postResp.Content.ReadAsStreamAsync(), options);
            Assert.NotNull(rent);

            // Update the created Rent
            DateTime newStartDate = DateTime.Now.AddMonths(1).AddDays(1);
            UpdateRentDTO updateRentDTO = new UpdateRentDTO() {  StartDate = newStartDate };
            var resp = await _client.PutAsync($"api/Rent/UpdateRent/{rent!.Id}", new StringContent(JsonSerializer.Serialize(updateRentDTO), encoding: Encoding.UTF8, mediaType: "application/json"));



            // It should pass
            Assert.True(resp.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        }

        /// <summary>
        /// Test the failure of an update with wrong dates
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task FailUpdateARent()
        {
            DateTime startDate = DateTime.Now.AddMonths(1);
            PostRentDTO postRentDTO = new PostRentDTO() { EndDate = startDate.AddDays(7), StartDate = startDate, VehicleId = 1 };
            PostRentDTO postRentDTO2 = new PostRentDTO() { EndDate = startDate.AddDays(16), StartDate = startDate.AddDays(8), VehicleId = 1 }; // create a second rent after the first


            // Clean DB, login and create a new Rent at DateTime.Now.AddMonths(1)
            CleanDB();
            await LoginTestUser(loginDTO);
            var postResp1 = await CreateARent(postRentDTO);
            var postResp2 = await CreateARent(postRentDTO2);

            var rent1 = await JsonSerializer.DeserializeAsync<Rent>(await postResp1.Content.ReadAsStreamAsync(), options);
            var rent2 = await JsonSerializer.DeserializeAsync<Rent>(await postResp2.Content.ReadAsStreamAsync(), options);
            Assert.NotNull(rent1);
            Assert.NotNull(rent2);

            // Update the first rent with dates intersecting the second rent
            DateTime newStartDate = DateTime.Now.AddMonths(1).AddDays(9);
            UpdateRentDTO updateRentDTO = new UpdateRentDTO() { StartDate = newStartDate, EndDate = newStartDate.AddDays(12) }; 
            var resp = await _client.PutAsync($"api/Rent/UpdateRent/{rent1!.Id}", new StringContent(JsonSerializer.Serialize(updateRentDTO), encoding: Encoding.UTF8, mediaType: "application/json"));
            
            //It should fail
            Assert.False(resp.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.InternalServerError, resp.StatusCode);
        }

        /// <summary>
        /// Test the success of a deletion
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SuccessDeleteARent()
        {
            DateTime startDate = DateTime.Now.AddMonths(1);
            PostRentDTO postRentDTO = new PostRentDTO() { EndDate = startDate.AddDays(7), StartDate = startDate, VehicleId = 1 };

            // Clean DB, login and create a new Rent at DateTime.Now.AddMonths(1)
            CleanDB();
            await LoginTestUser(loginDTO);
            var postResp = await CreateARent(postRentDTO);

            // Deserialise the resp to get the id
            var rent = await JsonSerializer.DeserializeAsync<Rent>(await postResp.Content.ReadAsStreamAsync(), options);
            Assert.NotNull(rent);

            // Delete the created Rent
            var resp = await _client.DeleteAsync($"api/Rent/DeleteRent/{rent!.Id}");
            // It should pass
            Assert.True(resp.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, resp.StatusCode);
            
        }

        /// <summary>
        /// Test the failure of a deletion if you're not the user of the rent
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task FailDeleteARent()
        {
            DateTime startDate = DateTime.Now.AddMonths(1);
            PostRentDTO postRentDTO = new PostRentDTO() { EndDate = startDate.AddDays(7), StartDate = startDate, VehicleId = 1 };

            // Clean DB, login and create a new Rent at DateTime.Now.AddMonths(1)
            CleanDB();
            await LoginTestUser(loginDTO);
            var postResp = await CreateARent(postRentDTO);

            //Logout
            await _client.GetAsync("api/Auth/Logout");

            // Deserialise the resp to get the id
            var rent = await JsonSerializer.DeserializeAsync<Rent>(await postResp.Content.ReadAsStreamAsync(), options);
            Assert.NotNull(rent);

            // Delete the created Rent
            var resp = await _client.DeleteAsync($"api/Rent/DeleteRent/{rent!.Id}");

            // It should fail
            Assert.False(resp.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, resp.StatusCode);
        }
    }
}