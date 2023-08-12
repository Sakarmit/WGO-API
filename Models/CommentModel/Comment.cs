namespace WGO_API.Models.CommentModel
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime DateTime { get; set; }
        public required string Message { get; set; }
        public int ReportCount { get; set; }
    }
}
