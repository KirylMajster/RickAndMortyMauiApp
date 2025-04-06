using Microsoft.EntityFrameworkCore;
using RickAndMortyMauiApp.Models;
using System.IO;

namespace RickAndMortyMauiApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<CharacterEpisode> CharacterEpisodes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine("C:\\Users\\antek\\Desktop\\semestr6\\Net_i_Java\\RickAndMortyMauiApp", "rickandmorty.db");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterEpisode>()
                .HasKey(ce => new { ce.CharacterId, ce.EpisodeId });

            modelBuilder.Entity<CharacterEpisode>()
                .HasOne(ce => ce.Character)
                .WithMany(c => c.CharacterEpisodes)
                .HasForeignKey(ce => ce.CharacterId);

            modelBuilder.Entity<CharacterEpisode>()
                .HasOne(ce => ce.Episode)
                .WithMany(e => e.CharacterEpisodes)
                .HasForeignKey(ce => ce.EpisodeId);
        }
    }
}
