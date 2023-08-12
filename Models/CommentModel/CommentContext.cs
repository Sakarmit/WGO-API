using Microsoft.EntityFrameworkCore;

namespace WGO_API.Models.CommentModel
{
    public class CommentContext : DbContext
    {
        public CommentContext(DbContextOptions<CommentContext> options) : base(options)
        {

        }

        public DbSet<Comment> Comments { get; set; } = null!;
    }
}
