using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Pennywhistle.API;
using Pennywhistle.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTestProject.Admin
{
    public class AdminTest : WebApplicationFactory<Startup>
    {
        private readonly HttpClient _client;

        public AdminTest()
        {
            WebApplicationFactory<Startup> application = new WebApplicationFactory<Startup>();
            _client = application.CreateClient();
            UserSetting.SetToken(_client, UserSetting.loginUserName, UserSetting.loginPassword);
        }

        [Fact]
        public async Task Admin_Create_CreateEmployeeUser()
        {
            var product = new Register
            {
               UserName ="TestUser",
               Email =  "testemail@gmail.com",
               Password ="1234",
               Role = "DeliveryStaff"
            };
            var json = JsonConvert.SerializeObject(product);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Admin/CreateEmployeeUser", data);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
