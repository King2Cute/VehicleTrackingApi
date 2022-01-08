using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace VehicleTracking.GoogleApi
{
    public class GoogleRoads
    {
        public GoogleRoads(IConfiguration config)
        {
            _baseUrl = config.GetSection("AppConfiguration").GetValue<string>("GoogleApiBaseUrl");
            _apiKey = config.GetSection("AppConfiguration").GetValue<string>("GoogleApiKey");

            _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
            _httpClient.DefaultRequestHeaders.TransferEncodingChunked = false;
        }

        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly HttpClient _httpClient;
    }
}
