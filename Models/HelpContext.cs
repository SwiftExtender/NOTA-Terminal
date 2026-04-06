using Microsoft.EntityFrameworkCore;
using System;

namespace NOTATerminal.Models
{
    public class HelpContext : DbContext
    {
        public DbSet<Macros>? ScriptsTable { get; set; }
        public DbSet<FavoriteCommandModel>? FavoriteCommandsTable { get; set; }
        private string DbPath { get; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
        public HelpContext()
        {
            DbPath = System.IO.Path.Join(AppDomain.CurrentDomain.BaseDirectory, "scripts.db");
        }
    }
}
