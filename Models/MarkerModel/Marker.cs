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
        public required float longitude { get; set; }
        public required float latitude { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ReportCount { get; set; }

    }
}