using Microsoft.CodeAnalysis;

namespace WGO_API.Models.MarkerModel
{
    public class Marker
    {
        public required int Id { get; set; }
        public required string UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public required string Title { get; set; }
        public string Summary { get; set; } = string.Empty;
        public required float Longitude { get; set; }
        public required float Latitude { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ReportCount { get; set; }

    }
}