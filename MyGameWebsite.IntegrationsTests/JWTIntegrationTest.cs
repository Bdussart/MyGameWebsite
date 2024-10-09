using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using AngularApp1.Server;
using Newtonsoft.Json;
using System.Text;
using Xunit.Sdk;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace MyGameWebsite.IntegrationsTests
{
    public class JWTIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private string _url = "/api/Auth/Login";

        public JWTIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Test_Good_Auth_Admin()
        {
            var log = new
            {

                Name = "admin",
                Password = "password"
            };


            var jsonContent = JsonConvert.SerializeObject(log); // Utilise System.Text.Json si tu préfères
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(_url, httpContent);

            response.EnsureSuccessStatusCode();

            string jwt =  await response.Content.ReadAsStringAsync();

            Assert.False(String.IsNullOrEmpty(jwt));
        }


        [Fact]
        public async Task Test_Good_Auth_User()
        {

            var log = new
            {

                Name = "user",
                Password = "password"
            };


            var jsonContent = JsonConvert.SerializeObject(log); // Utilise System.Text.Json si tu préfères
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(_url, httpContent);

            response.EnsureSuccessStatusCode();

            string jwt = await response.Content.ReadAsStringAsync();

            Assert.False(String.IsNullOrEmpty(jwt));
        }


        [Fact]
        public async Task Test_Good_Auth_Test()
        {

            var log = new
            {

                Name = "test",
                Password = "password"
            };


            var jsonContent = JsonConvert.SerializeObject(log); // Utilise System.Text.Json si tu préfères
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(_url, httpContent);

            response.EnsureSuccessStatusCode();

            string jwt = await response.Content.ReadAsStringAsync();

            Assert.False(String.IsNullOrEmpty(jwt));
        }

        [Fact]
        public async Task Test_Bad_Auth()
        {
            var log = new
            {
                Name = "test42",
                Password = "password"
            };

            var jsonContent = JsonConvert.SerializeObject(log); // Utilise System.Text.Json si tu préfères
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(_url, httpContent);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

            string errorMessage = await response.Content.ReadAsStringAsync();

            Assert.True(errorMessage.Equals( "Invalid user credentials.", StringComparison.CurrentCultureIgnoreCase));
        }


        [Fact]
        public async Task Test_Invalid_Query()
        {
            var log = new
            {
                NameName = "test42",
                Password = "password"
            };

            var jsonContent = JsonConvert.SerializeObject(log); // Utilise System.Text.Json si tu préfères
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(_url, httpContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}