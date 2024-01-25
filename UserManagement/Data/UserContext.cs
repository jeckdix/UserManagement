using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<SessionClass> SessionClasses { get; set; }
        public DbSet<Session>  Session {  get; set; } 
    }
}
