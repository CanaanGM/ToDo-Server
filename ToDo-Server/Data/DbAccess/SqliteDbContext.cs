using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using ToDo_Server.Data.Models;

namespace ToDo_Server.Data.DbAccess
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<ToDo> ToDos => Set<ToDo>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ToDo>()
                .HasOne(x => x.User)
                .WithMany(s => s.ToDos)
                .HasForeignKey(x => x.UserId);

            builder.Entity<AppUser>()
                .HasMany(s => s.ToDos)
                .WithOne(x => x.User);
        }
    }
}
