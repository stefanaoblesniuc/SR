//using Ardalis.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using MovieApp.Entities;
//using SmartEnum.EFCore;

namespace MovieApp.DataBase;

public class MovieAppDbContext : DbContext
{
    public MovieAppDbContext(DbContextOptions<MovieAppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<FavoriteMovie> FavoriteMovies { get; set; }
    public DbSet<PreferenceForm> PreferenceForms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Username).IsRequired().HasMaxLength(255);
            entity.Property(u => u.Password).IsRequired();
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Title).IsRequired().HasMaxLength(255);
            entity.Property(m => m.Genre).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Movie>()
                    .Property(m => m.Premiere)
                    .HasColumnType("text");
            entity.Property(m => m.IMDBScore).IsRequired();
            entity.Property(m => m.Runtime);
            entity.Property(m => m.Language).IsRequired().HasMaxLength(50);
        });
        modelBuilder.Entity<FavoriteMovie>()
        .HasKey(fm => fm.Id);
        modelBuilder.Entity<FavoriteMovie>()
            .HasOne(fm => fm.User)
            .WithMany(u => u.FavoriteMovies)
            .HasForeignKey(fm => fm.UserId);

        modelBuilder.Entity<FavoriteMovie>()
            .HasOne(fm => fm.Movie)
            .WithMany(m => m.FavoriteMovies)
            .HasForeignKey(fm => fm.MovieId);

        modelBuilder.Entity<PreferenceForm>()
            .HasOne(pf => pf.User)
            .WithMany(u => u.PreferenceForms)
            .HasForeignKey(pf => pf.UserId)
            .OnDelete(DeleteBehavior.Cascade);  // Optional: cascade delete

        // Optional: Define index for UserId in PreferenceForm table
        modelBuilder.Entity<PreferenceForm>()
            .HasIndex(pf => pf.UserId)
            .HasDatabaseName("idx_userid");
    }
}
