using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Chat.Persistence.Entities;

namespace Chat.Persistence.Contexts
{
    public sealed class DefaultContext : IdentityDbContext<User>
    {
        public DefaultContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Message> Messages { get; set; }
    }
}
