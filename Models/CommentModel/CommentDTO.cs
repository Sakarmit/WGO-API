namespace WGO_API.Models.CommentModel
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime DateTime { get; set; }
        public required string Message { get; set; }
    }
}
