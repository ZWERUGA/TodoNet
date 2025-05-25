using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todo.Models;

namespace Todo.Data
{
    public class TodoDbContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
    {
        public DbSet<TodoModel> Todos { get; set; }
    }
}
