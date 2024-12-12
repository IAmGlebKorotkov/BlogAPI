using BlogAPI.Models.Author;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.DbContext;

public class AuthorContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<AuthorFULLDto> Authors { get; set; }

    public AuthorContext(DbContextOptions<AuthorContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthorFULLDto>(entity =>
        {
            entity.ToTable("author", "fias");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(a => a.FullName).HasColumnName("full_name").IsRequired();
            entity.Property(a => a.id_user).HasColumnName("id_user").IsRequired();
            entity.Property(a => a.BirthDate).HasColumnName("birth_date");
            entity.Property(a => a.Gender).HasColumnName("gender").IsRequired();
            entity.Property(a => a.Posts).HasColumnName("posts").IsRequired();
            entity.Property(a => a.Likes).HasColumnName("likes").IsRequired();
            entity.Property(a => a.Created).HasColumnName("created").IsRequired();
        });
    }
}