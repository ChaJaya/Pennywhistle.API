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

namespace UnitTestProject.Login
{
    public class LoginTest : WebApplicationFactory<Startup>
    {
        private readonly HttpClient _client;

        string email = "test@gmail.com";
        string password = "1234";
        string userName = "randomusername";
        public LoginTest()
        {
            WebApplicationFactory<Startup> application = new WebApplicationFactory<Startup>();
            _client = application.CreateClient();
            UserSetting.SetToken(_client, UserSetting.loginUserName, UserSetting.loginPassword);
        }


        [Fact]
        public async Task Login_For_Any_Use_Type()
        {
            var login = new Pennywhistle.Application.Common.Models.Login
            {
                UserName = UserSetting.loginUserName,
                Password = UserSetting.loginPassword,
            };
            var json = JsonConvert.SerializeObject(login);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Login/Login", data);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Customer_Create_Account()
        {
            var customer = new CustomerRegsiter
            {
                Email = email,
                Password = password,
                UserName = userName
            };
            var json = JsonConvert.SerializeObject(customer);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Login/CreateCustomer", data);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
