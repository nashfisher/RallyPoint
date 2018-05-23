using System;
using Microsoft.EntityFrameworkCore;

namespace Rallypoint.Models
{
    public class RallypointContext : DbContext
    {
        public  RallypointContext(DbContextOptions<RallypointContext> options) : base(options) { }
        public DbSet<User> Users {get; set;}
        public DbSet<Game> Games {get; set;}
        public DbSet<Post> Posts {get; set;}
        public DbSet<Like> Likes {get; set;}

        public DbSet<Comment> Comments {get;set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Game>().HasOne(g => g.playerone).WithMany(u => u.gamescreated);
            builder.Entity<Game>().HasOne(g => g.playertwo).WithMany(u => u.gamesjoined);
        }
    }
}