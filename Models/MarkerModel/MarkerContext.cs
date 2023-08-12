using Microsoft.EntityFrameworkCore;

namespace WGO_API.Models.MarkerModel
{
    public class MarkerContext : DbContext
    {
        public MarkerContext(DbContextOptions<MarkerContext> options) : base(options)
        {

        }

        public DbSet<Marker> Markers { get; set; } = null!;
    }
}
