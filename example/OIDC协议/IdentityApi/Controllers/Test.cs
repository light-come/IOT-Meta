using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Test : ControllerBase
    {
        /// <summary>
        /// 开放获取token  API 接口
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetToken")]
        public async Task<string> GetToken()
        {

            var httpClient = new HttpClient();
            var disco = httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = "http://localhost:7776",
                Policy =
                {
                     RequireHttps = false
                }
            }).Result;
            if (disco.IsError)
            {
                return (disco.Error);
            }
            var tokenResponse = httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "123",
                ClientSecret = "123",
                Scope = "invoice.read"
            });
            return tokenResponse.Result.AccessToken;
        }


        [HttpGet("Get2")]
        public string Get2(int id)
        {
            return "Get2";
        }
        [Authorize]//加权限
        [HttpGet("Get3")]
        public IEnumerable<WeatherForecast> Get3()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
