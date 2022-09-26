using Microsoft.AspNetCore.Mvc.Testing;
using Pennywhistle.API;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace UnitTestProject.Report
{
    public class ReportTest : WebApplicationFactory<Startup>
    {
        private readonly HttpClient _client;

        int orderStatus = 5;
        string date = "2022-09-24";
        string userId = "97A39C84-3629-40D7-B166-4191BC611EBE";
        public ReportTest()
        {
            WebApplicationFactory<Startup> application = new WebApplicationFactory<Startup>();
            _client = application.CreateClient();
            UserSetting.SetToken(_client, UserSetting.loginUserName, UserSetting.loginPassword);
        }

        [Fact]
        public async Task Get_Order_Report_By_Admin()
        {
            var response = await _client.GetAsync("api/Report/GetOrderReport?date=" + date +"&" + "orderStatus=" + orderStatus);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Get_Customer_Order_History_By_Admin()
        {
            var response = await _client.GetAsync("api/Report/GetOrderHistoryForCustomerReport?userId=" + userId);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Get_Customer_Details_Report_By_Admin()
        {
            var response = await _client.GetAsync("api/Report/GetCustomerDetailsReport");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
