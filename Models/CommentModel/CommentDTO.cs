using System.ComponentModel.DataAnnotations;
using WGO_API.Utils;

namespace WGO_API.Models.CommentModel
{
    public class CommentDTO
    {
        public required int Id { get; set; }
        public required int MarkerId { get; set; }
        public required string UserName { get; set; } = String.Empty;
        public DateTime DateTime { get; set; }

        [MinLength(1)]
        [MaxLength(100)]
        public required string Message { get; set; }
    }
}
