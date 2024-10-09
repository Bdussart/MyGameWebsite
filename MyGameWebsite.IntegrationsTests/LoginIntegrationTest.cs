using MyGameWebsite.Server;
using MyGameWebsite.Server.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyGameWebsite.IntegrationsTests
{
    public class LoginIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private string _authUrl = "/api/Auth/Login";
        private string _userhUrl = "/api/Users";

        public LoginIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        private async Task<string> GetJWT(dynamic log)
        {

            string jsonContent = JsonConvert.SerializeObject(log); // Utilise System.Text.Json si tu préfères
            StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _factory.CreateClient().PostAsync(_authUrl, httpContent);
            response.EnsureSuccessStatusCode();

            JObject jsonObject = JObject.Parse(await response.Content.ReadAsStringAsync());



            return jsonObject["Token"].ToString();
        }

        [Fact]
        public async Task Test_Admin_Get_All_Users()
        {

            var log = new
            {
                Name = "admin",
                Password = "password"
            };

            string jwt = await GetJWT(log);

            HttpClient client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            HttpResponseMessage responseMessage = await client.GetAsync(_userhUrl);

            responseMessage.EnsureSuccessStatusCode();

            string data = responseMessage.Content.ReadAsStringAsync().Result;

            List<User>? users = JsonConvert.DeserializeObject<List<User>>(data);

            Assert.True(users is not null);
            Assert.True(users.Count > 0);            
        }

        [Fact]
        public async Task Test_Admin_Get_User()
        {
            var log = new
            {
                Name = "admin",
                Password = "password"
            };

            string jwt = await GetJWT(log);

            HttpClient client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            HttpResponseMessage responseMessage = await client.GetAsync($"{_userhUrl}/1");

            responseMessage.EnsureSuccessStatusCode();

            string data = responseMessage.Content.ReadAsStringAsync().Result;

            User? user = JsonConvert.DeserializeObject<User>(data);

            Assert.True(user is not null);
            Assert.True(user.Id == 1);
        }

        [Fact]
        public async Task Test_User_Get_All_Users()
        {

            var log = new
            {
                Name = "user",
                Password = "password"
            };

            string jwt = await GetJWT(log);

            HttpClient client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            HttpResponseMessage responseMessage = await client.GetAsync(_userhUrl);

            responseMessage.EnsureSuccessStatusCode();

            string data = responseMessage.Content.ReadAsStringAsync().Result;

            List<User>? users = JsonConvert.DeserializeObject<List<User>>(data);

            Assert.True(users is not null);
            Assert.True(users.Count > 0);
        }

        [Fact]
        public async Task Test_User_Not_Get_User()
        {
            var log = new
            {
                Name = "user",
                Password = "password"
            };

            string jwt = await GetJWT(log);

            HttpClient client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            HttpResponseMessage responseMessage = await client.GetAsync($"{_userhUrl}/1");

            Assert.Equal(HttpStatusCode.Forbidden, responseMessage.StatusCode);
        }

        [Fact]
        public async Task Test_Test_Get_All_Users()
        {

            var log = new
            {
                Name = "test",
                Password = "password"
            };

            string jwt = await GetJWT(log);

            HttpClient client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            HttpResponseMessage responseMessage = await client.GetAsync(_userhUrl);

            responseMessage.EnsureSuccessStatusCode();

            string data = responseMessage.Content.ReadAsStringAsync().Result;

            List<User>? users = JsonConvert.DeserializeObject<List<User>>(data);

            Assert.True(users is not null);
            Assert.True(users.Count > 0);
        }
    }
}
