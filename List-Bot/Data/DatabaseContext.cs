using System;
using List_Bot.Models;
using Microsoft.EntityFrameworkCore;

namespace List_Bot.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<List> Lists { get; set; }
        public DbSet<ListItem> ListItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #if DEBUG
                Console.WriteLine($"Using './appdata/ListDatabase.db'");
                optionsBuilder.UseSqlite("Data Source=./appdata/ListDatabase.db");
#else
                Console.WriteLine($"Using '../appdata/ListDatabase.db'");
                optionsBuilder.UseSqlite("Data Source=../appdata/ListDatabase.db");
#endif
        }
    }
}
