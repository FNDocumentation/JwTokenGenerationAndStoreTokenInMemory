using BookStore_API.Interfaces;
using BookStore_API.Static;
using Common.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_API.Services
{
    public class TokenService : ITokenService
    {

        private readonly IMemoryCache _cache;
        private readonly IHttpClientFactory _client;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public TokenService(IMemoryCache cache, 
                            IHttpClientFactory client,
                            JwtSecurityTokenHandler tokenHandler)
        {
            _cache = cache;
            _client = client;
            _tokenHandler = tokenHandler;
        }

        public async Task<string> FetchTokenAsync(LoginModel user)
        {
            //string token = string.Empty;
            TokenResponse tokenResponse;

            // if cache doesn't contain 
            // an entry called TOKEN
            // error handling mechanism is mandatory
            if (!_cache.TryGetValue(CacheKeys.Entry, out tokenResponse))
            {
                //var tokenmodel = JsonConvert.DeserializeObject<TokenModel>(this.GetTokenFromApi(user).Result.ToString());
                tokenResponse = await this.GetTokenFromApi(user);               

                // keep the value within cache for 
                // given amount of time
                // if value is not accessed within the expiry time
                // delete the entry from the cache           

                var tokenContent = _tokenHandler.ReadJwtToken(tokenResponse.Token);
                var tokenExpiry = tokenContent.ValidTo;

                DateTime currentDateTime = DateTime.Now;

                var options = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(tokenExpiry.Subtract(currentDateTime).Minutes))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(tokenExpiry.Subtract(currentDateTime).Minutes));                

                //_cache.Set("TOKEN", tokenResponse, options);
                _cache.Set(CacheKeys.Entry, tokenResponse, options);
            }

            return tokenResponse.Token;
        }

        public static class CacheKeys
        {
            public static string Entry { get { return "_Entry"; } }
            public static string CallbackEntry { get { return "_Callback"; } }
            public static string CallbackMessage { get { return "_CallbackMessage"; } }
            public static string Parent { get { return "_Parent"; } }
            public static string Child { get { return "_Child"; } }
            public static string DependentMessage { get { return "_DependentMessage"; } }
            public static string DependentCTS { get { return "_DependentCTS"; } }
            public static string Ticks { get { return "_Ticks"; } }
            public static string CancelMsg { get { return "_CancelMsg"; } }
            public static string CancelTokenSource { get { return "_CancelTokenSource"; } }
        }

        private async Task<TokenResponse> GetTokenFromApi(LoginModel user)
        {
            // get api implementation happens here
            // returns a token model

            var request = new HttpRequestMessage(HttpMethod.Post
                 , Endpoints.LonginEndpoint);

            request.Content = new StringContent(JsonConvert.SerializeObject(user)
                             , Encoding.UTF8, "application/json");

            var client = _client.CreateClient();
            HttpResponseMessage response = client.SendAsync(request).Result;

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenResponse>(content);

            return token;
        }

        //private TokenModel GetTokenFromApi(LoginModel user)
        //{
        //    // get api implementation happens here
        //    // returns a token model

        //    var request = new HttpRequestMessage(HttpMethod.Post
        //         , Endpoints.LonginEndpoint);

        //    request.Content = new StringContent(JsonConvert.SerializeObject(user)
        //                     , Encoding.UTF8, "application/json");

        //    var client = _client.CreateClient();
        //    HttpResponseMessage response = client.SendAsync(request).Result;

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        return null;
        //    }

        //    var content = response.Content.ReadAsStringAsync();
        //    var token = JsonConvert.DeserializeAnonymousType(content);

        //    return token;
        //}

    }
}
