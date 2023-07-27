namespace WGO_API.Models
{
    public class Marker
    {
        public required long Id { get; set; }
        public byte reportCount { get; set; }
        public required string User { get; set; }

        public DateTime DateTime { get; set; }

        public string? Title { get; set; }

        public string? Summary { get; set; }

    }
}