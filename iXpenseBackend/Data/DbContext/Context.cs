using iXpenseBackend.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace iXpenseBackend.Data.DbContext
{
    public class Context : IdentityDbContext
    {
        public Context(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
