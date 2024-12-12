using BlogAPI.Models.Author;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.DbContext;

public class AuthorDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public AuthorDbContext(DbContextOptions<AuthorDbContext> options) : base(options)
    {
    }

    public DbSet<AuthorDto> Authors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthorDto>(entity =>
        {
            entity.ToTable("author", "fias");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FullName).HasColumnName("full_name");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Posts).HasColumnName("posts");
            entity.Property(e => e.Likes).HasColumnName("likes");
            entity.Property(e => e.Created).HasColumnName("created");
        });
    }
}