using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Pennywhistle.API;
using Pennywhistle.Application.Products.Commands;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTestProject.Product
{
    public class ProductTest : WebApplicationFactory<Startup>
    {
        private readonly HttpClient _client;
       
        public ProductTest()
        {
            WebApplicationFactory<Startup> application = new WebApplicationFactory<Startup>();
            _client = application.CreateClient();
            UserSetting.SetToken(_client,UserSetting.loginUserName, UserSetting.loginPassword);
        }

        [Fact]
        public async Task Admin_Create_Products()
        {
            var product = new CreateProductCommand
            {
               Name ="Test Product",
               Sku ="TestSKU",
               Size ="Small",
               Price = 10
            };
            var json = JsonConvert.SerializeObject(product);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Product/CreateProductItem", data);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Admin_Update_Product_Item()
        {
            var product = new UpdateProductCommand
            {
                Id = 10,
                Name = "Upate Test Product",
                Sku = "UpdateTestSKU",
                Size = "Update Small",
                Price = 200
            };
            var json = JsonConvert.SerializeObject(product);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Product/UpdateProductItem", data);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Admin_Get_All_Products()
        {
            var response = await _client.GetAsync("api/Product/GetAllProducts");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Admin_Delete_Product_Item()
        {
            int id = 10;
            var json = JsonConvert.SerializeObject(id);
            var data = new StringContent(json, Encoding.UTF8, "application/json");


            var response = await _client.PostAsync("api/Product/DeleteProductItem",data);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


    }
}
