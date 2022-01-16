using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VehicleTracking.Models;
using VehicleTracking.Models.GoogleGeoCode;
using VehicleTracking.Models.Vehicles;

namespace VehicleTracking.Core.GoogleAPIs
{
    public class GeoCoding
    {
        public GeoCoding(IConfiguration config, ILogger<Vehicle> logger)
        {
            _logger = logger;

            _baseUrl = config.GetSection("AppConfiguration").GetValue<string>("GoogleApiBaseUrl").ToString();
            _apiKey = config.GetSection("AppConfiguration").GetValue<string>("GoogleApiKey").ToString();

            _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
            _httpClient.DefaultRequestHeaders.TransferEncodingChunked = false;
        }

        public async Task<BaseLocation> GetGoogleGeoAsync(BaseLocation location)
        {
            var googleResult = await GetGeoCodeResult(location);

            if (googleResult == null)
            {
                _logger.LogError("Google request returned null");
                return null;
            }

            location.Name = googleResult.Results[0].Formatted_Address;

            return location;
        }

        public async Task<List<BaseLocation>> GetGoogleGeoRange(List<BaseLocation> locations)
        {
            foreach(var location in locations)
            {
                var googleResult = await GetGeoCodeResult(location);

                if (googleResult == null)
                {
                    _logger.LogError("Google request returned null");
                    return null;
                }

                location.Name = googleResult.Results[0].Formatted_Address;
            }

            return locations;
        }

        private async Task<GoogleGeoCode> GetGeoCodeResult(BaseLocation location)
        {
            string baseUri = _baseUrl +
                "?latlng={0},{1}&key={2}";
            var url = string.Format(baseUri, location.Lat, location.Lng, _apiKey);

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseBytes = await response.Content.ReadAsByteArrayAsync();
                var responseString = Encoding.UTF8.GetString(responseBytes).Replace("@attributes", "attributes");
                var journeyDetails = JsonConvert.DeserializeObject<GoogleGeoCode>(responseString);

                if (journeyDetails == null || responseString == "{}")
                {
                    _logger.LogInformation("Google request returned empty result");
                    return null;
                }

                return journeyDetails;
            }

            return null;
        }

        private readonly ILogger _logger;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly HttpClient _httpClient;
    }
}
