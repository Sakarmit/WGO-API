using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace WGO_API.Models.MarkerModel
{
    public class MarkerDTO
    {
        public required int Id { get; set; }
        public required string UserName { get; set; }
        [Required]
        [MinLength(10)]
        public required string Title { get; set; }
        public string Summary { get; set; } = string.Empty;
        [Required]
        [Range(-180, 180)]
        public required float longitude { get; set; }
        [Required]
        [Range(-90, 90)]
        public required float latitude { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime EndTime { get; set; }

    }
}