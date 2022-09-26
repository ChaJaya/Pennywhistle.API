using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Pennywhistle.API;
using Pennywhistle.Application.Customer.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTestProject.CustomerOrder
{
    public class CustomerOrderTest : WebApplicationFactory<Startup>
    {
        private readonly HttpClient _client;
        string userId = "97a39c84-3629-40d7-b166-4191bc611ebe";
        public CustomerOrderTest()
        {
            WebApplicationFactory<Startup> application = new WebApplicationFactory<Startup>();
            _client = application.CreateClient();
            UserSetting.SetToken(_client, UserSetting.loginUserName, UserSetting.loginPassword);
        }

        [Fact]
        public async Task Customer_Create_Order()
        {
            var order = new CreateOrderCommand
            {
               UserId = Guid.Parse(userId),
               Sku = "ItemSku",
               ProductItemId=1,
               Name = "CheasePizza",
               Size ="Small",
               TotalCost = 100

            };
            var json = JsonConvert.SerializeObject(order);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/CustomerOrder/CreateOrder", data);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Fact]
        public async Task Customer_Get_Order_History()
        {
           
            var response = await _client.GetAsync("api/CustomerOrder/GetOrderHistoryForCustomer?userId=" + userId);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Customer_Get_Current_Order()
        {
            var response = await _client.GetAsync("api/CustomerOrder/GetCurrentOrderForCustomer?userId=" + userId);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
