using Microsoft.EntityFrameworkCore;
using Todo.Models;

namespace Todo.Data
{
    public class TodoDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<TodoModel> Todos { get; set; }
    }
}
