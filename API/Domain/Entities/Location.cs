namespace API.Domain.Entities;
public class Location : SimpleBaseEntity
{
    // Coordinates
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    // English Properties
    public string? DisplayNameEn { get; set; }
    public string? CityEn { get; set; }
    public string? StateEn { get; set; }
    public string? CountryEn { get; set; }
    public string? RoadEn { get; set; }

    // Arabic Properties
    public string? DisplayNameAr { get; set; }
    public string? CityAr { get; set; }
    public string? StateAr { get; set; }
    public string? CountryAr { get; set; }
    public string? RoadAr { get; set; }

    // Common Properties
    public string? CountryCode { get; set; }
    public string? Postcode { get; set; }

    // OpenStreetMap Properties
    public long OsmId { get; set; }
    public string? OsmType { get; set; }
    public long PlaceId { get; set; }
}