using Microsoft.EntityFrameworkCore;

namespace Rallypoint.Models
{
    public class RallypointContext : DbContext
    {
        public RallypointContext(DbContextOptions<RallypointContext> options) : base(options) {}
    }
}