using Microsoft.EntityFrameworkCore;
using SkineroMotors.Core.Models;

namespace SkineroMotors.Persistence
{
    public class SMDbContext: DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<User> Users { get; set; }
        public SMDbContext (DbContextOptions<SMDbContext> options) : base (options) {}
    }
}