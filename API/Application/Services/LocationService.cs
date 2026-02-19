using API.Domain.Entities;
using System.Text.Json;


namespace API.Application.Services;

public class LocationService : ILocationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<LocationService> _logger;
    private const string NominatimBaseUrl = "https://nominatim.openstreetmap.org";

    public LocationService(HttpClient httpClient, ILogger<LocationService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        
        // Set user agent as required by Nominatim usage policy
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("EventsManagementApp/1.0");
    }

    public async Task<Location> GetLocationDataAsync(double latitude, double longitude)
    {
        var location = new Location
        {
            Latitude = latitude,
            Longitude = longitude
        };

        try
        {
            // Fetch English data
            var englishData = await FetchLocationDataAsync(latitude, longitude, "en");
            if (englishData != null)
            {
                location.DisplayNameEn = englishData.GetProperty("display_name").GetString();
                
                if (englishData.TryGetProperty("address", out var addressEn))
                {
                    location.CityEn = GetAddressComponent(addressEn, "city", "town", "village");
                    location.StateEn = GetAddressComponent(addressEn, "state", "province");
                    location.CountryEn = GetAddressComponent(addressEn, "country");
                    location.RoadEn = GetAddressComponent(addressEn, "road", "street");
                    location.CountryCode = GetAddressComponent(addressEn, "country_code");
                    location.Postcode = GetAddressComponent(addressEn, "postcode");
                }

                if (englishData.TryGetProperty("osm_id", out var osmId))
                    location.OsmId = osmId.GetInt64();
                
                if (englishData.TryGetProperty("osm_type", out var osmType))
                    location.OsmType = osmType.GetString();
                
                if (englishData.TryGetProperty("place_id", out var placeId))
                    location.PlaceId = placeId.GetInt64();
            }

            // Fetch Arabic data
            var arabicData = await FetchLocationDataAsync(latitude, longitude, "ar");
            if (arabicData != null)
            {
                location.DisplayNameAr = arabicData.GetProperty("display_name").GetString();
                
                if (arabicData.TryGetProperty("address", out var addressAr))
                {
                    location.CityAr = GetAddressComponent(addressAr, "city", "town", "village");
                    location.StateAr = GetAddressComponent(addressAr, "state", "province");
                    location.CountryAr = GetAddressComponent(addressAr, "country");
                    location.RoadAr = GetAddressComponent(addressAr, "road", "street");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching location data for coordinates ({Latitude}, {Longitude})", latitude, longitude);
            // Return location with just coordinates if geocoding fails
        }

        return location;
    }

    private async Task<JsonElement?> FetchLocationDataAsync(double latitude, double longitude, string language)
    {
        try
        {
            var url = $"{NominatimBaseUrl}/reverse?lat={latitude}&lon={longitude}&format=json&accept-language={language}";
            
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(jsonString);
            
            return jsonDocument.RootElement;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch {Language} location data", language);
            return null;
        }
    }

    private string? GetAddressComponent(JsonElement address, params string[] keys)
    {
        foreach (var key in keys)
        {
            if (address.TryGetProperty(key, out var value))
            {
                var stringValue = value.GetString();
                if (!string.IsNullOrWhiteSpace(stringValue))
                    return stringValue;
            }
        }
        return null;
    }
}
