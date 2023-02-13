using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Services.Client
{
    public class UserClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public UserClient(IHttpClientFactory httpClient)
        {
           _httpClientFactory = httpClient;
        }

        public void GetUserId(Guid userId)
        {
            HttpClient client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri("http://localhost:5159");
            HttpResponseMessage response =client.GetAsync($"api/user/{userId}").Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new NotFoundException("No user found with this id");
            }
            
        }
    }
}
