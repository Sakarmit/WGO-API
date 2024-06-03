namespace WGO_API.Models.CommentModel
{
    public class Comment
    {
        public required int Id { get; set; }
        public required int MarkerId { get; set; }
        public string UserId { get; set; } = String.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public required string Message { get; set; }
        public int ReportCount { get; set; }
    }
}
