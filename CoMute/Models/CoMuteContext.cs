using Microsoft.EntityFrameworkCore;

namespace CoMute.Models
{
    public class CoMuteContext : DbContext
    {
        public CoMuteContext(DbContextOptions<CoMuteContext> options)
         : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<CarPoolOpportunity>  Opportunities { get; set; } = null!;
        public virtual DbSet<CarPool>  Pools { get; set; } = null!;
    }
}
