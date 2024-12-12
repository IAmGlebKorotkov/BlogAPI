using BlogAPI.Models;
using BlogAPI.Models.Author;
using BlogAPI.Models.Post;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.DbContext;
public class PostContext : Microsoft.EntityFrameworkCore.DbContext
{
    public PostContext(DbContextOptions<PostContext> options) : base(options)
    {
    }

    public DbSet<PostDto> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostDto>(entity =>
        {
            entity.ToTable("post", "fias"); // Указываем имя таблицы и схему
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).ValueGeneratedNever(); // Указываем, что Id не генерируется автоматически
            entity.Property(p => p.Title).IsRequired();
            entity.Property(p => p.Description).IsRequired();
            entity.Property(p => p.ReadingTime).IsRequired();
            entity.Property(p => p.CreateTime).IsRequired();
            entity.Property(p => p.AuthorId).IsRequired();
            entity.Property(p => p.Author).IsRequired();
            entity.Property(p => p.CommunityId).IsRequired();
            entity.Property(p => p.CommunityName).IsRequired();
            entity.Property(p => p.TagPosts).IsRequired();
        });
    }
} 