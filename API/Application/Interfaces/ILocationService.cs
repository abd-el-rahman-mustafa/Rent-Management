using API.Domain.Entities;

namespace API.Application.Interfaces;

public interface ILocationService
{
    /// <summary>
    /// Fetches location data from geocoding service based on latitude and longitude
    /// Returns bilingual data (Arabic and English)
    /// </summary>
    Task<Location> GetLocationDataAsync(double latitude, double longitude);
}
