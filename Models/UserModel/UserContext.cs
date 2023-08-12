using Microsoft.EntityFrameworkCore;

namespace WGO_API.Models.UserModel
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; } = null!;
    }
}
