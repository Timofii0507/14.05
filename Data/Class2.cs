using ClassLibrary;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataAccess
{
    public class GameDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-GNKQNCJ\\SQLEXPRESS;Database=GameLibrary;Integrated Security=True;TrustServerCertificate=True;");
        }
    }
}