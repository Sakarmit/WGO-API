namespace WGO_API.Models.ReportModel
{
    public class Report
    {
        public required int Id { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        /// <summary>
        /// Marker 0, Comment 1
        /// </summary>
        public int Type { get; set; }
        public string? Message { get; set; }
    }
}
