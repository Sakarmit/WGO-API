using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace WGO_API.Models.MarkerModel
{
    public class MarkerDTO
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public string? Summary { get; set; }
        public required int UserId { get; set; }
        [Range(-180, 180)]
        public required float longitude { get; set; }
        [Range(-90, 90)]
        public required float latitude { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime EndTime { get; set; }

    }
}