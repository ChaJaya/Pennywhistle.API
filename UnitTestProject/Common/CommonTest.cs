using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Pennywhistle.API;
using Pennywhistle.Application.CommonFunction.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTestProject.Common
{
    public class CommonTest : WebApplicationFactory<Startup>
    {
        private readonly HttpClient _client;
        int orderId = 4;
        public CommonTest()
        {
            WebApplicationFactory<Startup> application = new WebApplicationFactory<Startup>();
            _client = application.CreateClient();
            UserSetting.SetToken(_client, UserSetting.loginUserName, UserSetting.loginPassword);
        }

        [Fact]
        public async Task Get_Order_Detailby_System_User()
        {
            var response = await _client.GetAsync("api/Common/GetOrderDetail?orderId=" + orderId);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Update_Order_Status_By_Staff()
        {
            var order = new UpdateOrderStatusCommand
            {
              CustomerEmail ="cha@gmail.com",
              LoggedInUserRole ="Admin",
              OrderId = orderId,
              OrderStatus =5
            };
            var json = JsonConvert.SerializeObject(order);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Common/UpdateOrderStatus", data);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
