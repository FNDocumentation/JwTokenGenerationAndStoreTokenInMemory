using BookStore_API.Interfaces;
using BookStore_API.Static;
using Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_API.Services
{
    public class Authenticate : IAuthenticate
    {
        private readonly IHttpClientFactory _client;
        public Authenticate(IHttpClientFactory client)
        {
            _client = client;
        }


        public async Task<bool> Login(LoginModel user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post
                             , Endpoints.LonginEndpoint);

            request.Content = new StringContent(JsonConvert.SerializeObject(user)
                             , Encoding.UTF8, "application/json");

            var client = _client.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var content = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenResponse>(content);

            //Store Token

            //Change auth state of app


            return true;
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }
    }
}
