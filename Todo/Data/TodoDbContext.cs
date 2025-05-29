using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todo.Models;

namespace Todo.Data
{
    public class TodoDbContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            List<IdentityRole> roles =
            [
                new()
                {
                    Id = "0926f441-7f24-4401-901e-bf527771c8f2",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                },
                new()
                {
                    Id = "0d10bdb5-82e5-4740-9ea1-dfa5b0f21275",
                    Name = "User",
                    NormalizedName = "USER",
                },
            ];

            builder.Entity<IdentityRole>().HasData(roles);

            builder
                .Entity<TodoModel>()
                .HasOne(e => e.AppUser)
                .WithMany(e => e.Todos)
                .HasForeignKey(e => e.AppUserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }

        public DbSet<TodoModel> Todos { get; set; }
    }
}
