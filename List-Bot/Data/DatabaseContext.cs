using List_Bot.Models;
using Microsoft.EntityFrameworkCore;

namespace List_Bot.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<List> Lists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=ListDatabase.db");
        }
    }
}
