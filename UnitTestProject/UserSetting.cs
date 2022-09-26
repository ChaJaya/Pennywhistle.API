using Newtonsoft.Json;
using Pennywhistle.Domain.Common;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace UnitTestProject
{
    public class UserSetting
    {
        public static string loginUserName = "adminuser";
        public static string loginPassword = "1234";
        public static HttpClient SetToken(HttpClient client, string userName, string password)
        {
            var loginData = new Pennywhistle.Application.Common.Models.Login
            {
                UserName = userName,
                Password = password
            };
            var json = JsonConvert.SerializeObject(loginData);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = client.PostAsync("api/Login/Login", data);
            var content = response.Result.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserManagerResponse>(content.Result);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);

            return client;
        }
    }
}
