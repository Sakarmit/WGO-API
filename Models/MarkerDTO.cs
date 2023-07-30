using Microsoft.CodeAnalysis;

namespace WGO_API.Models
{
    public class MarkerDTO
    {
        public required long Id { get; set; }
        public required string User { get; set; }
        public required decimal longitude { get; set; }
        public required decimal latitude { get; set; }
        public DateTime DateTime { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }

    }
}