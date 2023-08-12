using Microsoft.CodeAnalysis;

namespace WGO_API.Models.MarkerModel
{
    public class MarkerDTO
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public string? Summary { get; set; }
        public required int UserId { get; set; }
        public required float longitude { get; set; }
        public required float latitude { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime EndTime { get; set; }

    }
}