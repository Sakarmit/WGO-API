using Microsoft.EntityFrameworkCore;

namespace WGO_API.Models.ReportModel
{
    public class ReportContext : DbContext
    {
        public ReportContext() : base()
        {
        }

        public ReportContext(DbContextOptions<ReportContext> options) : base(options)
        {

        }

        public DbSet<Report> Reports { get; set; } = null!;
    }
}
