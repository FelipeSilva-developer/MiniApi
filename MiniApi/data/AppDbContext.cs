using Microsoft.EntityFrameworkCore;
using MiniApi.models;

namespace MiniApi.data
{   
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Cliente>? Clientes { get; set; }
    }
}
