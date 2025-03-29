
using System.IO;
using System.Data.SQLite;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core
{
    public class AppDbContext : DbContext
    {
        public DbSet<Theme> Theme { get; set; }


        public AppDbContext()
        {
            Database.EnsureCreated();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Poutanen", "LaunchPad");
            Directory.CreateDirectory(folderPath);
            string dbPath = Path.Combine(folderPath, "launchpad.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Theme>().HasData(new Theme());
        }
    }

}
