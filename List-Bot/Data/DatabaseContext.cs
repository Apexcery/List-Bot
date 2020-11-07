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
                optionsBuilder.UseSqlite("Data Source=./appdata/ListDatabase.db");
            #else
                optionsBuilder.UseSqlite("Data Source=../appdata/ListDatabase.db");
            #endif
        }
    }
}
